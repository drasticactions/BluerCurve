using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Headless.XUnit;
using Avalonia.LogicalTree;
using Avalonia.Styling;
using Avalonia.Threading;
using Xunit;

namespace BluerCurve.Tests;

public class DrawnDecorationsTests
{
    [AvaloniaFact]
    public void Underlay_Paints_Nothing_When_No_Part_Is_Enabled()
    {
        Assert.True(Application.Current!.TryGetResource(typeof(WindowDrawnDecorations), null, out var theme));
        var window = new Window { Width = 300, Height = 200 };
        window.Show();
        var decorations = new WindowDrawnDecorations { Theme = (ControlTheme)theme! };
        ((ISetLogicalParent)decorations).SetParent(window);
        decorations.ApplyStyling();
        var applyTemplate = typeof(WindowDrawnDecorations).GetMethod("ApplyTemplate", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(applyTemplate);
        applyTemplate.Invoke(decorations, null);
        Dispatcher.UIThread.RunJobs();

        var borders = decorations.GetLogicalDescendants().OfType<Border>().ToList();
        var frame = Assert.Single(borders, b => b.Name == "PART_WindowBorder");
        var outline = Assert.Single(borders, b => b.Name == "PART_ClientOutline");
        Assert.False(frame.IsVisible);
        Assert.False(outline.IsVisible);
        window.Close();
    }
}
