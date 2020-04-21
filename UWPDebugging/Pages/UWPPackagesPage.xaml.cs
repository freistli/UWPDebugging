using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Management.Deployment;
using Windows.UI.Popups;
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
    public sealed partial class UWPPackagesPage : Page
    {
        public UWPPackagesPage()
        {
            this.InitializeComponent();
        }

        private async void RemoveUWP_Click(object sender, RoutedEventArgs e)
        {
            PackageManager packageManager = new Windows.Management.Deployment.PackageManager();

            string inputPackageFullName = "015eefc1-86d1-4560-9488-c276a3c4b7cf_4.0.0.0_x64__2dhr6hz02r3tt";
            await packageManager.RemovePackageAsync(inputPackageFullName).AsTask().ConfigureAwait(true);

            MessageDialog md = new MessageDialog("Completed");
            await md.ShowAsync();
        }
    }
}
