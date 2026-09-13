using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace BluerCurve.Chrome;

[PseudoClasses(WindowStateTracker.Active, WindowStateTracker.Inactive, WindowStateTracker.Maximized)]
public class TitleBarPod : ContentControl
{
    private WindowStateTracker? _tracker;

    protected override Type StyleKeyOverride => typeof(TitleBarPod);

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
