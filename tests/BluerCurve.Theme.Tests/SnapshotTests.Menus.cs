using Avalonia.Media.Imaging;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Xunit;

namespace BluerCurve.Tests;

public class MenusSnapshotTests
{
    private static MenuItem Item(string header, string? gesture = null, bool enabled = true) => new()
    {
        Header = header,
        InputGesture = gesture is null ? null : KeyGesture.Parse(gesture),
        IsEnabled = enabled,
    };

    private static Menu BuildMenuBar(out MenuItem file)
    {
        file = new MenuItem { Header = "_File" };
        file.Items.Add(Item("_New", "Ctrl+N"));
        file.Items.Add(Item("_Open...", "Ctrl+O"));
        var recent = new MenuItem { Header = "Open _Recent" };
        recent.Items.Add(Item("notes.txt"));
        recent.Items.Add(Item("todo.txt"));
        file.Items.Add(recent);
        file.Items.Add(new Separator());
        file.Items.Add(Item("_Save", "Ctrl+S"));
        file.Items.Add(Item("Save _As...", "Shift+Ctrl+S", enabled: false));
        file.Items.Add(new Separator());
        file.Items.Add(new MenuItem { Header = "Show _Toolbar", ToggleType = MenuItemToggleType.CheckBox, IsChecked = true });
        file.Items.Add(new MenuItem { Header = "Word _Wrap", ToggleType = MenuItemToggleType.Radio, IsChecked = true });
        file.Items.Add(new Separator());
        file.Items.Add(Item("_Quit", "Ctrl+Q"));

        var edit = new MenuItem { Header = "_Edit" };
        edit.Items.Add(Item("_Undo", "Ctrl+Z"));
        edit.Items.Add(Item("_Redo", "Shift+Ctrl+Z"));
        var view = new MenuItem { Header = "_View" };
        view.Items.Add(Item("_Zoom In", "Ctrl+OemPlus"));
        var help = new MenuItem { Header = "_Help" };
        help.Items.Add(Item("_About"));

        var menu = new Menu();
        menu.Items.Add(file);
        menu.Items.Add(edit);
        menu.Items.Add(view);
        menu.Items.Add(help);
        return menu;
    }

