using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace BluerCurve.Chrome;

[PseudoClasses(WindowStateTracker.Active, WindowStateTracker.Inactive, WindowStateTracker.Maximized)]
public class TitleBarBackground : Control
{
    public static readonly StyledProperty<IBrush?> ActiveBrushProperty =
        AvaloniaProperty.Register<TitleBarBackground, IBrush?>(nameof(ActiveBrush));

    public static readonly StyledProperty<IBrush?> InactiveBrushProperty =
        AvaloniaProperty.Register<TitleBarBackground, IBrush?>(nameof(InactiveBrush));

    public static readonly StyledProperty<Color> StripeColorProperty =
        AvaloniaProperty.Register<TitleBarBackground, Color>(nameof(StripeColor), Color.FromArgb(91, 2, 2, 2));

    public static readonly StyledProperty<Color> StripeSoftColorProperty =
        AvaloniaProperty.Register<TitleBarBackground, Color>(nameof(StripeSoftColor), Color.FromArgb(74, 242, 242, 242));

    public static readonly StyledProperty<IBrush?> StripeCoverBrushProperty =
        AvaloniaProperty.Register<TitleBarBackground, IBrush?>(nameof(StripeCoverBrush));

    public static readonly StyledProperty<IBrush?> ShineBrushProperty =
        AvaloniaProperty.Register<TitleBarBackground, IBrush?>(nameof(ShineBrush));

    public static readonly StyledProperty<IBrush?> BottomLineBrushProperty =
        AvaloniaProperty.Register<TitleBarBackground, IBrush?>(nameof(BottomLineBrush));

    public static readonly StyledProperty<IBrush?> EdgeLineBrushProperty =
        AvaloniaProperty.Register<TitleBarBackground, IBrush?>(nameof(EdgeLineBrush));

    public static readonly StyledProperty<IBrush?> InactiveHighlightBrushProperty =
        AvaloniaProperty.Register<TitleBarBackground, IBrush?>(nameof(InactiveHighlightBrush));

    public static readonly StyledProperty<IBrush?> InactiveBottomLineBrushProperty =
        AvaloniaProperty.Register<TitleBarBackground, IBrush?>(nameof(InactiveBottomLineBrush));

    public static readonly StyledProperty<bool> IsActiveProperty =
        AvaloniaProperty.Register<TitleBarBackground, bool>(nameof(IsActive), true);

    public static readonly StyledProperty<bool> FollowWindowActivationProperty =
        AvaloniaProperty.Register<TitleBarBackground, bool>(nameof(FollowWindowActivation), true);

    public static readonly StyledProperty<bool> ShowStripesProperty =
        AvaloniaProperty.Register<TitleBarBackground, bool>(nameof(ShowStripes), true);

    private WindowStateTracker? _tracker;

    static TitleBarBackground()
    {
        AffectsRender<TitleBarBackground>(ActiveBrushProperty, InactiveBrushProperty, StripeColorProperty, StripeSoftColorProperty,
            StripeCoverBrushProperty, ShineBrushProperty, BottomLineBrushProperty, EdgeLineBrushProperty,
            InactiveHighlightBrushProperty, InactiveBottomLineBrushProperty, IsActiveProperty, ShowStripesProperty);
    }

    public IBrush? ActiveBrush { get => GetValue(ActiveBrushProperty); set => SetValue(ActiveBrushProperty, value); }
    public IBrush? InactiveBrush { get => GetValue(InactiveBrushProperty); set => SetValue(InactiveBrushProperty, value); }
    public Color StripeColor { get => GetValue(StripeColorProperty); set => SetValue(StripeColorProperty, value); }
    public Color StripeSoftColor { get => GetValue(StripeSoftColorProperty); set => SetValue(StripeSoftColorProperty, value); }
    public IBrush? StripeCoverBrush { get => GetValue(StripeCoverBrushProperty); set => SetValue(StripeCoverBrushProperty, value); }
    public IBrush? ShineBrush { get => GetValue(ShineBrushProperty); set => SetValue(ShineBrushProperty, value); }
    public IBrush? BottomLineBrush { get => GetValue(BottomLineBrushProperty); set => SetValue(BottomLineBrushProperty, value); }
    public IBrush? EdgeLineBrush { get => GetValue(EdgeLineBrushProperty); set => SetValue(EdgeLineBrushProperty, value); }
    public IBrush? InactiveHighlightBrush { get => GetValue(InactiveHighlightBrushProperty); set => SetValue(InactiveHighlightBrushProperty, value); }
    public IBrush? InactiveBottomLineBrush { get => GetValue(InactiveBottomLineBrushProperty); set => SetValue(InactiveBottomLineBrushProperty, value); }

    public bool IsActive { get => GetValue(IsActiveProperty); set => SetValue(IsActiveProperty, value); }

    public bool ShowStripes { get => GetValue(ShowStripesProperty); set => SetValue(ShowStripesProperty, value); }

    public bool FollowWindowActivation { get => GetValue(FollowWindowActivationProperty); set => SetValue(FollowWindowActivationProperty, value); }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (ChromeHelper.FindHostWindow(this) is { } window)
        {
            _tracker = new WindowStateTracker(window, PseudoClasses);
            if (FollowWindowActivation)
                this[!IsActiveProperty] = window[!WindowBase.IsActiveProperty];
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _tracker?.Dispose();
        _tracker = null;
    }

    public override void Render(DrawingContext context)
    {
        var w = Bounds.Width;
        var h = Bounds.Height;
        if (w <= 0 || h <= 0)
            return;

        var rect = new Rect(0, 0, w, h);

        if (!IsActive)
        {
            if (InactiveBrush is { } inactive)
                context.FillRectangle(inactive, rect);
            if (InactiveHighlightBrush is { } hl)
                context.FillRectangle(hl, new Rect(0, 0, w, 1));
            if (InactiveBottomLineBrush is { } ibl)
                context.FillRectangle(ibl, new Rect(0, h - 1, w, 1));
            return;
        }

        if (ActiveBrush is { } active)
            context.FillRectangle(active, new Rect(0, 0, w, h - 1));

        if (ShowStripes && h >= 4)
        {
            var stripeRect = new Rect(0, 1, w, h - 2);
            var dark = new ImmutablePen(new ImmutableSolidColorBrush(StripeColor), 1);
            var soft = new ImmutablePen(new ImmutableSolidColorBrush(StripeSoftColor), 1);
            using (context.PushClip(stripeRect))
            {
                for (var x = 0.5; x < w + stripeRect.Height + 5; x += 5)
                {
                    var p1 = new Point(x, stripeRect.Top);
                    var p2 = new Point(x - stripeRect.Height, stripeRect.Bottom);
                    context.DrawLine(dark, p1, p2);
                    context.DrawLine(soft, p1 + new Vector(1, 0), p2 + new Vector(1, 0));
                }
            }
            if (StripeCoverBrush is { } cover)
            {
                context.FillRectangle(cover, new Rect(0, 1, w, 1));
                context.FillRectangle(cover, new Rect(0, h - 2, w, 1));
            }
        }

        if (ShineBrush is { } shine)
            context.FillRectangle(shine, new Rect(0, 0, w, 1));
        if (BottomLineBrush is { } bottom)
            context.FillRectangle(bottom, new Rect(0, h - 1, w, 1));
        if (EdgeLineBrush is { } edge)
        {
            context.FillRectangle(edge, new Rect(0, 0, 1, h));
            context.FillRectangle(edge, new Rect(w - 1, 0, 1, h));
        }
    }
}
