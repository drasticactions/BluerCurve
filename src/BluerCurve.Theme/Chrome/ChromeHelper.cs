using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace BluerCurve.Chrome;

internal static class ChromeHelper
{
    public static Window? FindHostWindow(Visual visual)
    {
        if (TopLevel.GetTopLevel(visual) is Window direct)
            return direct;

        foreach (var ancestor in visual.GetVisualAncestors())
        {
            foreach (var child in ancestor.GetVisualChildren())
            {
                if (child is Window window)
                    return window;
            }
        }

        return null;
    }
}
