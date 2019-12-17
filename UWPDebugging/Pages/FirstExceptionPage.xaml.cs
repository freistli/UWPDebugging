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
using Windows.Media.Core;
using Windows.Media.Playback;
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
        private MediaPlayer _mediaPlayer;

        public FirstExceptionPage()
        {
            this.InitializeComponent();
   
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //By defalut this first exceptin will be handled without throwing
            //demo.ReadFileBuffer("ms-appx:///nonexist.txt");
            /*
            Uri uri = new Uri("ms-appx:///nonexist.txt");
            await StorageFile.GetFileFromApplicationUriAsync(uri);
            */
            System.Uri manifestUri = new Uri("http://amssamples.streaming.mediaservices.windows.net/49b57c87-f5f3-48b3-ba22-c55cfdffa9cb/Sintel.ism/manifest(format=m3u8-aapl)");
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.Source = MediaSource.CreateFromUri(manifestUri);
            _mediaPlayer.Play();
        }
    }
}
