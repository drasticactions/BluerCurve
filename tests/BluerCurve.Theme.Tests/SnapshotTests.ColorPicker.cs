using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using Xunit;

namespace BluerCurve.Tests;

public class ColorPickerSnapshotTests
{
    private static Window ColorViewWindow(int tab)
    {
        var view = new ColorView
        {
            Color = Color.Parse("#4464ac"),
            SelectedIndex = tab,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(12),
        };
        var window = new Window { Content = view, Width = 380, Height = 420, SizeToContent = SizeToContent.Manual };
        window.Show();
        var deadline = DateTime.UtcNow + TimeSpan.FromSeconds(3);
        while (DateTime.UtcNow < deadline)
        {
            Thread.Sleep(50);
            Dispatcher.UIThread.RunJobs();
        }
        return window;
    }

    [AvaloniaFact]
    public void Render_ColorView_Spectrum_Tab()
    {
        var path = Snapshot.Capture(ColorViewWindow(0), "colorpicker-view-spectrum");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_ColorView_Palette_Tab()
    {
        var path = Snapshot.Capture(ColorViewWindow(1), "colorpicker-view-palette");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_ColorView_Components_Tab()
    {
        var path = Snapshot.Capture(ColorViewWindow(2), "colorpicker-view-components");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_ColorPicker_Button()
    {
        var panel = new StackPanel { Margin = new Thickness(12), Spacing = 8 };
        panel.Children.Add(new ColorPicker { Color = Color.Parse("#4464ac"), HorizontalAlignment = HorizontalAlignment.Left });
        panel.Children.Add(new ColorPicker { Color = Color.Parse("#80ff0000"), HorizontalAlignment = HorizontalAlignment.Left });
        panel.Children.Add(new ColorPicker { Color = Color.Parse("#4464ac"), HorizontalAlignment = HorizontalAlignment.Left, IsEnabled = false });
        var window = new Window { Content = panel, Width = 160, Height = 130, SizeToContent = SizeToContent.Manual };
        var path = Snapshot.Capture(window, "colorpicker-button");
        Assert.True(File.Exists(path));
    }
}
