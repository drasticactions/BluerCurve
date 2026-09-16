namespace BluerCurve.Icons;

public sealed record BluecurveIcon(string Name, int Size, string Context)
{
    public string ResourcePath => $"{Size}x{Size}/{Context}/{Name}.svg";

    public Uri Uri => new(BluecurveIcons.BaseUri, ResourcePath);

    public string Path => Uri.ToString();

    public override string ToString() => Path;

    public static bool TryParse(string resourcePath, out BluecurveIcon icon)
    {
        icon = null!;
        var parts = resourcePath.Trim('/').Split('/');
        if (parts.Length != 3 || !parts[2].EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
            return false;
        var dims = parts[0].Split('x');
        if (dims.Length != 2 || !int.TryParse(dims[0], out var size) || dims[1] != dims[0])
            return false;
        icon = new BluecurveIcon(parts[2][..^4], size, parts[1]);
        return true;
    }
}
