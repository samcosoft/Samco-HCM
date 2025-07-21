using Notifications.Wpf.Core;

namespace Samco_HCM_Dentistry_Client.Classes;

public static class MainNotify
{
    public static void ShowSuccess(string title, string message)
    {
        var notifyManager = new NotificationManager();
        notifyManager.ShowAsync(new NotificationContent
        {
            Title = title,
            Message = message,
            Type = NotificationType.Success
        });
    }

    public static void ShowWarning(string title, string message)
    {
        var notifyManager = new NotificationManager();
        notifyManager.ShowAsync(new NotificationContent
        {
            Title = title,
            Message = message,
            Type = NotificationType.Warning
        });
    }

    public static void ShowInformation(string title, string message)
    {
        var notifyManager = new NotificationManager();
        notifyManager.ShowAsync(new NotificationContent
        {
            Title = title,
            Message = message,
            Type = NotificationType.Information
        });
    }

    public static void ShowError(string title, string message)
    {
        var notifyManager = new NotificationManager();
        notifyManager.ShowAsync(new NotificationContent
        {
            Title = title,
            Message = message,
            Type = NotificationType.Error
        });
    }
}