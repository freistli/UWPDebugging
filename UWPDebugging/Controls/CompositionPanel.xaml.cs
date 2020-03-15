using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWPDebugging.Controls
{
    public sealed partial class CompositionPanel : UserControl
    {
        Compositor _compositor;
        ContainerVisual _root;
        Visual _target;

        public CompositionPanel()
        {
            this.InitializeComponent();
            CreateComposition();
        }

        public void StartAnimation()
        {
            Logging.SingleInstance.LogMessage("AppService triggered start Animator message in CompansitionPanel Control");
 
            if (_target != null)
                AnimatingKeyFrameVisual(_target);
        }
        void CreateComposition()
        {
            _compositor = Window.Current.Compositor;

            _root = _compositor.CreateContainerVisual();
            _root.RelativeSizeAdjustment = Vector2.One;
            _root.Offset = new Vector3(0, 0, 0);

            Windows.UI.Xaml.Shapes.Rectangle modelhost = new Windows.UI.Xaml.Shapes.Rectangle();
         
            modelhost.Width = modelhost.Height = myStack.Width;
            myStack.Children.Add(modelhost);

            ElementCompositionPreview.SetElementChildVisual(modelhost, _root);

            if (_target == null)
            {
                _target = CreateRectangleElement(new Vector2(240, 48));
                _root.Children.InsertAtTop(_target);
            }
        }

        void AnimatingKeyFrameVisual(Visual targetVisual)
        {
            var func = _compositor.CreateLinearEasingFunction();
         
            var animation = _compositor.CreateVector3KeyFrameAnimation();
            animation.InsertKeyFrame(0.5f, new Vector3((float)this.ActualWidth/240f,(float)this.ActualHeight/48f,1f),func);
            animation.InsertKeyFrame(1.0f, new Vector3(1.0f, 1.0f, 1.0f), func);
            
            animation.Duration = TimeSpan.FromMilliseconds(10000);
            targetVisual.CenterPoint = new Vector3(targetVisual.Size.X/2,48f, 0);
            targetVisual.StartAnimation("Scale", animation);
            targetVisual.RotationAxis = Vector3.UnitZ;
            //targetVisual.RotationAngleInDegrees = -180f;
        }
         

        private Visual CreateRectangleElement(Vector2 size)
        {
            var element = _compositor.CreateContainerVisual();
            element.Size = new Vector2(0.0f, 0.0f);
            element.Offset = new Vector3(0.0f, 0.0f, 0.0f);

            // Create radial gradient brush.
            CompositionRadialGradientBrush RGBrush = _compositor.CreateRadialGradientBrush();

            // Create the color stops by defining the offset and color.
            CompositionColorGradientStop ColorStop1 = _compositor.CreateColorGradientStop();
            ColorStop1.Offset = 0;
            ColorStop1.Color = Colors.Blue;
            CompositionColorGradientStop ColorStop2 = _compositor.CreateColorGradientStop();
            ColorStop2.Offset = 0.75f;
            ColorStop2.Color = Colors.LightBlue;
            CompositionColorGradientStop ColorStop3 = _compositor.CreateColorGradientStop();
            ColorStop3.Offset = 1.5f;
            ColorStop3.Color = Colors.Navy;

            RGBrush.ColorStops.Add(ColorStop1);
            RGBrush.ColorStops.Add(ColorStop2);
            RGBrush.ColorStops.Add(ColorStop3);

            // Set up animation for ColorStop1's color.
            var colorAnimation = _compositor.CreateColorKeyFrameAnimation();
            colorAnimation.InsertKeyFrame(0.0f, Colors.Blue);
            colorAnimation.InsertKeyFrame(0.25f, Colors.LightBlue);
            colorAnimation.InsertKeyFrame(0.5f, Colors.Navy);
            colorAnimation.InsertKeyFrame(0.75f, Colors.LightBlue);
            colorAnimation.InsertKeyFrame(1.0f, Colors.Blue);
            colorAnimation.Duration = TimeSpan.FromSeconds(8);
            colorAnimation.IterationBehavior = AnimationIterationBehavior.Forever;
            ColorStop1.StartAnimation("Color", colorAnimation);
             
            //Create Shadow
            var visual0 = _compositor.CreateSpriteVisual();
  
            visual0.Size = new Vector2(size.X+2, size.Y+2);
            //Create drop shadow
            DropShadow shadow = _compositor.CreateDropShadow();
            shadow.BlurRadius = 5;
            shadow.Offset = new Vector3(0, 0, 0);
            shadow.Color = Colors.DarkGray;
                        //Associate shadow to visual
            visual0.Shadow = shadow;
            element.Children.InsertAtBottom(visual0);
          

            //Create Rounded Rectangle
            var roundedRect = _compositor.CreateRoundedRectangleGeometry();
            roundedRect.Size = new Vector2(size.X, size.Y);
            roundedRect.CornerRadius = new Vector2(7,7);

            var rectShape = _compositor.CreateSpriteShape(roundedRect);
            rectShape.FillBrush = RGBrush;
            rectShape.Offset = new Vector2(0f, 0f);

            var visual = _compositor.CreateShapeVisual();
            visual.Size = new Vector2(size.X, size.Y);
            visual.Shapes.Add(rectShape);
            element.Children.InsertAtTop(visual);            

            element.Opacity = 0.8f;
            element.Size = visual.Size;
            return element;

        }
    }
}
