using Avalonia.Headless.XUnit;
using Avalonia.Platform;
using BluerCurve.Icons;
using Xunit;

namespace BluerCurve.Tests;

public class IconsTests
{
    [AvaloniaFact]
    public void Catalog_Contains_Every_Upstream_Svg()
    {
        Assert.Equal(3135, BluecurveIcons.All.Count);
        Assert.Equal(BluecurveIcons.Sizes, BluecurveIcons.All.Select(i => i.Size).Distinct().Order().ToList());
        Assert.Equal(BluecurveIconContext.All.Order(), BluecurveIcons.All.Select(i => i.Context).Distinct().Order());
        Assert.Equal(BluecurveIcons.All.Count, BluecurveIcons.All.Select(i => i.ResourcePath).Distinct().Count());
    }

    [AvaloniaFact]
    public void Every_Icon_Opens_As_Svg()
    {
        foreach (var icon in BluecurveIcons.All)
        {
            Assert.True(AssetLoader.Exists(icon.Uri), icon.Path);
            using var stream = BluecurveIcons.Open(icon);
            using var reader = new StreamReader(stream);
            var head = new char[512];
            var read = reader.Read(head, 0, head.Length);
            Assert.Contains("<svg", new string(head, 0, read));
        }
    }

    [AvaloniaFact]
    public void Exact_Lookup()
    {
        Assert.True(BluecurveIcons.Exists("stock-copy", 48, BluecurveIconContext.Actions));
        Assert.False(BluecurveIcons.Exists("stock-copy", 96, BluecurveIconContext.Actions));
        Assert.Equal("avares://BluerCurve.Icons/48x48/actions/stock-copy.svg", BluecurveIcons.GetUri("stock-copy", 48, "actions").ToString());
    }

    [AvaloniaFact]
    public void Find_Resolves_Freedesktop_Aliases()
    {
        var copy = BluecurveIcons.Find("edit-copy", 24);
        Assert.Equal(new BluecurveIcon("stock-copy", 24, BluecurveIconContext.Actions), copy);
        Assert.Equal(new BluecurveIcon("folder", 24, BluecurveIconContext.MimeTypes), BluecurveIcons.Find("folder", 24));
        Assert.Equal(new BluecurveIcon("folder", 24, BluecurveIconContext.MimeTypes), BluecurveIcons.Find("folder", 24, BluecurveIconContext.Places));

        Assert.Equal(("stock-execute", "actions"), BluecurveIcons.ResolveAlias("system-run", "actions"));
        Assert.Equal(("icon-runapp", "apps"), BluecurveIcons.ResolveAlias("system-run", "apps"));
        Assert.Equal(("stock-copy", "actions"), BluecurveIcons.ResolveAlias("stock-copy", "actions"));

        Assert.True(BluecurveIcons.Aliases.Count() > 2000);
        foreach (var alias in BluecurveIcons.Aliases)
            Assert.True(BluecurveIcons.Sizes.Any(s => BluecurveIcons.Exists(alias.TargetName, s, alias.TargetContext)), $"{alias.Context}/{alias.Name} -> {alias.TargetContext}/{alias.TargetName}");
    }

    [AvaloniaFact]
    public void Find_Falls_Back_To_Closest_Size()
    {
        Assert.Equal(48, BluecurveIcons.Find("stock-copy", 96)!.Size);
        Assert.Equal(48, BluecurveIcons.Find("stock-copy", 40)!.Size);
        Assert.Equal(32, BluecurveIcons.Find("stock-copy", 36)!.Size);
        Assert.Null(BluecurveIcons.Find("no-such-icon", 48));
        Assert.Null(BluecurveIcons.Find("stock-copy", 48, BluecurveIconContext.Places));
    }

    [AvaloniaFact]
    public void Parse_Resource_Paths()
    {
        Assert.True(BluecurveIcon.TryParse("/16x16/places/folder.svg", out var icon));
        Assert.Equal(new BluecurveIcon("folder", 16, "places"), icon);
        Assert.False(BluecurveIcon.TryParse("/icon-xml/icons-actions.icontheme", out _));
        Assert.False(BluecurveIcon.TryParse("/16x24/places/folder.svg", out _));
    }
}
