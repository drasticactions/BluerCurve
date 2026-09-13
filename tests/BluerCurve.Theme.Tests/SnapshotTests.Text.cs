using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Xunit;

namespace BluerCurve.Tests;

public class TextSnapshotTests
{
    [AvaloniaFact]
    public void Render_Check_And_Radio()
    {
        var checks = new StackPanel { Spacing = 2 };
        checks.Children.Add(new CheckBox { Content = "Check btn 1" });
        checks.Children.Add(new CheckBox { Content = "Check btn 2", IsChecked = true });
        checks.Children.Add(new CheckBox { Content = "Check btn 3", IsThreeState = true, IsChecked = null });
        checks.Children.Add(new CheckBox { Content = "Check btn 4", IsEnabled = false });
        checks.Children.Add(new CheckBox { Content = "Check btn 5", IsChecked = true, IsEnabled = false });
        checks.Children.Add(new CheckBox { Content = "Check btn 6", IsThreeState = true, IsChecked = null, IsEnabled = false });

        var radios = new StackPanel { Spacing = 2 };
        radios.Children.Add(new RadioButton { Content = "Radio btn 1", GroupName = "a" });
        radios.Children.Add(new RadioButton { Content = "Radio btn 2", GroupName = "a", IsChecked = true });
        radios.Children.Add(new RadioButton { Content = "Radio btn 3", GroupName = "b", IsThreeState = true, IsChecked = null });
        radios.Children.Add(new RadioButton { Content = "Radio btn 4", GroupName = "c", IsEnabled = false });
        radios.Children.Add(new RadioButton { Content = "Radio btn 5", GroupName = "c", IsChecked = true, IsEnabled = false });
        radios.Children.Add(new RadioButton { Content = "Radio btn 6", GroupName = "d", IsThreeState = true, IsChecked = null, IsEnabled = false });

        var focused = new CheckBox { Content = "Focused check", Margin = new Thickness(0, 8, 0, 0) };
        var row = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
        row.Children.Add(checks);
        row.Children.Add(radios);
        var panel = new StackPanel { Margin = new Thickness(12), Spacing = 4 };
        panel.Children.Add(row);
        panel.Children.Add(focused);

        var window = new Window { Content = panel, Width = 260, Height = 200, SizeToContent = SizeToContent.Manual };
        window.Show();
        focused.Focus(Avalonia.Input.NavigationMethod.Tab);
        var path = Snapshot.Capture(window, "text-check-radio");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_Entries()
    {
        var panel = new StackPanel { Margin = new Thickness(12), Spacing = 6, Width = 200 };
        var focusedEntry = new TextBox { Text = "Entry (focused)" };
        panel.Children.Add(focusedEntry);
        panel.Children.Add(new TextBox { Text = "Entry" });
        panel.Children.Add(new TextBox { PlaceholderText = "Entry" });
        panel.Children.Add(new TextBox { Text = "Entry", IsEnabled = false });
        panel.Children.Add(new TextBox { PlaceholderText = "Entry", IsEnabled = false });
        panel.Children.Add(new TextBox { Text = "Selected text", SelectionStart = 0, SelectionEnd = 8 });
        panel.Children.Add(new TextBox { Text = "Multi\nline\nentry", AcceptsReturn = true, Height = 60 });
        panel.Children.Add(new AutoCompleteBox { Text = "AutoComplete", ItemsSource = new[] { "One", "Two" } });
        panel.Children.Add(new AutoCompleteBox { PlaceholderText = "AutoComplete", ItemsSource = new[] { "One", "Two" }, IsEnabled = false });
        panel.Children.Add(new SelectableTextBlock { Text = "Selectable text block", SelectionStart = 0, SelectionEnd = 10 });

        var window = new Window { Content = panel, Width = 224, Height = 360, SizeToContent = SizeToContent.Manual };
        window.Show();
        focusedEntry.Focus();
        var path = Snapshot.Capture(window, "text-entries");
        Assert.True(File.Exists(path));
    }

    [AvaloniaFact]
    public void Render_ComboBoxes()
    {
        var items = new[] { "Combo box 1", "Two", "Three" };
        var panel = new StackPanel { Margin = new Thickness(12), Spacing = 6, Width = 200 };
        panel.Children.Add(new ComboBox { ItemsSource = items, SelectedIndex = 0, HorizontalAlignment = HorizontalAlignment.Stretch });
        panel.Children.Add(new ComboBox { ItemsSource = items, PlaceholderText = "Combo box", HorizontalAlignment = HorizontalAlignment.Stretch });
        panel.Children.Add(new ComboBox { ItemsSource = items, SelectedIndex = 0, IsEnabled = false, HorizontalAlignment = HorizontalAlignment.Stretch });
        panel.Children.Add(new ComboBox { ItemsSource = items, IsEditable = true, Text = "Combo box entry 1", HorizontalAlignment = HorizontalAlignment.Stretch });
        panel.Children.Add(new ComboBox { ItemsSource = items, IsEditable = true, Text = "Combo box entry 1", IsEnabled = false, HorizontalAlignment = HorizontalAlignment.Stretch });
        var focused = new ComboBox { ItemsSource = items, SelectedIndex = 1, HorizontalAlignment = HorizontalAlignment.Stretch };
        panel.Children.Add(focused);

        var popupBox = new BluerCurve.Primitives.BevelBorder
        {
            Background = Brush("BcBgBrush"),
            BorderBrush = Brush("BcBorderBrush"),
            Padding = new Thickness(4, 3),
            Child = new StackPanel
            {
                Children =
                {
                    new ComboBoxItem { Content = "Combo box 1" },
                    new ComboBoxItem { Content = "Two", IsSelected = true },
                    new ComboBoxItem { Content = "Three", IsEnabled = false },
                },
            },
        };
        panel.Children.Add(popupBox);

        var window = new Window { Content = panel, Width = 224, Height = 320, SizeToContent = SizeToContent.Manual };
        window.Show();
        focused.Focus(Avalonia.Input.NavigationMethod.Tab);
        var path = Snapshot.Capture(window, "text-combobox");
        Assert.True(File.Exists(path));
    }

    private static Avalonia.Media.IBrush? Brush(string key)
        => Application.Current?.TryGetResource(key, null, out var b) == true ? b as Avalonia.Media.IBrush : null;
}
