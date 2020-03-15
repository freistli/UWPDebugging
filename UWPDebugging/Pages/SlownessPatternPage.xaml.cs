using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWPDebugging.Classes;
using Windows.ApplicationModel.LockScreen;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.System.UserProfile;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.ViewManagement;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPDebugging.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SlownessPatternPage : Page
    {
        Demo demo = new Demo();

        public SlownessPatternPage()
        {
            this.InitializeComponent();
            ((UWPDebugging.App)App.Current).StartAnimator += OnStartAnimatorMessage;
            Logging.SingleInstance.LogMessage("SlownessPatternPage Created");

            LockApplicationHost lockHost = LockApplicationHost.GetForCurrentView();
            if (lockHost != null)
            {
                lockHost.Unlocking += LockHost_Unlocking;
                Logging.SingleInstance.LogMessage("SlownessPatternPage setup unlocking event handler");
            }
        }

        private void OnStartAnimatorMessage(object sender, object e)
        {
            Logging.SingleInstance.LogMessage("AppService triggered start Animator message");
            expandingPanel.StartAnimation();
        }

        private void LockHost_Unlocking(LockApplicationHost sender, LockScreenUnlockingEventArgs args)
        {
            // save any unsaved work and gracefully exit the app
            Logging.SingleInstance.LogMessage("LockHost_Unlocking");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           Task.Run(async ()=>await demo.CrossThreads()).GetAwaiter().GetResult();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await demo.CrossThreads();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            demo.CrossThreads().Wait();
        }
        
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            Logging.SingleInstance.LogMessage("OnNavigatedTo");
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            LockApplicationHost host = LockApplicationHost.GetForCurrentView();
              if (host != null)
             { 
                  host.RequestUnlock();
                Logging.SingleInstance.LogMessage("Lockhost RequestUnlock");
            }
            else
            {
                
                await ApplicationView.GetForCurrentView().TryConsolidateAsync();
                Logging.SingleInstance.LogMessage("TryConsolidateAsync");
            }
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {

            IRandomAccessStream imageStream = Windows.System.UserProfile.LockScreen.GetImageStream();

            var bitmapImage = new BitmapImage();
            var backgroundImageBrush = new ImageBrush();
            bitmapImage.DecodePixelWidth = 64; // just good practice
            await bitmapImage.SetSourceAsync(imageStream);
            backgroundImageBrush.ImageSource = bitmapImage;

            panel.Background = backgroundImageBrush;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var view = ApplicationView.GetForCurrentView();

            //https://docs.microsoft.com/en-us/uwp/api/windows.ui.viewmanagement.applicationview.setpreferredminsize
            view.SetPreferredMinSize(new Size(192, 48));

            if (view.TryResizeView(new Size { Width = 192, Height = 48 }))
            {

            }
            else
            {

            }

        }

        private async void Button_Click_6(object sender, RoutedEventArgs e)
        {
            await ApplicationView.GetForCurrentView().TryEnterViewModeAsync((ApplicationViewMode)viewOption.SelectedIndex);
            
        }

        private void StartAnimation_Click(object sender, RoutedEventArgs e)
        {
            expandingPanel.StartAnimation();
        }
    }
}
