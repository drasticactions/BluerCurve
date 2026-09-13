using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Xunit;

namespace BluerCurve.Tests;

public class ButtonsSnapshotTests
{
    private static StackPanel Column(double width = 200) =>
        new() { Spacing = 8, Width = width, HorizontalAlignment = HorizontalAlignment.Left };

    private static T Hovered<T>(T control) where T : Control
    {
        ((IPseudoClasses)control.Classes).Set(":pointerover", true);
        return control;
    }

    [AvaloniaFact]
    public void Render_Buttons()
    {
        var left = Column();
        left.Children.Add(new RepeatButton { Content = "Repeat" });
        left.Children.Add(Hovered(new RepeatButton { Content = "Repeat (hover)" }));
        left.Children.Add(new RepeatButton { Content = "Repeat (disabled)", IsEnabled = false });
        left.Children.Add(new ToggleButton { Content = "Toggle" });
        left.Children.Add(Hovered(new ToggleButton { Content = "Toggle (hover)" }));
        left.Children.Add(new ToggleButton { Content = "Toggle (checked)", IsChecked = true });
        left.Children.Add(new ToggleButton { Content = "Toggle (disabled)", IsEnabled = false });
        left.Children.Add(new ToggleButton { Content = "Toggle (checked, disabled)", IsChecked = true, IsEnabled = false });
        left.Children.Add(new DropDownButton { Content = "Drop down", HorizontalAlignment = HorizontalAlignment.Stretch });
        left.Children.Add(Hovered(new DropDownButton { Content = "Drop down (hover)", HorizontalAlignment = HorizontalAlignment.Stretch }));
        left.Children.Add(new DropDownButton { Content = "Drop down (disabled)", IsEnabled = false, HorizontalAlignment = HorizontalAlignment.Stretch });
        left.Children.Add(new SplitButton { Content = "Split button", HorizontalAlignment = HorizontalAlignment.Stretch });
        left.Children.Add(new SplitButton { Content = "Split (disabled)", IsEnabled = false, HorizontalAlignment = HorizontalAlignment.Stretch });
        left.Children.Add(new HyperlinkButton { Content = "Link button" });
        left.Children.Add(Hovered(new HyperlinkButton { Content = "Link button (hover)" }));
        left.Children.Add(new HyperlinkButton { Content = "Link button (disabled)", IsEnabled = false });

        var right = Column(220);
        right.Children.Add(new ToggleSwitch { OffContent = "Off", OnContent = "On" });
        right.Children.Add(Hovered(new ToggleSwitch { OffContent = "Off (hover)", OnContent = "On" }));
        right.Children.Add(new ToggleSwitch { OffContent = "Off", OnContent = "On", IsChecked = true });
        right.Children.Add(new ToggleSwitch { OffContent = "Off (disabled)", OnContent = "On", IsEnabled = false });
        right.Children.Add(new ToggleSwitch { OffContent = "Off", OnContent = "On (disabled)", IsChecked = true, IsEnabled = false });
        right.Children.Add(new NumericUpDown { Value = 1, Minimum = 0, Maximum = 10 });
        right.Children.Add(new NumericUpDown { Value = 1, Minimum = 0, Maximum = 10, IsEnabled = false });
        right.Children.Add(new ButtonSpinner { Content = new TextBlock { Text = "Spinner content", Margin = new Thickness(3) } });
        right.Children.Add(new ButtonSpinner { Content = new TextBlock { Text = "Spinner (disabled)", Margin = new Thickness(3) }, IsEnabled = false });
        right.Children.Add(new PathIcon { Data = Avalonia.Media.Geometry.Parse("M0,0 L16,0 L16,16 L0,16 Z M4,4 L12,4 L12,12 L4,12 Z"), HorizontalAlignment = HorizontalAlignment.Left });

        var row = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 16, Margin = new Thickness(12) };
        row.Children.Add(left);
        row.Children.Add(right);

