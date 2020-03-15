using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;

namespace OOPBackgroundTask
{
    public sealed class BadgeTask: IBackgroundTask
    {
        private AppServiceConnection appService;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var cost = BackgroundWorkCost.CurrentBackgroundWorkCost;
            if (cost == BackgroundWorkCostValue.High)
            {
                Debug.WriteLine("Background task aborted (cost is high)");
            }
            else
            {
                // handle canceled
                taskInstance.Canceled += (s, e) =>
                {
                    Debug.WriteLine("Background task canceled");
                };

                // perform task
                var deferral = taskInstance.GetDeferral();
                try
                {
                    var seed = (int)DateTime.Now.Ticks;
                    var random = new Random(seed);
                    var value = random.Next(1, 50);
                    UpdateTile(value);
                    StartAppService().GetAwaiter().GetResult();

                    Debug.WriteLine("Background task complete: " + value.ToString());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Background task error: " + ex.Message);
                }
                finally
                {
                    deferral.Complete();
                }

            }
        }

        public static void UpdateTile(int value)
        {            
            var template = ToastTemplateType.ToastText01;
            var xml = ToastNotificationManager.GetTemplateContent(template);
            var text = xml.CreateTextNode(string.Format("Toast updated to {0}", value));
            var elements = xml.GetElementsByTagName("text");
            elements[0].AppendChild(text);

            var toast = new ToastNotification(xml);
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);

            Debug.WriteLine("Background task toast shown: " + value.ToString());

            /*
            var type = BadgeTemplateType.BadgeNumber;
            xml = BadgeUpdateManager.GetTemplateContent(type);
            elements = xml.GetElementsByTagName("badge");
            var element = elements[0] as XmlElement;
            element.SetAttribute("value", value.ToString());
            */

            /*
            string badgeGlyphValue = "alert";

            // Get the blank badge XML payload for a badge glyph
            var badgeXml =
                BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeGlyph);

            // Set the value of the badge in the XML to our glyph value
            Windows.Data.Xml.Dom.XmlElement badgeElement =
                badgeXml.SelectSingleNode("/badge") as Windows.Data.Xml.Dom.XmlElement;
            badgeElement.SetAttribute("value", badgeGlyphValue);
            */

            string badgeXmlString = string.Format("<badge value='{0}'/>",value);
            Windows.Data.Xml.Dom.XmlDocument badgeDOM = new Windows.Data.Xml.Dom.XmlDocument();
            badgeDOM.LoadXml(badgeXmlString);

            var updater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            var notification = new BadgeNotification(badgeDOM);
            updater.Update(notification);

            Debug.WriteLine("Background task badge updated: " + value.ToString());
        }

        private async Task StartAppService()
        {
            if (this.appService == null)
            {
                this.appService = new AppServiceConnection();

                // Here, we use the app service name defined in the app service 
                // provider's Package.appxmanifest file in the <Extension> section.
                this.appService.AppServiceName = "InProcessAppService";

                // Use Windows.ApplicationModel.Package.Current.Id.FamilyName 
                // within the app service provider to get this value.
                this.appService.PackageFamilyName = "30812FreistLi.HoloDomino_33rssfepyhexc";

                var status = await this.appService.OpenAsync();

                if (status != AppServiceConnectionStatus.Success)
                {
                    Debug.WriteLine("Failed to create");
                    return;
                }
            }

            var message = new ValueSet();
            message.Add("Request", "StartAnimator"); 
            AppServiceResponse response = await this.appService.SendMessageAsync(message);
            string result = "";

            if (response.Status == AppServiceResponseStatus.Success)
            {
                // Get the data  that the service sent to us.
                if (response.Message["Response"] as string == "OK")
                {
                    result = response.Message["StatusCode"] as string;
                }
            }

            message.Clear();
          
        }
    }
}
