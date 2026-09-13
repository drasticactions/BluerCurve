using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Xunit;

namespace BluerCurve.Tests;

public class ListsSnapshotTests
{
    [AvaloniaFact]
    public void Render_ListBox_And_TreeView()
    {
        var listBox = new ListBox
        {
            ItemsSource = Enumerable.Range(1, 20).Select(i => $"List item {i}").ToArray(),
            SelectedIndex = 2,
            Width = 160,
            Height = 160,
        };

        var tree = new TreeView { Width = 200, Height = 160 };
        var root = new TreeViewItem { Header = "Root", IsExpanded = true };
        var child = new TreeViewItem { Header = "Child 1", IsExpanded = true };
        child.Items.Add(new TreeViewItem { Header = "Leaf 1" });
        child.Items.Add(new TreeViewItem { Header = "Leaf 2" });
        root.Items.Add(child);
        root.Items.Add(new TreeViewItem { Header = "Child 2" });
        var collapsed = new TreeViewItem { Header = "Collapsed" };
        collapsed.Items.Add(new TreeViewItem { Header = "Hidden" });
        root.Items.Add(collapsed);
        tree.Items.Add(root);
        tree.Items.Add(new TreeViewItem { Header = "Second root" });
        tree.SelectedItem = child;

        var disabled = new ListBox
        {
            ItemsSource = new[] { "Disabled 1", "Disabled 2" },
            IsEnabled = false,
            Width = 120,
            Height = 60,
        };

        var panel = new StackPanel { Margin = new Thickness(12), Spacing = 12, Orientation = Orientation.Horizontal };
        panel.Children.Add(listBox);
        panel.Children.Add(tree);
        panel.Children.Add(disabled);

        var window = new Window { Content = panel, Width = 540, Height = 200, SizeToContent = SizeToContent.Manual };
        var path = Snapshot.Capture(window, "lists-listbox-treeview");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_TabControls()
    {
        var grid = new UniformGrid { Columns = 2, Rows = 2, Margin = new Thickness(12) };
        foreach (var placement in new[] { Dock.Top, Dock.Bottom, Dock.Left, Dock.Right })
        {
            var tabs = new TabControl
            {
                TabStripPlacement = placement,
                Margin = new Thickness(6),
            };
            for (var i = 1; i <= 4; i++)
            {
                tabs.Items.Add(new TabItem
                {
                    Header = $"Tab{i}",
                    Content = new TextBlock { Text = $"Page {i} ({placement})", Margin = new Thickness(8) },
                });
            }
            tabs.SelectedIndex = 1;
            grid.Children.Add(tabs);
        }

        var window = new Window { Content = grid, Width = 520, Height = 360, SizeToContent = SizeToContent.Manual };
        var path = Snapshot.Capture(window, "lists-tabcontrol");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_TabStrip_And_ScrollViewer()
    {
        var strip = new TabStrip
        {
            ItemsSource = new[] { "T1", "T2", "T3", "T4" },
            SelectedIndex = 0,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        var big = new Border
        {
            Width = 400,
            Height = 300,
            Background = Avalonia.Media.Brushes.White,
            Child = new TextBlock { Text = "Scrollable content", Margin = new Thickness(8) },
        };
        var scroll = new ScrollViewer
        {
            Content = big,
            Width = 220,
            Height = 140,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible,
            VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
        };
        scroll.Offset = new Vector(40, 60);

        var horizontalBar = new ScrollBar
        {
            Orientation = Orientation.Horizontal,
            Width = 220,
            Maximum = 100,
            ViewportSize = 30,
            Value = 20,
            Visibility = ScrollBarVisibility.Visible,
        };
        var verticalBar = new ScrollBar
        {
            Orientation = Orientation.Vertical,
            Height = 140,
            Maximum = 100,
            ViewportSize = 30,
            Value = 70,
            Visibility = ScrollBarVisibility.Visible,
        };

        var row = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 12 };
        row.Children.Add(scroll);
        row.Children.Add(verticalBar);

        var panel = new StackPanel { Margin = new Thickness(12), Spacing = 12 };
        panel.Children.Add(strip);
        panel.Children.Add(row);
        panel.Children.Add(horizontalBar);

        var window = new Window { Content = panel, Width = 300, Height = 260, SizeToContent = SizeToContent.Manual };
        var path = Snapshot.Capture(window, "lists-scrollviewer");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_TableView()
    {
        var table = new TableView
        {
            Width = 320,
            Height = 180,
            ItemsSource = Enumerable.Range(1, 12).Select(i => new RowModel(i, $"Text {i}.1", i * 10)).ToArray(),
            SelectedIndex = 1,
        };
        table.Columns.Add(new TableViewColumn { Header = "i", Width = new GridLength(30), CellTemplate = Cell(nameof(RowModel.Index)) });
        table.Columns.Add(new TableViewColumn { Header = "Text", Width = new GridLength(1, GridUnitType.Star), CellTemplate = Cell(nameof(RowModel.Text)) });
        table.Columns.Add(new TableViewColumn { Header = "Progress", Width = new GridLength(90), CellTemplate = Cell(nameof(RowModel.Progress)) });

        var window = new Window
        {
            Content = new Border { Padding = new Thickness(12), Child = table },
            Width = 360,
            Height = 220,
            SizeToContent = SizeToContent.Manual,
        };
        var path = Snapshot.Capture(window, "lists-tableview");
        Assert.True(File.Exists(path));
    }

    private static Avalonia.Controls.Templates.FuncDataTemplate<RowModel> Cell(string property) =>
        new((model, _) =>
        {
            var value = model?.GetType().GetProperty(property)?.GetValue(model)?.ToString();
            return new TextBlock { Text = value };
        });

    public sealed record RowModel(int Index, string Text, int Progress);
}
