using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Notifications.Management;
using Windows.UI.Notifications;
using System.Linq;
using System.Threading;

public class NotificationManager
{
    private UserNotificationListener listener;
    private uint lastSeenNotificationId = 0;
    private DateTime lastSeenNotificationTime = DateTime.MinValue;

    public async Task InitializeListener()
    {
        listener = UserNotificationListener.Current;
        var accessStatus = await listener.RequestAccessAsync();

        switch (accessStatus)
        {
            case UserNotificationListenerAccessStatus.Allowed:
                break;
            case UserNotificationListenerAccessStatus.Denied:
                break;
            case UserNotificationListenerAccessStatus.Unspecified:
                break;
        }
    }

    public async Task ReadNewNotifications()
    {
        try
        {
            var notifications = await listener.GetNotificationsAsync(NotificationKinds.Toast);
            lastSeenNotificationId = notifications.LastOrDefault()?.Id ?? lastSeenNotificationId;
            if (listener == null)
            {
                throw new InvalidOperationException("Notification listener not initialized.");
            }

            while (true)
            {
                
                notifications = await listener.GetNotificationsAsync(NotificationKinds.Toast);
                if (notifications.Count > 0)
                {
                    foreach (var notification in notifications)
                    {
                        if (notification.Id > lastSeenNotificationId)
                        {
                            lastSeenNotificationId = notification.Id;

                            var appDisplayName = notification.AppInfo.DisplayInfo.DisplayName;

                            NotificationBinding toastBinding = notification.Notification.Visual.GetBinding(KnownNotificationBindings.ToastGeneric);

                            if (toastBinding != null)
                            {
                                IReadOnlyList<AdaptiveNotificationText> textElements = toastBinding.GetTextElements();

                                string titleText = textElements.FirstOrDefault()?.Text;
                                string bodyText = string.Join("\n", textElements.Skip(1).Select(t => t.Text));
                                Console.WriteLine($"New Notification - App: {appDisplayName}");
                                Console.WriteLine($"Title: {titleText}");
                                Console.WriteLine($"Body: {bodyText}");
                            }
                        }
                    }
                }
                await Task.Delay(0);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading new notifications: {ex.Message}");
        }
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        var notificationManager = new NotificationManager();

        try
        {
            await notificationManager.InitializeListener();

            // Run ReadNewNotifications asynchronously
            var readNotificationsTask = notificationManager.ReadNewNotifications();

            // Delay for 30 seconds
            await Task.Delay(30000);

            // Cancel the ReadNewNotifications task after 30 seconds
            var cts = new CancellationTokenSource();
            cts.Cancel();
            return;
            //await readNotificationsTask.ContinueWith(_ => { }, cts.Token, TaskContinuationOptions.None, TaskScheduler.Default);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}