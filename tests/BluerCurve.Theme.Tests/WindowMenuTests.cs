using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.VisualTree;
using BluerCurve.Chrome;
using Xunit;

namespace BluerCurve.Tests;

public class WindowMenuTests
{
    [AvaloniaFact]
    public void Window_Menu_Closes_On_Click_Elsewhere()
    {
        var window = new BluerCurveWindow { Width = 400, Height = 300, Content = new Button { Content = "x", HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center } };
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var menuButton = window.GetVisualDescendants().OfType<WindowMenuButton>().Single();
        var center = menuButton.TranslatePoint(new Point(menuButton.Bounds.Width / 2, menuButton.Bounds.Height / 2), window)!.Value;
        window.MouseDown(center, MouseButton.Left);
        window.MouseUp(center, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.True(menuButton.Flyout is { IsOpen: true }, "flyout did not open");
        var menuFlyout = Assert.IsType<MenuFlyout>(menuButton.Flyout);
        Assert.True(menuFlyout.Items.Count >= 4, $"flyout has {menuFlyout.Items.Count} items");
        var popupRoot = Avalonia.VisualTree.VisualExtensions.GetVisualDescendants(window).Concat(
            Avalonia.Application.Current!.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime l ? l.Windows.SelectMany(x => Avalonia.VisualTree.VisualExtensions.GetVisualDescendants(x)) : []);
        var presenter = Avalonia.VisualTree.VisualExtensions.GetVisualDescendants(window).OfType<MenuFlyoutPresenter>().FirstOrDefault()
            ?? (menuFlyout.GetType().GetProperty("Popup", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(menuFlyout) as Popup)?.Child as MenuFlyoutPresenter;
        Assert.NotNull(presenter);
        Dispatcher.UIThread.RunJobs();
        var realized = presenter!.GetRealizedContainers().Count();
        Assert.True(realized >= 4, $"presenter realized {realized} containers, bounds {presenter.Bounds}");

        var elsewhere = new Point(200, 200);
        window.MouseDown(elsewhere, MouseButton.Left);
        window.MouseUp(elsewhere, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.False(menuButton.Flyout is { IsOpen: true }, "flyout stayed open after clicking elsewhere");
        window.Close();
    }
}
