using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWPDebugging.Classes;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Composition.Scenes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Microsoft.UI.Xaml.Controls;

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
        Compositor _compositor = new Compositor();
        ContainerVisual _root;
        Windows.UI.Composition.CompositionTarget _compositionTarget;
        Random _random;

        private SpriteVisual GetRadialGradientVisualWithAnimation(Vector2 size,
                                                          Vector2 gradientOrigin,
                                                          Vector2 ellipseCenter,
                                                          Vector2 ellipseRadius)
        {
           
            // Create radial gradient brush.
            var gradientBrush = _compositor.CreateRadialGradientBrush();
            gradientBrush.GradientOriginOffset = gradientOrigin;
            gradientBrush.EllipseCenter = ellipseCenter;
            gradientBrush.EllipseRadius = ellipseRadius;

            // Add the color stops. The first color stop needs a name so you can refer to it later.
            CompositionColorGradientStop ColorStop1 = _compositor.CreateColorGradientStop(0, Colors.Blue);
            gradientBrush.ColorStops.Add(ColorStop1);
            gradientBrush.ColorStops.Add(_compositor.CreateColorGradientStop(1, Colors.LightBlue));
            gradientBrush.ColorStops.Add(_compositor.CreateColorGradientStop(1, Colors.Navy));
            gradientBrush.ColorStops.Add(_compositor.CreateColorGradientStop(1, Colors.White));

            // Set up animation for ColorStop1's color.
            var colorAnimation = _compositor.CreateColorKeyFrameAnimation();
            
            colorAnimation.InsertKeyFrame(0.0f, Colors.Blue);
            colorAnimation.InsertKeyFrame(0.5f, Colors.LightBlue);
            colorAnimation.InsertKeyFrame(0.75f, Colors.Navy);
            colorAnimation.InsertKeyFrame(1.0f, Colors.Blue);
            colorAnimation.Duration = TimeSpan.FromSeconds(5);
            colorAnimation.IterationBehavior = AnimationIterationBehavior.Forever;
            ColorStop1.StartAnimation("Color", colorAnimation);

            // SpriteVisual to be painted with gradated content.
            var gradientVisual = _compositor.CreateSpriteVisual();
            gradientVisual.Size = size;
            // Set brush on the SpriteVisual.
            gradientVisual.Brush = gradientBrush;

            return gradientVisual;
        }
        Visual CreateChildElement()
        {
            //
            // Each element consists of three visuals, which produce the appearance
            // of a framed rectangle
            //
            var element = _compositor.CreateContainerVisual();
            element.Size = new Vector2(100.0f, 100.0f);

            //
            // Position this visual randomly within our window
            //
            element.Offset = new Vector3((float)(_random.NextDouble() * 400), (float)(_random.NextDouble() * 400), 0.0f);

            //
            // The outer rectangle is always white
            //
            var visual = _compositor.CreateSpriteVisual();
            element.Children.InsertAtTop(visual);
            visual.Brush = _compositor.CreateColorBrush(Windows.UI.Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
            visual.Size = new Vector2(100.0f, 100.0f);

            //
            // The inner rectangle is inset from the outer by three pixels all around
            //
            var child = _compositor.CreateSpriteVisual();
            visual.Children.InsertAtTop(child);
            child.Offset = new Vector3(3.0f, 3.0f, 0.0f);
            child.Size = new Vector2(94.0f, 94.0f);

            //
            // Pick a random color for every rectangle
            //
            byte red = (byte)(0xFF * (0.2f + (_random.NextDouble() / 0.8f)));
            byte green = (byte)(0xFF * (0.2f + (_random.NextDouble() / 0.8f)));
            byte blue = (byte)(0xFF * (0.2f + (_random.NextDouble() / 0.8f)));
            child.Brush = _compositor.CreateColorBrush(Windows.UI.Color.FromArgb(0xFF, red, green, blue));

            //
            // Make the subtree root visual partially transparent. This will cause each visual in the subtree
            // to render partially transparent, since a visual's opacity is multiplied with its parent's
            // opacity
            //
            element.Opacity = 0.8f;

            return element;
        }

        public FirstExceptionPage()
        {

            this.InitializeComponent();

            _random = new Random();
            _compositor = Window.Current.Compositor;

            _root = _compositor.CreateContainerVisual();
            _root.RelativeSizeAdjustment = Vector2.One;
            _root.Offset = new Vector3(0, 50, 0);

            Windows.UI.Xaml.Shapes.Rectangle modelhost = new Windows.UI.Xaml.Shapes.Rectangle();
            modelhost.Width = modelhost.Height = myStack.Width;
            myStack.Children.Add(modelhost);

            ElementCompositionPreview.SetElementChildVisual(modelhost, _root);

            _root.Children.InsertAtTop(GetRadialGradientVisualWithAnimation(new Vector2(120, 120), new Vector2(0, 0), new Vector2(0.5f, 0.5f), new Vector2(0.4f, 0.5f)));

            for (int index = 0; index < 10; index++)
            {
                _root.Children.InsertAtTop(CreateChildElement());
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //By defalut this first exceptin will be handled without throwing

            demo.ReadFileBuffer("ms-appx:///nonexist.txt");           
            Uri uri = new Uri("ms-appx:///nonexist.txt");
            await StorageFile.GetFileFromApplicationUriAsync(uri);             
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _mediaPlayer = new MediaPlayer();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Uri manifestUri = new Uri("http://www.largesound.com/ashborytour/sound/brobob.mp3");
            _mediaPlayer.Source = MediaSource.CreateFromUri(manifestUri);
            _mediaPlayer.Play();
            _mediaPlayer.Volume = 0;
        }
    }
}
