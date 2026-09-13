using Avalonia.Media;

namespace BluerCurve;

public static class ColorMath
{
    public static Color Shade(Color c, double k)
    {
        RgbToHls(c.R / 255.0, c.G / 255.0, c.B / 255.0, out var h, out var l, out var s);
        l = Math.Clamp(l * k, 0, 1);
        s = Math.Clamp(s * k, 0, 1);
        HlsToRgb(h, l, s, out var r, out var g, out var b);
        return Color.FromArgb(c.A, ToByte(r), ToByte(g), ToByte(b));
    }

    public static Color Blend(Color a, Color b, double t)
    {
        t = Math.Clamp(t, 0, 1);
        return Color.FromArgb(
            ToByte((a.A + (b.A - a.A) * t) / 255.0),
            ToByte((a.R + (b.R - a.R) * t) / 255.0),
            ToByte((a.G + (b.G - a.G) * t) / 255.0),
            ToByte((a.B + (b.B - a.B) * t) / 255.0));
    }

    public static Color Mix(Color a, Color b, double t) => Blend(a, b, t);

    public static Color WithAlpha(Color c, double alpha) => Color.FromArgb(ToByte(alpha), c.R, c.G, c.B);

    public static Color Light(Color c) => Shade(c, 1.3);

    public static Color Dark(Color c) => Shade(c, 0.7);

    public static Color Over(Color bottom, Color top)
    {
        var a = top.A / 255.0;
        return Color.FromArgb(255,
            ToByte((top.R * a + bottom.R * (1 - a)) / 255.0),
            ToByte((top.G * a + bottom.G * (1 - a)) / 255.0),
            ToByte((top.B * a + bottom.B * (1 - a)) / 255.0));
    }

    private static byte ToByte(double v) => (byte)Math.Clamp((int)Math.Round(v * 255.0), 0, 255);

    private static void RgbToHls(double r, double g, double b, out double h, out double l, out double s)
    {
        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));
        l = (max + min) / 2;
        s = 0;
        h = 0;
        if (max != min)
        {
            s = l <= 0.5 ? (max - min) / (max + min) : (max - min) / (2 - max - min);
            var delta = max - min;
            if (r == max) h = (g - b) / delta;
            else if (g == max) h = 2 + (b - r) / delta;
            else h = 4 + (r - g) / delta;
            h *= 60;
            if (h < 0) h += 360;
        }
    }

    private static void HlsToRgb(double h, double l, double s, out double r, out double g, out double b)
    {
        var m2 = l <= 0.5 ? l * (1 + s) : l + s - l * s;
        var m1 = 2 * l - m2;
        if (s == 0)
        {
            r = g = b = l;
            return;
        }
        r = Channel(m1, m2, h + 120);
        g = Channel(m1, m2, h);
        b = Channel(m1, m2, h - 120);
    }

    private static double Channel(double m1, double m2, double hue)
    {
        while (hue > 360) hue -= 360;
        while (hue < 0) hue += 360;
        if (hue < 60) return m1 + (m2 - m1) * hue / 60;
        if (hue < 180) return m2;
        if (hue < 240) return m1 + (m2 - m1) * (240 - hue) / 60;
        return m1;
    }
}
