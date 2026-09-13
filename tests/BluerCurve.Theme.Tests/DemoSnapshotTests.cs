using Avalonia;
using Avalonia.Headless.XUnit;
using BluerCurve.Demo;
using Xunit;

namespace BluerCurve.Tests;

public class DemoSnapshotTests
{
    [AvaloniaTheory]
    [InlineData(BluerCurveVariant.Bluecurve)]
    [InlineData(BluerCurveVariant.Grape)]
    [InlineData(BluerCurveVariant.Tangerine)]
    public void Render_Demo_MainWindow(BluerCurveVariant variant)
    {
        var theme = Application.Current!.Styles.OfType<BluerCurveTheme>().Single();
        try
        {
            theme.Variant = variant;
            var window = new MainWindow();
            var path = Snapshot.Capture(window, $"demo-{variant}".ToLowerInvariant());
            Assert.True(File.Exists(path));
        }
        finally
        {
            theme.Variant = BluerCurveVariant.Bluecurve;
        }
    }
}
