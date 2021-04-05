using Microsoft.Toolkit.Uwp.Notifications;
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
            /*
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
            */

            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BaseUri = new Uri("ms-appx:///Assets/Weather/"),
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
            {
                new AdaptiveText()
                {
                    Text = "Today will be sunny with a high of 63 and a low of 42."
                },
                new AdaptiveGroup()
                {
                    Children =
                    {
                        new AdaptiveSubgroup()
                        {
                            HintWeight = 1,
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = "Mon",
                                    HintAlign = AdaptiveTextAlign.Center
                                },
                                new AdaptiveImage()
                                {
                                    HintRemoveMargin = true,
                                    Source = "Mostly Cloudy.png"
                                },
                                new AdaptiveText()
                                {
                                    Text = "63°",
                                    HintAlign = AdaptiveTextAlign.Center
                                },
                                new AdaptiveText()
                                {
                                    Text = "42°",
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                    HintAlign = AdaptiveTextAlign.Center
                                }
                            }
                        },
                        new AdaptiveSubgroup()
                        {
                            HintWeight = 1,
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = "Tue",
                                    HintAlign = AdaptiveTextAlign.Center
                                },
                                new AdaptiveImage()
                                {
                                    HintRemoveMargin = true,
                                    Source = "Cloudy.png"
                                },
                                new AdaptiveText()
                                {
                                    Text = "57°",
                                    HintAlign = AdaptiveTextAlign.Center
                                },
                                new AdaptiveText()
                                {
                                    Text = "38°",
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                    HintAlign = AdaptiveTextAlign.Center
                                }
                            }
                        },
                        new AdaptiveSubgroup()
                        {
                            HintWeight = 1,
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = "Wed",
                                    HintAlign = AdaptiveTextAlign.Center
                                },
                                new AdaptiveImage()
                                {
                                    HintRemoveMargin = true,
                                    Source = "Sunny.png"
                                },
                                new AdaptiveText()
                                {
                                    Text = "59°",
                                    HintAlign = AdaptiveTextAlign.Center
                                },
                                new AdaptiveText()
                                {
                                    Text = "43°",
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                    HintAlign = AdaptiveTextAlign.Center
                                }
                            }
                        },
                        new AdaptiveSubgroup()
                        {
                            HintWeight = 1,
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = "Thu",
                                    HintAlign = AdaptiveTextAlign.Center
                                },
                                new AdaptiveImage()
                                {
                                    HintRemoveMargin = true,
                                    Source = "Sunny.png"
                                },
                                new AdaptiveText()
                                {
                                    Text = "62°",
                                    HintAlign = AdaptiveTextAlign.Center
                                },
                                new AdaptiveText()
                                {
                                    Text = "42°",
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                    HintAlign = AdaptiveTextAlign.Center
                                }
                            }
                        },
                        new AdaptiveSubgroup()
                        {
                            HintWeight = 1,
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = "Fri",
                                    HintAlign = AdaptiveTextAlign.Center
                                },
                                new AdaptiveImage()
                                {
                                    HintRemoveMargin = true,
                                    Source = "Sunny.png"
                                },
                                new AdaptiveText()
                                {
                                    Text = "71°",
                                    HintAlign = AdaptiveTextAlign.Center
                                },
                                new AdaptiveText()
                                {
                                    Text = "66°",
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                    HintAlign = AdaptiveTextAlign.Center
                                }
                            }
                        }
                    }
                }
            }
                    }
                }
            };

            // Create the toast notification
            var content = toastContent.GetXml();

            // And send the notification
            //ToastNotificationManager.CreateToastNotifier().Show(toastNotif);       

            ToastNotifier toastNotifier =
            ToastNotificationManager.CreateToastNotifier();
            var scheduledToast = new ScheduledToastNotification(
              content, DateTime.Now.AddSeconds(5));
            scheduledToast.Tag = "TestTag";
            toastNotifier.AddToSchedule(scheduledToast);
            toastNotifier.ScheduledToastNotificationShowing += ToastNotifier_ScheduledToastNotificationShowing;
         
        }

        private void ToastNotifier_ScheduledToastNotificationShowing(ToastNotifier sender, ScheduledToastNotificationShowingEventArgs args)
        {
            
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddText("Andrew sent you a picture")
                .AddText("Check this out, The Enchantments in Washington!")
                .Show();

            //args.Cancel = true;
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
