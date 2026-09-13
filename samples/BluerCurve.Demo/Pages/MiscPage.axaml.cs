using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;

namespace BluerCurve.Demo;

public partial class MiscPage : UserControl
{
    private WindowNotificationManager? _manager;

    public MiscPage()
    {
        InitializeComponent();
        Cards.Children.Add(new NotificationCard { Content = new Notification("Information", "The document was reloaded from disk.", NotificationType.Information) });
        Cards.Children.Add(new NotificationCard { Content = new Notification("Success", "All files were saved.", NotificationType.Success), NotificationType = NotificationType.Success });
        Cards.Children.Add(new NotificationCard { Content = new Notification("Warning", "The file has been modified by another program.", NotificationType.Warning), NotificationType = NotificationType.Warning });
        Cards.Children.Add(new NotificationCard { Content = new Notification("Error", "Could not write to /etc/fstab: permission denied.", NotificationType.Error), NotificationType = NotificationType.Error });
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (_manager is null && TopLevel.GetTopLevel(this) is { } top)
            _manager = new WindowNotificationManager(top) { Position = NotificationPosition.TopRight, MaxItems = 3 };
    }

    private void OnShowNotification(object? sender, RoutedEventArgs e) =>
        _manager?.Show(new Notification("BluerCurve", "This is a window notification.", NotificationType.Information));

    private void OnNoop(object? sender, RoutedEventArgs e) { }
}
