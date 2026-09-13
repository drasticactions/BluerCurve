using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Xunit;

namespace BluerCurve.Tests;

public class ThemeLoadTests
{
    [AvaloniaFact]
    public void Theme_Resolves_Palette_Brushes()
    {
        var app = Application.Current!;
        Assert.True(app.TryGetResource("BcBgBrush", null, out var bg));
        Assert.Equal(Color.Parse("#e6e6e6"), ((ISolidColorBrush)bg!).Color);
        Assert.True(app.TryGetResource("BcSelectedBgBrush", null, out _));
        Assert.True(app.TryGetResource("BcTitleActiveGradientBrush", null, out var title));
        Assert.IsAssignableFrom<ILinearGradientBrush>(title);
    }

    [AvaloniaFact]
    public void Variant_Switch_Updates_Resources()
    {
        var theme = Application.Current!.Styles.OfType<BluerCurveTheme>().Single();
        try
        {
            theme.Variant = BluerCurveVariant.Grape;
            Assert.True(Application.Current!.TryGetResource("BcSelectedBgColor", null, out var c));
            Assert.Equal(Color.Parse("#493973"), (Color)c!);
        }
        finally
        {
            theme.Variant = BluerCurveVariant.Bluecurve;
        }
    }

    [AvaloniaFact]
    public void Shade_Matches_Gtk_Ramp()
    {
        var bg = Color.Parse("#e6e6e6");
        Assert.Equal(Color.Parse("#f5f5f5"), ColorMath.Shade(bg, 1.065));
        Assert.Equal(Color.Parse("#999999"), ColorMath.Shade(bg, 0.665));
        Assert.Equal(Color.Parse("#5c5c5c"), ColorMath.Shade(bg, 0.4));
        Assert.Equal(Color.Parse("#2f2f2f"), ColorMath.Shade(bg, 0.205));
        var sel = Color.Parse("#4464ac");
        Assert.Equal(Color.Parse("#3b4c71"), ColorMath.Shade(sel, 0.72));
    }

    [AvaloniaFact]
    public void Window_Uses_Luxi_Sans()
    {
        var window = new Window { Content = new TextBlock { Text = "x" } };
        window.Show();
        Assert.Contains("Luxi Sans", window.FontFamily.ToString());
        Assert.Equal(13, window.FontSize);
        window.Close();
    }
}
