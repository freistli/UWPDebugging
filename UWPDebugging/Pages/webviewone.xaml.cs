using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class webviewone : Page
    {
        public webviewone()
        {
            this.InitializeComponent();
            sharplink.Navigate(new Uri(@"https://api.ximalaya.com/ximalayaos-iot/xyh5/home.do?appKey=35f6cbc140574ac480ac3a5455e03e24&sn=11390_00_100390&deviceId=123321#/HP"));
            
        }

        private void refreshbutton_Click(object sender, RoutedEventArgs e)
        {
            sharplink.Navigate(new Uri(@"https://api.ximalaya.com/ximalayaos-iot/xyh5/home.do?appKey=35f6cbc140574ac480ac3a5455e03e24&sn=11390_00_100390&deviceId=123321#/HP"));
        }
    }
}
