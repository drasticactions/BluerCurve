using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace BluerCurve;

public static class TextSmoothing
{
    public static readonly AttachedProperty<bool> SmoothTextProperty =
        AvaloniaProperty.RegisterAttached<Visual, bool>("SmoothText", typeof(TextSmoothing), defaultValue: true);

    static TextSmoothing()
    {
        SmoothTextProperty.Changed.AddClassHandler<Visual>((visual, e) => Apply(visual, e.GetNewValue<bool>()));
        TemplatedControl.TemplateAppliedEvent.AddClassHandler<TopLevel>((topLevel, _) => Apply(topLevel, GetSmoothText(topLevel)));
    }

    internal static void EnsureInitialized()
    {
    }

    public static bool GetSmoothText(Visual visual) => visual.GetValue(SmoothTextProperty);

    public static void SetSmoothText(Visual visual, bool value) => visual.SetValue(SmoothTextProperty, value);

    private static void Apply(Visual visual, bool smooth)
    {
        TextOptions.SetTextRenderingMode(visual, smooth ? TextRenderingMode.Antialias : TextRenderingMode.Unspecified);
        TextOptions.SetTextHintingMode(visual, smooth ? TextHintingMode.None : TextHintingMode.Unspecified);
    }
}
