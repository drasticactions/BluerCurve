using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using BluerCurve.Demo;
using Xunit;

namespace BluerCurve.Tests;

public class DemoPagesSnapshotTests
{
    [AvaloniaFact]
    public void Render_Misc_Page()
    {
        var window = new Window { Width = 760, Height = 480, Content = new MiscPage() };
        Assert.True(File.Exists(Snapshot.Capture(window, "demo-misc")));
    }

    [AvaloniaFact]
    public void Render_Lists_Page()
    {
        var window = new Window { Width = 760, Height = 480, Content = new ListsPage() };
        Assert.True(File.Exists(Snapshot.Capture(window, "demo-lists")));
    }
}
