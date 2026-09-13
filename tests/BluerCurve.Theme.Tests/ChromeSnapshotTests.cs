using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using BluerCurve.Chrome;
using Xunit;

namespace BluerCurve.Tests;

public class ChromeSnapshotTests
{
    private static Control SampleContent() => new StackPanel
    {
        Margin = new Thickness(8), Spacing = 6,
        Children =
        {
            new TextBlock { Text = "The quick brown fox jumps over the lazy dog!" },
            new Button { Content = "Button 1", HorizontalAlignment = HorizontalAlignment.Left },
            new TextBox { Text = "Entry" },
        },
    };

    [AvaloniaFact]
    public void Render_BluerCurveWindow_Frame()
    {
        var window = new BluerCurveWindow { Title = "*Unsaved Document 1 - Pluma", Width = 420, Height = 200, Content = SampleContent() };
        var path = Snapshot.Capture(window, "chrome-window-active");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_BluerCurveWindow_Maximized()
    {
        var window = new BluerCurveWindow { Title = "galculator", Width = 420, Height = 160, Content = SampleContent(), WindowState = WindowState.Maximized };
        var path = Snapshot.Capture(window, "chrome-window-maximized");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_TitleBar_Pieces_At_Scale()
    {
        var panel = new StackPanel { Spacing = 4, Margin = new Thickness(4) };
        panel.Children.Add(new TitleBarBackground { Width = 260, Height = 22, IsActive = true, FollowWindowActivation = false });
        panel.Children.Add(new TitleBarBackground { Width = 260, Height = 22, IsActive = false, FollowWindowActivation = false });
        var buttons = new StackPanel { Orientation = Orientation.Horizontal, Background = Brushes.SteelBlue, Height = 22 };
        buttons.Children.Add(new WindowMenuButton());
        buttons.Children.Add(new CaptionButton { Glyph = (Geometry)Application.Current!.FindResource("BcCaptionMinimizeGeometry")! });
        buttons.Children.Add(new CaptionButton { Glyph = (Geometry)Application.Current!.FindResource("BcCaptionMaximizeGeometry")! });
        buttons.Children.Add(new CaptionButton { Glyph = (Geometry)Application.Current!.FindResource("BcCaptionRestoreGeometry")! });
        buttons.Children.Add(new CaptionButton { StrokeGlyph = (Geometry)Application.Current!.FindResource("BcCaptionCloseStrokeGeometry")! });
        panel.Children.Add(buttons);
        var pods = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto"), Height = 21 };
        var leftPod = new TitleBarPod { Classes = { "left" }, Content = new WindowMenuButton() };
        var bg = new TitleBarBackground { IsActive = true, FollowWindowActivation = false };
        Grid.SetColumn(bg, 1);
        var rightPod = new TitleBarPod { Classes = { "right" }, Content = new CaptionButton { StrokeGlyph = (Geometry)Application.Current!.FindResource("BcCaptionCloseStrokeGeometry")! } };
        Grid.SetColumn(rightPod, 2);
        pods.Children.Add(leftPod); pods.Children.Add(bg); pods.Children.Add(rightPod);
        panel.Children.Add(pods);
        var zoom = new LayoutTransformControl { LayoutTransform = new ScaleTransform(3, 3), Child = panel };
        var window = new Window { Content = zoom, SizeToContent = SizeToContent.WidthAndHeight };
        var path = Snapshot.Capture(window, "chrome-pieces-3x");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void DrawnDecorations_Theme_Resolves()
    {
        Assert.True(Application.Current!.TryGetResource(typeof(WindowDrawnDecorations), null, out var theme));
        var controlTheme = Assert.IsType<ControlTheme>(theme);
        Assert.Contains(controlTheme.Setters, s => s is Setter { Property.Name: "Template" });
        Assert.True(Application.Current!.TryGetResource("BluerCurveWindow", null, out var named));
        Assert.IsType<ControlTheme>(named);
    }
}
