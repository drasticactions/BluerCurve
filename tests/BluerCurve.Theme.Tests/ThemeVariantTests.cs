using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Avalonia.Styling;
using Xunit;

namespace BluerCurve.Tests;

public class ThemeVariantTests
{
    private static BluerCurveTheme Theme => Application.Current!.Styles.OfType<BluerCurveTheme>().Single();

    private static Color ColorOf(string key, ThemeVariant variant)
    {
        Assert.True(Application.Current!.TryGetResource(key, variant, out var value), $"{key} missing in {variant}");
        return (Color)value!;
    }

    [AvaloniaFact]
    public void Dark_Dictionary_Resolves_Palette()
    {
        Assert.Equal(BluerCurvePalette.BluecurveDark.Bg, ColorOf("BcBgColor", ThemeVariant.Dark));
        Assert.Equal(BluerCurvePalette.Bluecurve.Bg, ColorOf("BcBgColor", ThemeVariant.Light));
        Assert.Equal(BluerCurvePalette.Bluecurve.Bg, ColorOf("BcBgColor", ThemeVariant.Default));
    }

    [AvaloniaFact]
    public void Every_Key_Exists_In_Both_Variants()
    {
        var dictionary = (ResourceDictionary)Theme.Resources;
        var light = (ResourceDictionary)dictionary.ThemeDictionaries[ThemeVariant.Default];
        var dark = (ResourceDictionary)dictionary.ThemeDictionaries[ThemeVariant.Dark];
        Assert.NotEmpty(light.Keys);
        Assert.Equal(light.Keys.OrderBy(k => k.ToString()), dark.Keys.OrderBy(k => k.ToString()));
    }

    [AvaloniaFact]
    public void Light_Bevel_Highlight_Stays_White()
    {
        Assert.Equal(Colors.White, ColorOf("BcBevelLightColor", ThemeVariant.Light));
        Assert.NotEqual(Colors.White, ColorOf("BcBevelLightColor", ThemeVariant.Dark));
    }

    [AvaloniaFact]
    public void Window_Follows_Requested_Variant()
    {
        var window = new Window { Content = new TextBlock { Text = "x" } };
        window.Show();
        try
        {
            Assert.Equal(BluerCurvePalette.Bluecurve.Bg, ((ISolidColorBrush)window.Background!).Color);
            window.RequestedThemeVariant = ThemeVariant.Dark;
            Assert.Equal(BluerCurvePalette.BluecurveDark.Bg, ((ISolidColorBrush)window.Background!).Color);
            Assert.Equal(BluerCurvePalette.BluecurveDark.Fg, ((ISolidColorBrush)window.Foreground!).Color);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void Variant_Switch_Updates_Both_Dictionaries()
    {
        try
        {
            Theme.Variant = BluerCurveVariant.Grape;
            Assert.Equal(BluerCurvePalette.Grape.SelectedBg, ColorOf("BcSelectedBgColor", ThemeVariant.Light));
            Assert.Equal(BluerCurvePalette.GrapeDark.SelectedBg, ColorOf("BcSelectedBgColor", ThemeVariant.Dark));
        }
        finally
        {
            Theme.Variant = BluerCurveVariant.Bluecurve;
        }
    }

    [AvaloniaFact]
    public void Custom_Palettes_Are_Independent()
    {
        try
        {
            Theme.DarkPalette = BluerCurvePalette.BluecurveDark with { Bg = Color.Parse("#101010") };
            Assert.Equal(Color.Parse("#101010"), ColorOf("BcBgColor", ThemeVariant.Dark));
            Assert.Equal(BluerCurvePalette.Bluecurve.Bg, ColorOf("BcBgColor", ThemeVariant.Light));

            Theme.Palette = BluerCurvePalette.Bluecurve with { Bg = Color.Parse("#fafafa") };
            Assert.Equal(Color.Parse("#fafafa"), ColorOf("BcBgColor", ThemeVariant.Light));
            Assert.Equal(Color.Parse("#101010"), ColorOf("BcBgColor", ThemeVariant.Dark));
        }
        finally
        {
            Theme.Palette = null;
            Theme.DarkPalette = null;
        }
    }

    [AvaloniaFact]
    public void FromVariant_Honours_Theme_Variant()
    {
        Assert.Same(BluerCurvePalette.LimeDark, BluerCurvePalette.FromVariant(BluerCurveVariant.Lime, ThemeVariant.Dark));
        Assert.Same(BluerCurvePalette.Lime, BluerCurvePalette.FromVariant(BluerCurveVariant.Lime, ThemeVariant.Light));
    }
}
