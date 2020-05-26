using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPDebugging.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlarmToast : Page
    {
        public AlarmToast()
        {
            this.InitializeComponent();
        }         

        private async void ButtonAlarmManager_Click(object sender, RoutedEventArgs e)
        {
            AlarmAccessStatus aas = await AlarmApplicationManager.RequestAccessAsync();            
        }

        private void ButtonPopUpAlarmNotification_Click(object sender, RoutedEventArgs e)
        {
            string textLine1 = "Sample Toast App";
            string textLine2 = "This is a sample message.";
            string contentString =
              "<toast duration=\"long\">\n" +
                "<visual>\n" +
                  "<binding template=\"ToastText02\">\n" +
                    "<text id=\"1\">" + textLine1 + "</text>\n" +
                    "<text id=\"2\">" + textLine2 + "</text>\n" +
                  "</binding>\n" +
                "</visual>\n" +
              "</toast>\n";
            XmlDocument content = new Windows.Data.Xml.Dom.XmlDocument();
            content.LoadXml(contentString);

            ToastNotifier toastNotifier =
            ToastNotificationManager.CreateToastNotifier();
            var scheduledToast = new ScheduledToastNotification(
              content, DateTime.Now.AddSeconds(5));
            toastNotifier.AddToSchedule(scheduledToast);

        }

        private void ButtonClearAllAlarmNotifications_Click(object sender, RoutedEventArgs e)
        {
            var toastNotifier = ToastNotificationManager.CreateToastNotifier();
            var notifications = toastNotifier.GetScheduledToastNotifications();
            // Remove each notification from the schedule
            foreach (var notification in notifications)
            {
                toastNotifier.RemoveFromSchedule(notification);
            }
        }
    }
}
