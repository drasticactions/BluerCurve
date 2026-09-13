using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Xunit;

namespace BluerCurve.Tests;

public class GallerySnapshotTests
{
    [AvaloniaFact]
    public void Render_Basic_Controls()
    {
        var panel = new StackPanel { Margin = new Thickness(12), Spacing = 8, Width = 260 };
        panel.Children.Add(new Button { Content = "Button 1" });
        panel.Children.Add(new Button { Content = "Button 2", IsEnabled = false });
        panel.Children.Add(new ToggleButton { Content = "Button 3", IsChecked = true });
        panel.Children.Add(new Button { Content = "Default", IsDefault = true });
        panel.Children.Add(new CheckBox { Content = "Check btn 1" });
        panel.Children.Add(new CheckBox { Content = "Check btn 2", IsChecked = true });
        panel.Children.Add(new CheckBox { Content = "Check btn 3", IsChecked = null, IsThreeState = true });
        panel.Children.Add(new RadioButton { Content = "Radio btn 1" });
        panel.Children.Add(new RadioButton { Content = "Radio btn 2", IsChecked = true });
        panel.Children.Add(new TextBox { Text = "Entry" });
        panel.Children.Add(new TextBox { PlaceholderText = "Entry", IsEnabled = false });
        panel.Children.Add(new ComboBox { ItemsSource = new[] { "Combo box 1", "Two" }, SelectedIndex = 0, HorizontalAlignment = HorizontalAlignment.Stretch });
        panel.Children.Add(new ProgressBar { Value = 40 });
        panel.Children.Add(new Slider { Value = 40 });
        var window = new Window { Content = panel, Width = 300, Height = 520, SizeToContent = SizeToContent.Manual };
        var path = Snapshot.Capture(window, "basic-controls");
        Assert.True(File.Exists(path));
    }
}
