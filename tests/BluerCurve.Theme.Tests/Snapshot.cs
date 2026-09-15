using Avalonia.Media.Imaging;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Styling;
using Avalonia.Threading;

namespace BluerCurve.Tests;

internal static class Snapshot
{
    public static string Directory { get; } =
        Environment.GetEnvironmentVariable("BLUERCURVE_SNAPSHOT_DIR")
        ?? Path.Combine(AppContext.BaseDirectory, "snapshots");

    public static string Capture(Window window, string name)
    {
        window.Show();
        var path = Render(window, name);
        window.RequestedThemeVariant = ThemeVariant.Dark;
        Render(window, name + "-dark");
        window.Close();
        return path;
    }

    private static string Render(Window window, string name)
    {
        Dispatcher.UIThread.RunJobs();
        AvaloniaHeadlessPlatform.ForceRenderTimerTick();
        Dispatcher.UIThread.RunJobs();
        using var frame = window.CaptureRenderedFrame() ?? throw new InvalidOperationException("No frame rendered");
        System.IO.Directory.CreateDirectory(Directory);
        var path = Path.Combine(Directory, name + ".png");
        frame.Save(path, PngBitmapEncoderOptions.Default);
        return path;
    }
}
