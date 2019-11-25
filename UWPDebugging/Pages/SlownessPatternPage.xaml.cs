using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWPDebugging.Classes;
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
    public sealed partial class SlownessPatternPage : Page
    {
        Demo demo = new Demo();

        public SlownessPatternPage()
        {
            this.InitializeComponent();
            
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
    }
}
