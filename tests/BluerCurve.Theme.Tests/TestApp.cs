using Avalonia;
using Avalonia.Headless;
using Avalonia.Markup.Xaml.Styling;
using BluerCurve.Tests;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]

namespace BluerCurve.Tests;

public class TestApp : Application
{
    public override void Initialize()
    {
        Styles.Add(new BluerCurveTheme());
        Styles.Add(new StyleInclude(new Uri("avares://BluerCurve.Theme.Tests/"))
        {
            Source = new Uri("avares://BluerCurve.Theme.ColorPicker/BluerCurve.axaml"),
        });
    }
}

public static class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<TestApp>()
        .UseSkia()
        .UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false });
}
