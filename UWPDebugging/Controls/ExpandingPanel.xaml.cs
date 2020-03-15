using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWPDebugging.Controls
{
    public sealed partial class ExpandingPanel : UserControl
    {
        public static readonly DependencyProperty PanelWidthProperty = DependencyProperty.Register(
            "PanelWidth", typeof(double), typeof(ExpandingPanel), new PropertyMetadata(240d));
        public double PanelWidth
        {
            get { return (double)GetValue(PanelWidthProperty); }
            set { SetValue(PanelWidthProperty, value); }
        }

        public static readonly DependencyProperty PanelHeightProperty = DependencyProperty.Register(
            "PanelHeight", typeof(double), typeof(ExpandingPanel), new PropertyMetadata(48d));
        public double PanelHeight
        {
            get { return (double)GetValue(PanelHeightProperty); }
            set { SetValue(PanelHeightProperty, value); }
        }

        Storyboard storyboard = new Storyboard();

        public ExpandingPanel()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) =>
            {
                storyboard.Children.Add(AnimationCreate(this, 10000, "PanelHeight", 48d, this.ActualHeight));
                storyboard.Children.Add(AnimationCreate(this, 10000, "PanelWidth", 240d, this.ActualWidth));
                storyboard.Completed += (ss, o) => { };
            };
        }

        public void StartAnimation()
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                PanelWidth = 240d;
                PanelHeight = 48d;
                this.Visibility = Visibility.Visible;
                storyboard.Begin();
            });
        }

        public void PauseAnimation()
        {
            storyboard.Pause();
        }

        public void ResumeAnimation()
        {
            storyboard.Resume();
        }

        public void Reset()
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                PanelWidth = 240d;
                PanelHeight = 48d;
                this.Visibility = Visibility.Visible;
            });
        }

        private static Timeline AnimationCreate(DependencyObject d, double duration, string property, double from, double to)
        {
            var animation = new DoubleAnimation();
            Storyboard.SetTarget(animation, d);
            Storyboard.SetTargetProperty(animation, property);
            animation.EnableDependentAnimation = true;
            animation.From = from;
            animation.To = to;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(duration));
            animation.EasingFunction = new CircleEase()
            {
                EasingMode = EasingMode.EaseInOut
            };
            return animation;
        }
    }
}
