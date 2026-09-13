using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using BluerCurve.Chrome;

namespace BluerCurve.Demo;

public sealed class VariantItem
{
    public VariantItem(BluerCurveVariant variant, Action<BluerCurveVariant> select)
    {
        Variant = variant;
        Select = new RelayCommand(() => select(variant));
    }

    public BluerCurveVariant Variant { get; }
    public string Name => Variant == BluerCurveVariant.BerriesAndCream ? "Berries and Cream" : Variant.ToString();
    public ICommand Select { get; }
}

public sealed class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly Window _owner;
    private VariantItem _selectedVariant;

    public MainWindowViewModel(Window owner)
    {
        _owner = owner;
        Variants = Enum.GetValues<BluerCurveVariant>().Select(v => new VariantItem(v, ApplyVariant)).ToList();
        _selectedVariant = Variants.First(v => v.Variant == Theme.Variant);
        OpenPlainWindow = new RelayCommand(() => new Window
        {
            Title = "Plain Window",
            Width = 400, Height = 240,
            Content = new TextBlock { Margin = new Thickness(12), TextWrapping = Avalonia.Media.TextWrapping.Wrap, Text = "A plain Window. On Windows and Linux with drawn decorations this uses the WindowDrawnDecorations theme; on macOS the native chrome is shown." },
        }.Show(_owner));
        OpenBluerCurveWindow = new RelayCommand(() => new BluerCurveWindow
        {
            Title = "BluerCurveWindow",
            Width = 400, Height = 240,
            Content = new TextBlock { Margin = new Thickness(12), TextWrapping = Avalonia.Media.TextWrapping.Wrap, Text = "A BluerCurveWindow draws the Metacity frame itself on every platform." },
        }.Show(_owner));
        Quit = new RelayCommand(() => (Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Shutdown());
    }

    private static BluerCurveTheme Theme => Application.Current!.Styles.OfType<BluerCurveTheme>().First();

    public IReadOnlyList<VariantItem> Variants { get; }

    public VariantItem SelectedVariant
    {
        get => _selectedVariant;
        set
        {
            if (value is null || ReferenceEquals(_selectedVariant, value)) return;
            _selectedVariant = value;
            Theme.Variant = value.Variant;
            OnPropertyChanged();
        }
    }

    public ICommand OpenPlainWindow { get; }
    public ICommand OpenBluerCurveWindow { get; }
    public ICommand Quit { get; }

    private void ApplyVariant(BluerCurveVariant variant) => SelectedVariant = Variants.First(v => v.Variant == variant);

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public sealed class RelayCommand(Action execute) : ICommand
{
    public event EventHandler? CanExecuteChanged { add { } remove { } }
    public bool CanExecute(object? parameter) => true;
    public void Execute(object? parameter) => execute();
}
