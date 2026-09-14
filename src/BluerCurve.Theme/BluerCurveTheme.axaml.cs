using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using Avalonia.Styling;

[assembly: XmlnsDefinition("https://github.com/drasticactions/BluerCurve", "BluerCurve")]
[assembly: XmlnsDefinition("https://github.com/drasticactions/BluerCurve", "BluerCurve.Chrome")]
[assembly: XmlnsDefinition("https://github.com/drasticactions/BluerCurve", "BluerCurve.Primitives")]

namespace BluerCurve;

public partial class BluerCurveTheme : Styles
{
    public static readonly StyledProperty<BluerCurveVariant> VariantProperty =
        AvaloniaProperty.Register<BluerCurveTheme, BluerCurveVariant>(nameof(Variant));

    public static readonly StyledProperty<BluerCurvePalette?> PaletteProperty =
        AvaloniaProperty.Register<BluerCurveTheme, BluerCurvePalette?>(nameof(Palette));

    public BluerCurveTheme(IServiceProvider? sp = null)
    {
        TextSmoothing.EnsureInitialized();
        AvaloniaXamlLoader.Load(sp, this);
        ApplyPalette();
    }

    public BluerCurveVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public BluerCurvePalette? Palette
    {
        get => GetValue(PaletteProperty);
        set => SetValue(PaletteProperty, value);
    }

    public BluerCurvePalette EffectivePalette => Palette ?? BluerCurvePalette.FromVariant(Variant);

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == VariantProperty || change.Property == PaletteProperty)
        {
            ApplyPalette();
        }
    }

    private void ApplyPalette() => BluerCurveResources.Apply(EffectivePalette, Resources);
}
