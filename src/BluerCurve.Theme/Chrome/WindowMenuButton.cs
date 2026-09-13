using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace BluerCurve.Chrome;

public class WindowMenuButton : CaptionButton
{
    private readonly MenuFlyout _flyout;
    private readonly MenuItem _minimize = new() { Header = "Mi_nimize" };
    private readonly MenuItem _maximize = new() { Header = "Ma_ximize" };
    private readonly MenuItem _close = new() { Header = "_Close" };
    private bool _built;
    private InputElement? _dismissRoot;

    protected override Type StyleKeyOverride => typeof(WindowMenuButton);

    public WindowMenuButton()
    {
        _flyout = new MenuFlyout { Placement = PlacementMode.BottomEdgeAlignedLeft };
        _flyout.Opening += (_, _) => UpdateState();
        _flyout.Opened += OnFlyoutOpened;
        _flyout.Closed += OnFlyoutClosed;
        _minimize.Click += (_, _) => { if (Host is { } w) w.WindowState = WindowState.Minimized; };
        _maximize.Click += (_, _) => { if (Host is { } w) w.WindowState = w.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized; };
        _close.Click += (_, _) => Host?.Close();
        Flyout = _flyout;
    }

    private Window? Host => ChromeHelper.FindHostWindow(this);

    protected override void OnClick()
    {
        EnsureBuilt();
        base.OnClick();
    }

    private void EnsureBuilt()
    {
        if (_built)
            return;
        _built = true;
        if (Host is { } window && WindowMenu.GetItems(window) is { Count: > 0 } custom)
        {
            foreach (var item in custom)
                _flyout.Items.Add(item);
            _flyout.Items.Add(new Separator());
        }
        _flyout.Items.Add(_minimize);
        _flyout.Items.Add(_maximize);
        _flyout.Items.Add(new Separator());
        _flyout.Items.Add(_close);
    }

    private void UpdateState()
    {
        if (Host is not { } window)
            return;
        _minimize.IsEnabled = window.CanMinimize;
        _maximize.IsEnabled = window.CanMaximize && window.CanResize;
        _maximize.Header = window.WindowState == WindowState.Maximized ? "Unma_ximize" : "Ma_ximize";
    }

    private void OnFlyoutOpened(object? sender, EventArgs e)
    {
        _dismissRoot = VisualRoot as InputElement;
        _dismissRoot?.AddHandler(PointerPressedEvent, OnRootPointerPressed, RoutingStrategies.Tunnel, handledEventsToo: true);
    }

    private void OnFlyoutClosed(object? sender, EventArgs e)
    {
        _dismissRoot?.RemoveHandler(PointerPressedEvent, OnRootPointerPressed);
        _dismissRoot = null;
    }

    private void OnRootPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source is Visual v && (v == this || v.GetVisualAncestors().Contains(this)))
            return;
        _flyout.Hide();
    }

}
