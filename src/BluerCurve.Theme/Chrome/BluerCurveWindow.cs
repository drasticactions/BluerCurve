using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.VisualTree;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.Threading;

namespace BluerCurve.Chrome;

public class BluerCurveWindow : Window
{
    private const string PartTitleBar = "PART_TitleBar";
    private const string ResizeBorderClass = "bc-resize";

    private static readonly ControlTheme s_emptyDecorations = new(typeof(WindowDrawnDecorations))
    {
        Setters =
        {
            new Setter(WindowDrawnDecorations.DefaultTitleBarHeightProperty, 0.0),
            new Setter(WindowDrawnDecorations.DefaultFrameThicknessProperty, new Thickness(0)),
            new Setter(WindowDrawnDecorations.DefaultShadowThicknessProperty, new Thickness(0)),
        },
    };

    private const uint DoubleClickMs = 400;

    private Control? _titleBar;
    private ulong _lastTitlePressMs;
    private bool _titleArmed;

    protected override Type StyleKeyOverride => typeof(BluerCurveWindow);

    public BluerCurveWindow()
    {
        Configure(this);
    }

    public static void Configure(Window window)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            window.WindowDecorations = WindowDecorations.Full;
            window.ExtendClientAreaToDecorationsHint = true;
            window.ExtendClientAreaTitleBarHeightHint = 0;
            window.WindowDecorationsTheme = s_emptyDecorations;
            window.Opened += (_, _) => NativeWindows.DisableRoundedCorners(window);
        }
        else
        {
            window.WindowDecorations = WindowDecorations.None;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                window.Activated += (_, _) => NativeMacOs.EnsureResizable(window);
            }
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (_titleBar is not null)
        {
            _titleBar.PointerPressed -= OnTitleBarPointerPressed;
        }
        _titleBar = e.NameScope.Find<Control>(PartTitleBar);
        if (_titleBar is not null)
        {
            _titleBar.PointerPressed += OnTitleBarPointerPressed;
            if (WindowDecorations == WindowDecorations.None)
                TakeOverTitleBarRole(_titleBar);
        }
        foreach (var border in this.GetVisualDescendants().OfType<Border>().Where(b => b.Classes.Contains(ResizeBorderClass)))
        {
            border.PointerPressed -= OnResizeBorderPointerPressed;
            border.PointerPressed += OnResizeBorderPointerPressed;
        }
    }

    private static void TakeOverTitleBarRole(Control titleBar)
    {
        foreach (var visual in titleBar.GetSelfAndVisualDescendants())
        {
            if (WindowDecorationProperties.GetElementRole(visual) == WindowDecorationsElementRole.TitleBar)
                WindowDecorationProperties.SetElementRole(visual, WindowDecorationsElementRole.User);
        }
    }

    private void OnTitleBarPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed || e.Source is Button or MenuItem)
        {
            _titleArmed = false;
            return;
        }
        if (e.Source is Visual v && v.FindAncestorOfType<Button>(true) is not null)
        {
            _titleArmed = false;
            return;
        }

        var timeMs = e.Timestamp;
        var doubleClick = e.ClickCount == 2
            || (_titleArmed && timeMs != 0 && timeMs - _lastTitlePressMs <= DoubleClickMs);
        if (doubleClick)
        {
            _titleArmed = false;
            if (CanResize && CanMaximize)
                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            e.Handled = true;
            return;
        }

        _titleArmed = true;
        _lastTitlePressMs = timeMs;

        if (WindowDecorations == WindowDecorations.None)
        {
            BeginMoveDrag(e);
            e.Handled = true;
        }
    }

    private void OnResizeBorderPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _titleArmed = false;
        if (sender is not Border border || !CanResize || WindowState != WindowState.Normal)
            return;
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed || WindowDecorations != WindowDecorations.None)
            return;

        var edge = border.Tag switch
        {
            "N" => WindowEdge.North,
            "S" => WindowEdge.South,
            "E" => WindowEdge.East,
            "W" => WindowEdge.West,
            "NE" => WindowEdge.NorthEast,
            "NW" => WindowEdge.NorthWest,
            "SE" => WindowEdge.SouthEast,
            "SW" => WindowEdge.SouthWest,
            _ => (WindowEdge?)null,
        };
        if (edge is { } value)
        {
            BeginResizeDrag(value, e);
            e.Handled = true;
        }
    }

    private static class NativeWindows
    {
        [DllImport("dwmapi.dll")]
        private static extern unsafe int DwmSetWindowAttribute(IntPtr hwnd, int attribute, void* value, int size);

        private const int DwmwaWindowCornerPreference = 33;
        private const int DwmwcpDoNotRound = 1;

        public static void DisableRoundedCorners(Window window)
        {
            if (window.TryGetPlatformHandle() is not { } handle)
                return;
            try
            {
                unsafe
                {
                    var pref = DwmwcpDoNotRound;
                    DwmSetWindowAttribute(handle.Handle, DwmwaWindowCornerPreference, &pref, sizeof(int));
                }
            }
            catch (Exception)
            {
            }
        }
    }

    private static class NativeMacOs
    {
        private const string ObjC = "/usr/lib/libobjc.dylib";

        [DllImport(ObjC, EntryPoint = "sel_registerName")]
        private static extern IntPtr sel_registerName(string selector);

        [DllImport(ObjC, EntryPoint = "objc_msgSend")]
        private static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector);

        [DllImport(ObjC, EntryPoint = "objc_msgSend")]
        private static extern void objc_msgSend_ulong(IntPtr receiver, IntPtr selector, ulong arg);

        private const ulong NsWindowStyleMaskResizable = 1 << 3;

        public static void EnsureResizable(Window window)
        {
            if (!window.CanResize)
                return;
            DispatcherTimer.RunOnce(() =>
            {
                if (!window.CanResize || window.TryGetPlatformHandle() is not { } handle)
                    return;
                try
                {
                    var mask = (ulong)objc_msgSend(handle.Handle, sel_registerName("styleMask")).ToInt64();
                    if ((mask & NsWindowStyleMaskResizable) == 0)
                        objc_msgSend_ulong(handle.Handle, sel_registerName("setStyleMask:"), mask | NsWindowStyleMaskResizable);
                }
                catch (Exception)
                {
                }
            }, TimeSpan.FromMilliseconds(1));
        }
    }
}
