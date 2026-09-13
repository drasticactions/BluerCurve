using Avalonia;
using Avalonia.Controls;

namespace BluerCurve.Chrome;

internal sealed class WindowStateTracker : IDisposable
{
    public const string Active = ":active";
    public const string Inactive = ":inactive";
    public const string Normal = ":normal";
    public const string Maximized = ":maximized";
    public const string Minimized = ":minimized";
    public const string FullScreen = ":fullscreen";

    private readonly Window _window;
    private readonly IPseudoClasses _target;

    public WindowStateTracker(Window window, IPseudoClasses target)
    {
        _window = window;
        _target = target;
        _window.PropertyChanged += OnWindowPropertyChanged;
        Update();
    }

    private void OnWindowPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == WindowBase.IsActiveProperty || e.Property == Window.WindowStateProperty)
            Update();
    }

    private void Update()
    {
        var active = _window.IsActive;
        _target.Set(Active, active);
        _target.Set(Inactive, !active);
        var state = _window.WindowState;
        _target.Set(Normal, state == WindowState.Normal);
        _target.Set(Maximized, state == WindowState.Maximized);
        _target.Set(Minimized, state == WindowState.Minimized);
        _target.Set(FullScreen, state == WindowState.FullScreen);
    }

    public void Dispose() => _window.PropertyChanged -= OnWindowPropertyChanged;
}
