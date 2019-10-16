using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
    public sealed partial class HighMemoryPage : Page
    {
        List<Demo> demoList = new List<Demo>();

        public HighMemoryPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Demo demo = new Demo();
            Windows.UI.Xaml.Shapes.Rectangle modelhost = new Windows.UI.Xaml.Shapes.Rectangle();
            modelhost.Width = modelhost.Height = 200;
            myStack.Children.Add(modelhost);
            demoList.Add(demo);
            await demo.CrossThread4(modelhost);
        }
    }
}
