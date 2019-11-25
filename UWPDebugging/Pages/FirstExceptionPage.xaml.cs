using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWPDebugging.Classes;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Composition;
using Windows.UI.Composition.Scenes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWPDebugging.Pages
{
 
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class FirstExceptionPage : Page
    {
        Demo demo = new Demo();
        public FirstExceptionPage()
        {
            this.InitializeComponent();
   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //By defalut this first exceptin will be handled without throwing
            //demo.ReadFileBuffer("ms-appx:///nonexist.txt");

            Uri uri = new Uri("ms-appx:///nonexist.txt");
            StorageFile.GetFileFromApplicationUriAsync(uri);
        }
    }
}
