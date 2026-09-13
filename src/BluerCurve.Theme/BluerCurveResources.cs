using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using static BluerCurve.ColorMath;

namespace BluerCurve;

public static class BluerCurveResources
{
    public static void Apply(BluerCurvePalette p, IResourceDictionary target)
    {
        var bg = p.Bg;
        var sel = p.SelectedBg;
        var light = Light(bg);
        var dark = Dark(bg);
        var lightSel = Light(sel);
        var darkSel = Dark(sel);
        var white = Colors.White;
        var black = Colors.Black;

        void Set(string name, Color c)
        {
            target[$"Bc{name}Color"] = c;
            target[$"Bc{name}Brush"] = new ImmutableSolidColorBrush(c);
        }
        void Brush(string name, IBrush b) => target[$"Bc{name}Brush"] = b;

        Set("Bg", bg);
        Set("Fg", p.Fg);
        Set("Text", p.Text);
        Set("Base", p.Base);
        Set("Prelight", p.Prelight);
        Set("BgActive", p.BgActive);
        Set("SelectedBg", sel);
        Set("UnfocusedSelectedBg", p.UnfocusedSelectedBg);
        Set("SelectedFg", p.SelectedFg);
        Set("InsensitiveBg", p.InsensitiveBg);
        Set("InsensitiveFg", p.InsensitiveFg);
        Set("InsensitiveBase", p.InsensitiveBase);
        Set("TextActive", p.TextActive);
        Set("Warning", p.Warning);
        Set("Error", p.Error);
        Set("Success", p.Success);
        Set("Link", p.Link);
        Set("LinkVisited", p.LinkVisited);
        Set("TooltipBg", p.TooltipBg);
        Set("TooltipFg", p.TooltipFg);
        Set("TooltipBorder", p.TooltipBorder);

        Set("LightBorder", Shade(bg, 1.065));
        Set("EntryShadow", Shade(bg, 0.963));
        Set("SeparatorDarker", Shade(bg, 0.896));
        Set("TroughBg", Shade(bg, 0.85));
        Set("TroughShadow", Shade(bg, 0.768));
        Set("MediumBorder", Shade(bg, 0.7));
        Set("Border", Shade(bg, 0.665));
        Set("DarkerBorder", Shade(bg, 0.4));
        Set("Outline", Shade(bg, 0.205));
        Set("ButtonIcon", Shade(bg, 0.205));
        Set("Darker", p.BgActive);
        Set("White", white);
        Set("Black", black);
        Set("BevelLight", white);
        Set("BevelDark", Shade(bg, 0.896));
        Set("EntryDisabledText", Color.Parse("#757575"));
        Set("DisabledTextShadow", p.SelectedFg);
        Set("Light", light);
        Set("Dark", dark);
        Set("LightSelected", lightSel);
        Set("DarkSelected", darkSel);

        Set("MenuItemTop", Shade(sel, 0.9));
        Set("MenuItemBottom", Shade(sel, 1.2));
        Set("MenuItemLight", Shade(sel, 1.62));
        Set("MenuItemDark", Shade(sel, 1.05));
        Set("MenuItemBorder", Shade(sel, 0.72));
        Set("ProgressTop", Shade(sel, 0.92));
        Set("ProgressBottom", Shade(sel, 1.66));
        Set("ProgressLight", Shade(sel, 1.62));
        Set("ProgressDark", Shade(sel, 0.72));
        Set("ProgressBorder", Shade(sel, 1.05));
        Set("Check", sel);
        Set("SelectionTranslucent", WithAlpha(sel, 0.25));
        Set("SelectedBgHover", Shade(sel, 1.1));

        Brush("MenuItemGradient", Vertical((0, Shade(sel, 0.9)), (0.04, Shade(sel, 0.9)), (1, Shade(sel, 1.2))));
        Brush("MenuItemGradientHorizontal", Horizontal((0, Shade(sel, 0.9)), (0.04, Shade(sel, 0.9)), (1, Shade(sel, 1.2))));
        Brush("ProgressGradient", Vertical((0, Shade(sel, 0.92)), (0.1, Shade(sel, 0.92)), (1, Shade(sel, 1.66))));
        Brush("ProgressGradientHorizontal", Horizontal((0, Shade(sel, 0.92)), (0.1, Shade(sel, 0.92)), (1, Shade(sel, 1.66))));

        Brush("TitleActiveGradient", Vertical(
            (0, Blend(light, sel, 0.8)), (0.25, Blend(light, sel, 0.95)), (0.5, sel), (0.75, Blend(light, sel, 0.95)), (1, Blend(light, sel, 0.8))));
        Set("TitleActiveMid", sel);
        Set("TitleActiveEdge", Blend(light, sel, 0.8));
        Set("TitleStripeCover", Blend(light, sel, 0.8));
        Set("TitleShine", Blend(light, lightSel, 0.75));
        Set("TitleBottomLine", Shade(sel, 0.1));
        Set("TitleEdgeLine", Blend(sel, black, 0.4));
        Set("TitleText", p.SelectedFg);
        Set("TitleTextShadow", Blend(sel, black, 0.4));
        Set("TitleTextInactive", p.InsensitiveFg);
        Set("TitleStripe", WithAlpha(Color.FromRgb(2, 2, 2), 0.357));
        Set("TitleStripeSoft", WithAlpha(Color.FromRgb(242, 242, 242), 0.29));
        Brush("TitlePodGradient", Vertical((0, Blend(sel, bg, 0.75)), (0.8, Blend(sel, bg, 0.75)), (1, Blend(lightSel, light, 0.75))));
        Set("TitlePodSide", Blend(sel, black, 0.4));
        Set("TitlePodBottom", Blend(dark, sel, 0.5));
        Brush("TitlePodInactiveGradient", Vertical((0, Blend(light, bg, 0.7)), (1, Blend(light, bg, 0.9))));
        Set("TitlePodInactiveSide", Blend(dark, bg, 0.4));
        Brush("TitleInactiveGradient", Vertical(
            (0, Blend(light, bg, 0.95)), (0.17, Blend(bg, dark, 0.2)), (0.83, Blend(bg, dark, 0.2)), (1, Blend(bg, dark, 0.3))));
        Set("TitleInactiveHighlight", Shade(p.InsensitiveBg, 1.2));
        Set("TitleInactiveBottomLine", Blend(dark, bg, 0.5));
        Set("FrameActiveTint", Blend(sel, bg, 0.75));
        Set("FrameActiveTintLight", Blend(lightSel, light, 0.75));
        Set("FrameActiveSeparator", Blend(dark, sel, 0.5));
        Set("FrameInactiveTint", Blend(bg, dark, 0.2));
        Set("FrameInactiveSeparator", Blend(dark, bg, 0.5));
        Set("FrameOutline", black);
        Brush("FrameActiveGradient", Vertical((0, Blend(sel, bg, 0.75)), (0.8, Blend(sel, bg, 0.75)), (1, Blend(lightSel, light, 0.75))));
        Brush("CaptionButtonGradient", Diagonal(Blend(light, lightSel, 0.2), Blend(light, lightSel, 0.1)));
        Set("CaptionButtonFill", Blend(light, lightSel, 0.15));
        Set("CaptionButtonInnerLight", Blend(light, lightSel, 0.05));
        Set("CaptionButtonInnerDark", Blend(bg, sel, 0.15));
        Set("CaptionButtonBorder", Blend(dark, sel, 0.3));
        Set("CaptionButtonBorderHover", Blend(dark, sel, 0.7));
        Set("CaptionButtonPrelightTint", WithAlpha(light, 0.5));
        Set("CaptionButtonPressedTint", WithAlpha(Blend(darkSel, dark, 0.75), 0.25));
        Set("CaptionGlyph", WithAlpha(darkSel, 0.75));
        Set("CaptionGlyphHover", darkSel);
        Set("CaptionGlyphInactive", WithAlpha(p.InsensitiveFg, 0.75));
        Brush("CaptionButtonInactiveGradient", Vertical((0, Blend(light, bg, 0.7)), (1, Blend(light, bg, 0.9))));
        Set("CaptionButtonInactiveBorder", Blend(dark, bg, 0.4));

    }

    private static ImmutableLinearGradientBrush Vertical(params (double Offset, Color Color)[] stops) =>
        new(stops.Select(s => new ImmutableGradientStop(s.Offset, s.Color)).ToList(),
            startPoint: new RelativePoint(0, 0, RelativeUnit.Relative),
            endPoint: new RelativePoint(0, 1, RelativeUnit.Relative));

    private static ImmutableLinearGradientBrush Horizontal(params (double Offset, Color Color)[] stops) =>
        new(stops.Select(s => new ImmutableGradientStop(s.Offset, s.Color)).ToList(),
            startPoint: new RelativePoint(0, 0, RelativeUnit.Relative),
            endPoint: new RelativePoint(1, 0, RelativeUnit.Relative));

    private static ImmutableLinearGradientBrush Diagonal(Color from, Color to) =>
        new(new List<ImmutableGradientStop> { new(0, from), new(1, to) },
            startPoint: new RelativePoint(0, 0, RelativeUnit.Relative),
            endPoint: new RelativePoint(1, 1, RelativeUnit.Relative));
}
