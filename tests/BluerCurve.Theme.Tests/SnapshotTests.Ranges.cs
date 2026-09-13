using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Avalonia.Threading;
using Xunit;

namespace BluerCurve.Tests;

public class RangesSnapshotTests
{
    [AvaloniaFact]
    public void Render_Sliders_And_ProgressBars()
    {
        var left = new StackPanel { Spacing = 10, Width = 220 };
        left.Children.Add(new Slider { Value = 40 });
        left.Children.Add(new Slider { Value = 70, TickPlacement = TickPlacement.BottomRight, TickFrequency = 10 });
        left.Children.Add(new Slider { Value = 25, IsEnabled = false });
        left.Children.Add(new ProgressBar { Value = 40 });
        left.Children.Add(new ProgressBar { Value = 75, ShowProgressText = true });
        left.Children.Add(new ProgressBar { IsIndeterminate = true });
        left.Children.Add(new ProgressBar { Value = 30, IsEnabled = false });
        left.Children.Add(new PipsPager { NumberOfPages = 5, SelectedPageIndex = 2, HorizontalAlignment = HorizontalAlignment.Left });

        var right = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 12, Height = 200 };
        right.Children.Add(new Slider { Value = 40, Orientation = Orientation.Vertical });
        right.Children.Add(new Slider { Value = 60, Orientation = Orientation.Vertical, TickPlacement = TickPlacement.TopLeft, TickFrequency = 10 });
        right.Children.Add(new ProgressBar { Value = 40, Orientation = Orientation.Vertical });
        right.Children.Add(new ProgressBar { IsIndeterminate = true, Orientation = Orientation.Vertical });
        right.Children.Add(new PipsPager { NumberOfPages = 4, SelectedPageIndex = 1, Orientation = Orientation.Vertical });

        var root = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 20, Margin = new Thickness(12) };
        root.Children.Add(left);
        root.Children.Add(right);
        var window = new Window { Content = root, Width = 440, Height = 300, SizeToContent = SizeToContent.Manual };
        var path = Snapshot.Capture(window, "ranges-sliders-progress");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_Calendar_And_Pickers()
    {
        var panel = new StackPanel { Margin = new Thickness(12), Spacing = 10 };
        var today = DateTime.Today;
        panel.Children.Add(new Calendar
        {
            DisplayDate = new DateTime(2004, 5, 1),
            SelectedDate = new DateTime(2004, 5, 12),
            HorizontalAlignment = HorizontalAlignment.Left,
        });
        panel.Children.Add(new CalendarDatePicker { SelectedDate = new DateTime(2004, 5, 12), Width = 200, HorizontalAlignment = HorizontalAlignment.Left });
        panel.Children.Add(new DatePicker { SelectedDate = new DateTimeOffset(new DateTime(2004, 5, 12)) });
        panel.Children.Add(new DatePicker());
        panel.Children.Add(new TimePicker { SelectedTime = new TimeSpan(9, 41, 0) });
        panel.Children.Add(new TimePicker { IsEnabled = false });
        var window = new Window { Content = panel, Width = 300, Height = 470, SizeToContent = SizeToContent.Manual };
        var path = Snapshot.Capture(window, "ranges-calendar-pickers");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_Picker_Flyouts()
    {
        var panel = new StackPanel { Margin = new Thickness(12), Spacing = 12, Orientation = Orientation.Horizontal };
        var datePresenter = new DatePickerPresenter { Date = new DateTimeOffset(new DateTime(2004, 5, 12)) };
        var timePresenter = new TimePickerPresenter { Time = new TimeSpan(9, 41, 0) };
        panel.Children.Add(datePresenter);
        panel.Children.Add(timePresenter);
        var window = new Window { Content = panel, Width = 480, Height = 300, SizeToContent = SizeToContent.Manual };
        var path = Snapshot.Capture(window, "ranges-picker-flyouts");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_Calendar_Year_View()
    {
        var calendar = new Calendar
        {
            DisplayDate = new DateTime(2004, 5, 1),
            SelectedDate = new DateTime(2004, 5, 12),
            DisplayMode = CalendarMode.Year,
            HorizontalAlignment = HorizontalAlignment.Left,
        };
        var window = new Window { Content = new Border { Margin = new Thickness(12), Child = calendar }, Width = 260, Height = 220, SizeToContent = SizeToContent.Manual };
        var path = Snapshot.Capture(window, "ranges-calendar-year");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_File_Chooser()
    {
        var panel = new StackPanel { Margin = new Thickness(12), Spacing = 12 };
        panel.Children.Add(new Avalonia.Dialogs.ManagedFileChooser { Width = 520, Height = 260 });
        panel.Children.Add(new Avalonia.Dialogs.ManagedFileChooserOverwritePrompt { FileName = "notes.txt", HorizontalAlignment = HorizontalAlignment.Left });
        var window = new Window { Content = panel, Width = 560, Height = 400, SizeToContent = SizeToContent.Manual };
        var path = Snapshot.Capture(window, "ranges-file-chooser");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_Pages()
    {
        var nav = new NavigationPage { PageTransition = null, Width = 220, Height = 160 };
        var root = new ContentPage { Header = "Root", Content = new TextBlock { Text = "Root page", Margin = new Thickness(8) } };
        var pushed = new ContentPage { Header = "Details", Content = new TextBlock { Text = "Pushed page", Margin = new Thickness(8) } };
        _ = nav.PushAsync(root, null);
        Dispatcher.UIThread.RunJobs();
        _ = nav.PushAsync(pushed, null);
        Dispatcher.UIThread.RunJobs();

        var tabbed = new TabbedPage { Width = 220, Height = 160 };
        tabbed.Pages = new[]
        {
            new ContentPage { Header = "Tab1", Content = new TextBlock { Text = "First", Margin = new Thickness(8) } },
            new ContentPage { Header = "Tab2", Content = new TextBlock { Text = "Second", Margin = new Thickness(8) } },
            new ContentPage { Header = "Tab3", Content = new TextBlock { Text = "Third", Margin = new Thickness(8) } },
        };

        var drawer = new DrawerPage
        {
            Header = "Drawer",
            Width = 220,
            Height = 160,
            DrawerLength = 90,
            IsOpen = true,
            Drawer = new TextBlock { Text = "Menu", Margin = new Thickness(8) },
            Content = new TextBlock { Text = "Drawer content", Margin = new Thickness(8) },
        };

        var panel = new StackPanel { Margin = new Thickness(12), Spacing = 12, Orientation = Orientation.Horizontal };
        panel.Children.Add(nav);
        panel.Children.Add(tabbed);
        panel.Children.Add(drawer);
        var window = new Window { Content = panel, Width = 720, Height = 200, SizeToContent = SizeToContent.Manual };
        var path = Snapshot.Capture(window, "ranges-pages");
        Assert.True(File.Exists(path));
    }
}
