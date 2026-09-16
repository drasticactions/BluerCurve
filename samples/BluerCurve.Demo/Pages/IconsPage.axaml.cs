using Avalonia.Controls;

namespace BluerCurve.Demo;

public partial class IconsPage : UserControl
{
    public IconsPage()
    {
        InitializeComponent();
        DataContext = new IconsPageViewModel();
    }
}
