using System.Xml;
using Avalonia.Platform;

namespace BluerCurve.Icons;

public static class BluecurveIcons
{
    public static readonly Uri BaseUri = new("avares://BluerCurve.Icons/");

    public static IReadOnlyList<int> Sizes { get; } = [16, 20, 24, 32, 36, 48, 96];

    private static readonly Lazy<IReadOnlyList<BluecurveIcon>> AllIcons = new(LoadIcons);
    private static readonly Lazy<HashSet<string>> IconPaths = new(() => new HashSet<string>(All.Select(i => i.ResourcePath), StringComparer.Ordinal));
    private static readonly Lazy<Dictionary<string, List<BluecurveIconAlias>>> AliasesByName = new(LoadAliases);

    public static IReadOnlyList<BluecurveIcon> All => AllIcons.Value;

    public static IEnumerable<BluecurveIconAlias> Aliases => AliasesByName.Value.Values.SelectMany(a => a);

    public static Uri GetUri(string name, int size, string context) => new BluecurveIcon(name, size, context).Uri;

    public static bool Exists(BluecurveIcon icon) => IconPaths.Value.Contains(icon.ResourcePath);

    public static bool Exists(string name, int size, string context) => Exists(new BluecurveIcon(name, size, context));

    public static BluecurveIcon? Find(string name, int size, string? context = null)
    {
        var contexts = context is null ? BluecurveIconContext.All : [context];
        foreach (var ctx in contexts)
        {
            var (targetName, targetContext) = ResolveAlias(name, ctx);
            if (FindInContext(targetName, size, targetContext) is { } icon)
                return icon;
        }
        return null;
    }

    public static (string Name, string Context) ResolveAlias(string name, string context)
    {
        if (AliasesByName.Value.TryGetValue(name, out var aliases))
        {
            var alias = aliases.FirstOrDefault(a => a.Context == context) ?? aliases[0];
            return (alias.TargetName, alias.TargetContext);
        }
        return (name, context);
    }

    public static Stream Open(BluecurveIcon icon) => AssetLoader.Open(icon.Uri);

    private static BluecurveIcon? FindInContext(string name, int size, string context)
    {
        var exact = new BluecurveIcon(name, size, context);
        if (Exists(exact))
            return exact;

        BluecurveIcon? best = null;
        foreach (var candidateSize in Sizes)
        {
            var candidate = new BluecurveIcon(name, candidateSize, context);
            if (!Exists(candidate))
                continue;
            if (best is null || Distance(candidateSize, size) < Distance(best.Size, size))
                best = candidate;
        }
        return best;

        static int Distance(int have, int want) => have >= want ? (have - want) * 2 : (want - have) * 2 + 1;
    }

    private static IReadOnlyList<BluecurveIcon> LoadIcons()
    {
        var icons = new List<BluecurveIcon>();
        foreach (var uri in AssetLoader.GetAssets(BaseUri, null))
        {
            if (BluecurveIcon.TryParse(uri.AbsolutePath, out var icon))
                icons.Add(icon);
        }
        return icons
            .OrderBy(i => i.Context, StringComparer.Ordinal)
            .ThenBy(i => i.Name, StringComparer.Ordinal)
            .ThenBy(i => i.Size)
            .ToArray();
    }

    private static Dictionary<string, List<BluecurveIconAlias>> LoadAliases()
    {
        var result = new Dictionary<string, List<BluecurveIconAlias>>(StringComparer.Ordinal);
        var tables = AssetLoader.GetAssets(new Uri(BaseUri, "icon-xml/"), null)
            .Where(u => u.AbsolutePath.EndsWith(".icontheme", StringComparison.Ordinal))
            .OrderBy(u => u.AbsolutePath, StringComparer.Ordinal);
        foreach (var table in tables)
        {
            using var stream = AssetLoader.Open(table);
            using var reader = XmlReader.Create(stream, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, IgnoreComments = true, IgnoreWhitespace = true });
            string? themeContext = null;
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element)
                    continue;
                if (reader.Name == "theme")
                {
                    themeContext = reader.GetAttribute("typedir");
                }
                else if (reader.Name == "alias" && themeContext is not null)
                {
                    var name = reader.GetAttribute("name");
                    var target = reader.GetAttribute("target");
                    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(target))
                        continue;
                    var alias = new BluecurveIconAlias(name, reader.GetAttribute("typedir") ?? themeContext, target, themeContext);
                    if (!result.TryGetValue(name, out var list))
                        result[name] = list = [];
                    if (!list.Contains(alias))
                        list.Add(alias);
                }
            }
        }
        return result;
    }
}