        var window = new Window { Content = row, Width = 480, Height = 600, SizeToContent = SizeToContent.Manual };
        var path = Snapshot.Capture(window, "buttons-buttons");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_Containers()
    {
        var panel = new StackPanel { Margin = new Thickness(12), Spacing = 10, Width = 420 };

        panel.Children.Add(new Label { Content = "Label 1" });
        panel.Children.Add(new Label { Content = "Label 2 (disabled)", IsEnabled = false });
        panel.Children.Add(new Separator());

        var frames = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
        frames.Children.Add(new GroupBox { Header = "Frame (group box)", Padding = new Thickness(6), Width = 200, Content = new TextBlock { Text = "Content" } });
        frames.Children.Add(new HeaderedContentControl { Header = "Headered", Width = 200, Content = new TextBlock { Text = "Content" } });
        panel.Children.Add(frames);

        var vsep = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Height = 24 };
        vsep.Children.Add(new TextBlock { Text = "Left", VerticalAlignment = VerticalAlignment.Center });
        vsep.Children.Add(new Separator { Classes = { "vertical" } });
        vsep.Children.Add(new TextBlock { Text = "Right", VerticalAlignment = VerticalAlignment.Center });
        panel.Children.Add(vsep);

        panel.Children.Add(new Expander { Header = "More... (collapsed)", Content = new TextBlock { Text = "Hidden" } });
        panel.Children.Add(Hovered(new Expander { Header = "More... (hover)", Content = new TextBlock { Text = "Hidden" } }));
        panel.Children.Add(new Expander { Header = "More... (expanded)", IsExpanded = true, Content = new TextBlock { Text = "Expanded content", Margin = new Thickness(20, 2, 0, 2) } });
        panel.Children.Add(new Expander { Header = "Up (expanded)", ExpandDirection = ExpandDirection.Up, IsExpanded = true, Content = new TextBlock { Text = "Expanded content", Margin = new Thickness(20, 2, 0, 2) } });
        panel.Children.Add(new Expander { Header = "More... (disabled)", IsEnabled = false, Content = new TextBlock { Text = "Hidden" } });

        var grid = new Grid { Height = 60, ColumnDefinitions = new ColumnDefinitions("*,Auto,*") };
        var a = new Border { Background = Avalonia.Media.Brushes.White, Child = new TextBlock { Text = "Pane A", Margin = new Thickness(4) } };
        var splitter = new GridSplitter { ResizeDirection = GridResizeDirection.Columns };
        Grid.SetColumn(splitter, 1);
        var b = new Border { Background = Avalonia.Media.Brushes.White, Child = new TextBlock { Text = "Pane B", Margin = new Thickness(4) } };
        Grid.SetColumn(b, 2);
        grid.Children.Add(a);
        grid.Children.Add(splitter);
        grid.Children.Add(b);
        panel.Children.Add(grid);

        panel.Children.Add(new SplitView
        {
            Height = 70,
            DisplayMode = SplitViewDisplayMode.Inline,
            IsPaneOpen = true,
            OpenPaneLength = 120,
            Pane = new TextBlock { Text = "Pane", Margin = new Thickness(6) },
            Content = new Border { Background = Avalonia.Media.Brushes.White, Child = new TextBlock { Text = "Content", Margin = new Thickness(6) } },
        });

        var window = new Window { Content = panel, Width = 460, Height = 600, SizeToContent = SizeToContent.Manual };
        window.Show();
        Avalonia.Threading.Dispatcher.UIThread.RunJobs();
        var hoveredExpander = panel.Children.OfType<Expander>().Skip(1).First();
        Hovered(Avalonia.VisualTree.VisualExtensions.GetVisualDescendants(hoveredExpander).OfType<ToggleButton>().First());
        var path = Snapshot.Capture(window, "buttons-containers");
        Assert.True(File.Exists(path));
    }
}
