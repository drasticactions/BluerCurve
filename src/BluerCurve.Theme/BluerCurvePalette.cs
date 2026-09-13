using Avalonia.Media;

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
}
