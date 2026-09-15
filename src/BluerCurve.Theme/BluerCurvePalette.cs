using Avalonia.Media;
using Avalonia.Styling;

namespace BluerCurve;

public sealed record BluerCurvePalette(
    Color Bg,
    Color Prelight,
    Color BgActive,
    Color SelectedBg,
    Color UnfocusedSelectedBg,
    Color InsensitiveBg,
    Color InsensitiveFg,
    Color InsensitiveBase,
    Color TextActive)
{
    public Color Fg { get; init; } = Colors.Black;
    public Color Text { get; init; } = Colors.Black;
    public Color Base { get; init; } = Colors.White;
    public Color SelectedFg { get; init; } = Colors.White;
    public Color Warning { get; init; } = Color.Parse("#f57900");
    public Color Error { get; init; } = Color.Parse("#cc0000");
    public Color Success { get; init; } = Color.Parse("#73d216");
    public Color Link { get; init; } = Color.Parse("#0000ee");
    public Color LinkVisited { get; init; } = Color.Parse("#551a8b");
    public Color TooltipBg { get; init; } = Color.Parse("#ffffbf");
    public Color TooltipFg { get; init; } = Colors.Black;
    public Color TooltipBorder { get; init; } = Colors.Black;

    public Color BevelLight { get; init; } = Colors.White;

    public Color DisabledTextShadow { get; init; } = Colors.White;

    public Color EntryDisabledText { get; init; } = Color.Parse("#757575");

    public Color FrameOutline { get; init; } = Colors.Black;

    public Color DefaultButtonBorder { get; init; } = Colors.Black;

    public Color ButtonIconHover { get; init; } = Colors.Black;

    public Color? CaptionGlyph { get; init; }

    public Color? ButtonIcon { get; init; }

    public Color? Outline { get; init; }

    public Color? SecondaryText { get; init; }

    private static BluerCurvePalette Make(string bg, string prelight, string bgActive, string selected, string unfocused,
        string insBg, string insFg, string insBase, string textActive) =>
        new(Color.Parse(bg), Color.Parse(prelight), Color.Parse(bgActive), Color.Parse(selected), Color.Parse(unfocused),
            Color.Parse(insBg), Color.Parse(insFg), Color.Parse(insBase), Color.Parse(textActive));

    public static BluerCurvePalette Bluecurve { get; } = Make("#e6e6e6", "#f5f5f5", "#cccccc", "#4464ac", "#5e7ab7", "#eeeeee", "#777777", "#f0f0f0", "#ffffff");
    public static BluerCurvePalette Grape { get; } = Make("#d2d7e5", "#e2e7f5", "#b2b7c5", "#493973", "#6b5b95", "#c2c7d5", "#818a7d", "#f2f7f5", "#ffffff");
    public static BluerCurvePalette Strawberry { get; } = Make("#e6e1e1", "#ffffff", "#b3b3b3", "#b33333", "#b3b3b3", "#ccb3b3", "#996666", "#b3b3b3", "#333333");
    public static BluerCurvePalette Slate { get; } = Make("#d5d5d5", "#eeeeee", "#bbbbbb", "#333333", "#000000", "#cccccc", "#999999", "#eeeeee", "#ffffff");
    public static BluerCurvePalette Lime { get; } = Make("#d4d9d2", "#e6e9e5", "#acb6a9", "#698f64", "#d4d9d2", "#c6cbc4", "#818a7d", "#f4f9f2", "#ffffff");
    public static BluerCurvePalette Tangerine { get; } = Make("#dcd5d0", "#eeebe8", "#bbafa6", "#df7501", "#bbafa6", "#cabfb8", "#908176", "#eefaf6", "#ffffff");
    public static BluerCurvePalette BerriesAndCream { get; } = Make("#edede4", "#f5f5f1", "#d6d6c3", "#5355a1", "#a0bdee", "#e0e0da", "#747354", "#f0f0f0", "#ffffff");
    public static BluerCurvePalette Gnome { get; } = Make("#dcdad5", "#eeebe7", "#bab5ab", "#4b6983", "#9db8d2", "#dcdad5", "#777777", "#f0f0f0", "#ffffff");

    private static BluerCurvePalette MakeDark(string bg, string prelight, string bgActive, string selected, string unfocused,
        string insBg, string insFg, string insBase, string baseColor, string fg = "#e6e6e6", string text = "#eeeeee")
    {
        var bgColor = Color.Parse(bg);
        return new BluerCurvePalette(bgColor, Color.Parse(prelight), Color.Parse(bgActive), Color.Parse(selected), Color.Parse(unfocused),
            Color.Parse(insBg), Color.Parse(insFg), Color.Parse(insBase), Colors.White)
        {
            Fg = Color.Parse(fg),
            Text = Color.Parse(text),
            Base = Color.Parse(baseColor),
            SelectedFg = Colors.White,
            Warning = Color.Parse("#fcaf3e"),
            Error = Color.Parse("#ef2929"),
            Success = Color.Parse("#8ae234"),
            Link = Color.Parse("#7fb1f5"),
            LinkVisited = Color.Parse("#c69ae6"),
            TooltipBg = ColorMath.Shade(bgColor, 0.8),
            TooltipFg = Color.Parse("#eeeeee"),
            TooltipBorder = ColorMath.Shade(bgColor, 1.6),
            BevelLight = ColorMath.Shade(bgColor, 1.55),
            DisabledTextShadow = ColorMath.Shade(bgColor, 0.6),
            EntryDisabledText = Color.Parse("#9a9a9a"),
            FrameOutline = Color.Parse("#141414"),
            DefaultButtonBorder = Color.Parse(fg),
            ButtonIconHover = Color.Parse(fg),
            CaptionGlyph = Color.Parse(fg),
            ButtonIcon = Color.Parse("#d4d4d4"),
            Outline = Color.Parse("#bcbcbc"),
            SecondaryText = Color.Parse("#b4b4b4"),
        };
    }

    public static BluerCurvePalette BluecurveDark { get; } = MakeDark("#3c3c3c", "#4a4a4a", "#2c2c2c", "#4464ac", "#5e7ab7", "#3c3c3c", "#b8b8b8", "#333333", "#2b2b2b");
    public static BluerCurvePalette GrapeDark { get; } = MakeDark("#383a45", "#454856", "#2a2c34", "#6b5b95", "#493973", "#383a45", "#b6b9c6", "#2e3038", "#272930");
    public static BluerCurvePalette StrawberryDark { get; } = MakeDark("#3f3a3a", "#4d4747", "#2f2b2b", "#b33333", "#7a3b3b", "#3f3a3a", "#bdb1b1", "#332f2f", "#2c2828");
    public static BluerCurvePalette SlateDark { get; } = MakeDark("#383838", "#454545", "#282828", "#707070", "#5a5a5a", "#383838", "#b6b6b6", "#2f2f2f", "#262626");
    public static BluerCurvePalette LimeDark { get; } = MakeDark("#363a35", "#434840", "#282b27", "#698f64", "#4e6b4a", "#363a35", "#b3bab0", "#2d302c", "#262925");
    public static BluerCurvePalette TangerineDark { get; } = MakeDark("#3d3833", "#4a4540", "#2c2825", "#df7501", "#9a5a1a", "#3d3833", "#bcb5af", "#322e2a", "#2a2723");
    public static BluerCurvePalette BerriesAndCreamDark { get; } = MakeDark("#3b3b35", "#484842", "#2b2b27", "#5355a1", "#4a6a9e", "#3b3b35", "#b9b9a7", "#31312d", "#282825");
    public static BluerCurvePalette GnomeDark { get; } = MakeDark("#3a3936", "#474643", "#2b2a28", "#4b6983", "#6d8aa3", "#3a3936", "#b8b6b1", "#302f2d", "#282725");

    public static BluerCurvePalette FromVariant(BluerCurveVariant variant) => variant switch
    {
        BluerCurveVariant.Grape => Grape,
        BluerCurveVariant.Strawberry => Strawberry,
        BluerCurveVariant.Slate => Slate,
        BluerCurveVariant.Lime => Lime,
        BluerCurveVariant.Tangerine => Tangerine,
        BluerCurveVariant.BerriesAndCream => BerriesAndCream,
        BluerCurveVariant.Gnome => Gnome,
        _ => Bluecurve,
    };

    public static BluerCurvePalette FromVariantDark(BluerCurveVariant variant) => variant switch
    {
        BluerCurveVariant.Grape => GrapeDark,
        BluerCurveVariant.Strawberry => StrawberryDark,
        BluerCurveVariant.Slate => SlateDark,
        BluerCurveVariant.Lime => LimeDark,
        BluerCurveVariant.Tangerine => TangerineDark,
        BluerCurveVariant.BerriesAndCream => BerriesAndCreamDark,
        BluerCurveVariant.Gnome => GnomeDark,
        _ => BluecurveDark,
    };

    public static BluerCurvePalette FromVariant(BluerCurveVariant variant, ThemeVariant themeVariant) =>
        themeVariant == ThemeVariant.Dark ? FromVariantDark(variant) : FromVariant(variant);
}
