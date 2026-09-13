using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;

namespace BluerCurve.Chrome;

[PseudoClasses(WindowStateTracker.Active, WindowStateTracker.Inactive, WindowStateTracker.Normal,
    WindowStateTracker.Maximized, WindowStateTracker.Minimized, WindowStateTracker.FullScreen)]
public class WindowChromeHost : Panel
{
    private WindowStateTracker? _tracker;

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (ChromeHelper.FindHostWindow(this) is { } window)
            _tracker = new WindowStateTracker(window, PseudoClasses);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _tracker?.Dispose();
        _tracker = null;
    }
}
