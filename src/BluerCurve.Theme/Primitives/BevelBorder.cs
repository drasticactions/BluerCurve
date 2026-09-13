using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace BluerCurve.Primitives;

public class BevelBorder : Decorator
{
    public static readonly StyledProperty<IBrush?> BackgroundProperty =
        Border.BackgroundProperty.AddOwner<BevelBorder>();

    public static readonly StyledProperty<IBrush?> BorderBrushProperty =
        Border.BorderBrushProperty.AddOwner<BevelBorder>();

    public static readonly StyledProperty<Thickness> BorderThicknessProperty =
        Border.BorderThicknessProperty.AddOwner<BevelBorder>(new StyledPropertyMetadata<Thickness>(new Thickness(1)));

    public static readonly StyledProperty<BevelStyle> BevelProperty =
        AvaloniaProperty.Register<BevelBorder, BevelStyle>(nameof(Bevel), BevelStyle.Raised);

    public static readonly StyledProperty<IBrush?> LightBrushProperty =
        AvaloniaProperty.Register<BevelBorder, IBrush?>(nameof(LightBrush), Brushes.White);

    public static readonly StyledProperty<IBrush?> DarkBrushProperty =
        AvaloniaProperty.Register<BevelBorder, IBrush?>(nameof(DarkBrush), new ImmutableSolidColorBrush(Color.Parse("#cecece")));

    static BevelBorder()
    {
        AffectsRender<BevelBorder>(BackgroundProperty, BorderBrushProperty, BorderThicknessProperty, BevelProperty, LightBrushProperty, DarkBrushProperty);
        AffectsMeasure<BevelBorder>(BorderThicknessProperty, BevelProperty);
    }

    public IBrush? Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public IBrush? BorderBrush
    {
        get => GetValue(BorderBrushProperty);
        set => SetValue(BorderBrushProperty, value);
    }

    public Thickness BorderThickness
    {
        get => GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    public BevelStyle Bevel
    {
        get => GetValue(BevelProperty);
        set => SetValue(BevelProperty, value);
    }

    public IBrush? LightBrush
    {
        get => GetValue(LightBrushProperty);
        set => SetValue(LightBrushProperty, value);
    }

    public IBrush? DarkBrush
    {
        get => GetValue(DarkBrushProperty);
        set => SetValue(DarkBrushProperty, value);
    }

    private Thickness Inset
    {
        get
        {
            var b = BorderThickness;
            var bevel = Bevel == BevelStyle.None ? 0 : 1;
            return new Thickness(b.Left + bevel, b.Top + bevel, b.Right + bevel, b.Bottom + bevel);
        }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var inset = Inset + Padding;
        var child = Child;
        if (child is null)
            return new Size(inset.Left + inset.Right, inset.Top + inset.Bottom);
        child.Measure(availableSize.Deflate(inset));
        return child.DesiredSize.Inflate(inset);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        Child?.Arrange(new Rect(finalSize).Deflate(Inset + Padding));
        return finalSize;
    }

    public override void Render(DrawingContext context)
    {
        var rect = new Rect(Bounds.Size);
        if (rect.Width <= 0 || rect.Height <= 0)
            return;

        if (Background is { } background)
            context.FillRectangle(background, rect);

        var b = BorderThickness;
        if (BorderBrush is { } border)
        {
            if (b.Top > 0) context.FillRectangle(border, new Rect(0, 0, rect.Width, b.Top));
            if (b.Bottom > 0) context.FillRectangle(border, new Rect(0, rect.Height - b.Bottom, rect.Width, b.Bottom));
            if (b.Left > 0) context.FillRectangle(border, new Rect(0, b.Top, b.Left, rect.Height - b.Top - b.Bottom));
            if (b.Right > 0) context.FillRectangle(border, new Rect(rect.Width - b.Right, b.Top, b.Right, rect.Height - b.Top - b.Bottom));
        }

        if (Bevel == BevelStyle.None)
            return;

        var inner = rect.Deflate(b);
        if (inner.Width < 2 || inner.Height < 2)
            return;

        var topLeft = Bevel == BevelStyle.Raised ? LightBrush : DarkBrush;
        var bottomRight = Bevel == BevelStyle.Raised ? DarkBrush : LightBrush;

        if (topLeft is not null)
        {
            context.FillRectangle(topLeft, new Rect(inner.X, inner.Y, inner.Width, 1));
            context.FillRectangle(topLeft, new Rect(inner.X, inner.Y, 1, inner.Height));
        }
        if (bottomRight is not null)
        {
            context.FillRectangle(bottomRight, new Rect(inner.X, inner.Bottom - 1, inner.Width, 1));
            context.FillRectangle(bottomRight, new Rect(inner.Right - 1, inner.Y, 1, inner.Height));
        }
    }
}