    [AvaloniaFact]
    public void Render_Menu_Bar_With_Open_Menu()
    {
        var menu = BuildMenuBar(out var file);
        var dock = new DockPanel();
        DockPanel.SetDock(menu, Dock.Top);
        dock.Children.Add(menu);
        dock.Children.Add(new TextBlock { Text = "Document area", Margin = new Thickness(12) });
        var window = new Window { Content = dock, Width = 320, Height = 320, SizeToContent = SizeToContent.Manual };

        window.Show();
        Dispatcher.UIThread.RunJobs();
        file.IsSelected = true;
        file.IsSubMenuOpen = true;
        Dispatcher.UIThread.RunJobs();
        AvaloniaHeadlessPlatform.ForceRenderTimerTick();
        Dispatcher.UIThread.RunJobs();

        var popup = file.GetVisualDescendants().OfType<Popup>().First(p => p.Name == "PART_Popup");
        Assert.True(popup.IsOpen);
        if (TopLevel.GetTopLevel(popup.Child) is PopupRoot root)
        {
            using var frame = root.CaptureRenderedFrame() ?? throw new InvalidOperationException("No popup frame rendered");
            Directory.CreateDirectory(Snapshot.Directory);
            frame.Save(Path.Combine(Snapshot.Directory, "menus-menubar-popup.png"), PngBitmapEncoderOptions.Default);
        }

        var path = Snapshot.Capture(window, "menus-menubar");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_Context_Menu_Inline()
    {
        var cm = new ContextMenu { HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };
        cm.Items.Add(Item("_Undo", "Ctrl+Z"));
        var highlighted = Item("_Redo", "Shift+Ctrl+Z");
        highlighted.IsSelected = true;
        cm.Items.Add(highlighted);
        cm.Items.Add(new Separator());
        cm.Items.Add(Item("Cu_t", "Ctrl+X"));
        cm.Items.Add(Item("_Copy", "Ctrl+C"));
        cm.Items.Add(Item("_Paste", "Ctrl+V", enabled: false));
        cm.Items.Add(new Separator());
        cm.Items.Add(new MenuItem { Header = "Show _Line Numbers", ToggleType = MenuItemToggleType.CheckBox, IsChecked = true });
        cm.Items.Add(new MenuItem { Header = "_Plain Text", ToggleType = MenuItemToggleType.Radio, IsChecked = true });
        cm.Items.Add(new MenuItem { Header = "_Markdown", ToggleType = MenuItemToggleType.Radio });
        cm.Items.Add(new Separator());
        var more = new MenuItem { Header = "_Input Methods" };
        more.Items.Add(Item("System"));
        cm.Items.Add(more);
        var highlightedSub = new MenuItem { Header = "_Insert Unicode Control Character", IsSelected = true };
        highlightedSub.Items.Add(Item("LRM"));
        cm.Items.Add(highlightedSub);

        var window = new Window
        {
            Content = new Border { Padding = new Thickness(12), Child = cm },
            Width = 420,
            Height = 330,
            SizeToContent = SizeToContent.Manual,
        };
        var path = Snapshot.Capture(window, "menus-contextmenu");
        Assert.True(File.Exists(path));
    }

    private static PathIcon Icon(string data) => new() { Data = StreamGeometry.Parse(data), Width = 16, Height = 16 };

    private const string PlusIcon = "M7,2H9V7H14V9H9V14H7V9H2V7H7Z";
    private const string MinusIcon = "M2,7H14V9H2Z";
    private const string BoxIcon = "M2,2H14V14H2Z M4,4V12H12V4Z";
    private const string DiskIcon = "M2,2H12L14,4V14H2Z M4,4V7H10V4Z M5,9V12H11V9Z";

    [AvaloniaFact]
    public void Render_CommandBar()
    {
        var bar = new CommandBar { Width = 400 };
        bar.PrimaryCommands.Add(new CommandBarButton { Label = "New", Icon = Icon(PlusIcon) });
        bar.PrimaryCommands.Add(new CommandBarButton { Label = "Open", Icon = Icon(BoxIcon) });
        bar.PrimaryCommands.Add(new CommandBarSeparator());
        bar.PrimaryCommands.Add(new CommandBarButton { Label = "Save", Icon = Icon(DiskIcon) });
        bar.PrimaryCommands.Add(new CommandBarButton { Label = "Remove", Icon = Icon(MinusIcon), IsEnabled = false });
        bar.PrimaryCommands.Add(new CommandBarSeparator());
        bar.PrimaryCommands.Add(new CommandBarToggleButton { Label = "Bold", Icon = Icon(BoxIcon), IsChecked = true });
        bar.PrimaryCommands.Add(new CommandBarToggleButton { Label = "Italic", Icon = Icon(BoxIcon) });

        var compact = new CommandBar { Width = 400, DefaultLabelPosition = CommandBarDefaultLabelPosition.Collapsed };
        compact.PrimaryCommands.Add(new CommandBarButton { Label = "New", Icon = Icon(PlusIcon) });
        compact.PrimaryCommands.Add(new CommandBarButton { Label = "Open", Icon = Icon(BoxIcon) });
        compact.PrimaryCommands.Add(new CommandBarSeparator());
        compact.PrimaryCommands.Add(new CommandBarToggleButton { Label = "Bold", Icon = Icon(BoxIcon), IsChecked = true });
        compact.PrimaryCommands.Add(new CommandBarButton { Label = "Remove", Icon = Icon(MinusIcon), IsEnabled = false });

        var right = new CommandBar { Width = 400, DefaultLabelPosition = CommandBarDefaultLabelPosition.Right };
        right.PrimaryCommands.Add(new CommandBarButton { Label = "Open", Icon = Icon(BoxIcon) });
        right.PrimaryCommands.Add(new CommandBarButton { Label = "Save", Icon = Icon(DiskIcon) });
        right.PrimaryCommands.Add(new CommandBarSeparator());
        right.PrimaryCommands.Add(new CommandBarToggleButton { Label = "Undo", Icon = Icon(BoxIcon), IsChecked = true });

        var panel = new StackPanel { Spacing = 12, Margin = new Thickness(12) };
        panel.Children.Add(bar);
        panel.Children.Add(compact);
        panel.Children.Add(right);
        var window = new Window { Content = panel, Width = 424, Height = 200, SizeToContent = SizeToContent.Manual };
        var path = Snapshot.Capture(window, "menus-commandbar");
        Assert.True(File.Exists(path));
    }

    private static NotificationCard Card(NotificationType type, string title, string message)
    {
        var body = new StackPanel { Margin = new Thickness(8), Spacing = 4 };
        body.Children.Add(new TextBlock { Text = title, FontWeight = FontWeight.Bold });
        body.Children.Add(new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap });
        return new NotificationCard { NotificationType = type, Content = body };
    }

    [AvaloniaFact]
    public void Render_Notification_Cards()
    {
        var panel = new StackPanel { Spacing = 4, Margin = new Thickness(12) };
        panel.Children.Add(Card(NotificationType.Information, "Information", "The document was reloaded from disk."));
        panel.Children.Add(Card(NotificationType.Success, "Success", "All files were saved."));
        panel.Children.Add(Card(NotificationType.Warning, "Warning", "The file has been modified by another program."));
        panel.Children.Add(Card(NotificationType.Error, "Error", "Could not write to /etc/fstab: permission denied."));
        var window = new Window { Content = panel, Width = 380, Height = 300, SizeToContent = SizeToContent.Manual };
        var path = Snapshot.Capture(window, "menus-notifications");
        Assert.True(File.Exists(path));
    }
}
