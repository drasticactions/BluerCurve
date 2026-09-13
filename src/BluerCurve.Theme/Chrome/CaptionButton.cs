using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace BluerCurve.Chrome;

[PseudoClasses(WindowStateTracker.Active, WindowStateTracker.Inactive, WindowStateTracker.Normal,
    WindowStateTracker.Maximized, WindowStateTracker.Minimized, WindowStateTracker.FullScreen)]
public class CaptionButton : Button
{
    public static readonly StyledProperty<Geometry?> GlyphProperty =
        AvaloniaProperty.Register<CaptionButton, Geometry?>(nameof(Glyph));

    public static readonly StyledProperty<Geometry?> StrokeGlyphProperty =
        AvaloniaProperty.Register<CaptionButton, Geometry?>(nameof(StrokeGlyph));

    public static readonly StyledProperty<CaptionButtonAction> ActionProperty =
        AvaloniaProperty.Register<CaptionButton, CaptionButtonAction>(nameof(Action));

    private WindowStateTracker? _tracker;

    protected override Type StyleKeyOverride => typeof(CaptionButton);

    public Geometry? Glyph
    {
        get => GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    public Geometry? StrokeGlyph
    {
        get => GetValue(StrokeGlyphProperty);
        set => SetValue(StrokeGlyphProperty, value);
    }

    public CaptionButtonAction Action
    {
        get => GetValue(ActionProperty);
        set => SetValue(ActionProperty, value);
    }

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

    protected override void OnClick()
    {
        base.OnClick();
        if (Action == CaptionButtonAction.None || ChromeHelper.FindHostWindow(this) is not { } window)
            return;

        switch (Action)
        {
            case CaptionButtonAction.Close:
                window.Close();
                break;
            case CaptionButtonAction.Minimize:
                window.WindowState = WindowState.Minimized;
                break;
            case CaptionButtonAction.Maximize:
                window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                break;
            case CaptionButtonAction.FullScreen:
                window.WindowState = window.WindowState == WindowState.FullScreen ? WindowState.Normal : WindowState.FullScreen;
                break;
        }
    }
}
