using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPDebugging.Classes;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class OOPBackgroundtaskPage : Page
    {
        public OOPBackgroundtaskPage()
        {
            this.InitializeComponent();
        }

        private async void RegisterBGTask_Click(object sender, RoutedEventArgs e)
        {
            var existing = BackgroundHelper.FindRegistration<OOPBackgroundTask.BadgeTask>();

            if (existing == null)
                await BackgroundHelper.Register<OOPBackgroundTask.BadgeTask>(new SystemTrigger(SystemTriggerType.TimeZoneChange, false));
        }

        private async void UnregisterBGTask_Click(object sender, RoutedEventArgs e)
        {
            await BackgroundHelper.Unregister<OOPBackgroundTask.BadgeTask>();

            /*
            foreach (var t in BackgroundTaskRegistration.AllTasks)
                t.Value.Unregister(true);
                */
        }
    }
}
