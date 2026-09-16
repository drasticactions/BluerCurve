using System.ComponentModel;
using System.Runtime.CompilerServices;
using BluerCurve.Icons;

namespace BluerCurve.Demo;

public sealed class IconsPageViewModel : INotifyPropertyChanged
{
    private int _selectedSize = 48;
    private string _selectedContext = BluecurveIconContext.Actions;
    private string _filter = string.Empty;
    private string _lookupName = "edit-copy";
    private IReadOnlyList<BluecurveIcon> _icons = [];
    private BluecurveIcon? _lookupResult;

    public IconsPageViewModel() => Refresh();

    public IReadOnlyList<int> Sizes => BluecurveIcons.Sizes;

    public IReadOnlyList<string> Contexts => BluecurveIconContext.All;

    public int SelectedSize
    {
        get => _selectedSize;
        set { if (Set(ref _selectedSize, value)) Refresh(); }
    }

    public string SelectedContext
    {
        get => _selectedContext;
        set { if (value is not null && Set(ref _selectedContext, value)) Refresh(); }
    }

    public string Filter
    {
        get => _filter;
        set { if (Set(ref _filter, value ?? string.Empty)) Refresh(); }
    }

    public string LookupName
    {
        get => _lookupName;
        set { if (Set(ref _lookupName, value ?? string.Empty)) Refresh(); }
    }

    public IReadOnlyList<BluecurveIcon> Icons
    {
        get => _icons;
        private set => Set(ref _icons, value);
    }

    public BluecurveIcon? LookupResult
    {
        get => _lookupResult;
        private set => Set(ref _lookupResult, value);
    }

    public string Summary => $"{Icons.Count} icons at {SelectedSize}x{SelectedSize}/{SelectedContext} ({BluecurveIcons.All.Count} total, {BluecurveIcons.Aliases.Count()} aliases)";

    public string LookupSummary => LookupResult is { } icon ? icon.Path : "no icon found";

    private void Refresh()
    {
        Icons = BluecurveIcons.All
            .Where(i => i.Size == SelectedSize && i.Context == SelectedContext)
            .Where(i => i.Name.Contains(Filter, StringComparison.OrdinalIgnoreCase))
            .ToList();
        LookupResult = string.IsNullOrWhiteSpace(LookupName) ? null : BluecurveIcons.Find(LookupName.Trim(), SelectedSize);
        OnPropertyChanged(nameof(Summary));
        OnPropertyChanged(nameof(LookupSummary));
    }

    private bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(name);
        return true;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
