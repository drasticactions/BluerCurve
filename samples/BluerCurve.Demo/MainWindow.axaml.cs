using BluerCurve.Chrome;

namespace BluerCurve.Demo;

public partial class MainWindow : BluerCurveWindow
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel(this);
    }
}
