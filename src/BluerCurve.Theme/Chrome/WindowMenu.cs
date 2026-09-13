using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;

namespace BluerCurve.Chrome;

public static class WindowMenu
{
    public static readonly AttachedProperty<AvaloniaList<object>?> ItemsProperty =
        AvaloniaProperty.RegisterAttached<Window, AvaloniaList<object>?>("Items", typeof(WindowMenu));

    public static AvaloniaList<object>? GetItems(Window window) => window.GetValue(ItemsProperty);

    public static void SetItems(Window window, AvaloniaList<object>? value) => window.SetValue(ItemsProperty, value);
}
