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
using UWPDebugging.Pages;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace UWPDebugging
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DebuggingScenario_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            FrameNavigationOptions navOptions = new FrameNavigationOptions();
            navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;

            var itemContainer = sender.SelectedItem;
            Type pageType;
           
            if (itemContainer == FirstException)
            {
                pageType = typeof(FirstExceptionPage);
            }
            else if (itemContainer == SecondException)
            {
                pageType = typeof(SecondExceptionPage);
            }
            else if (itemContainer == ThirdException)
            {
                pageType = typeof(ThirdExceptionPage);
            }
            else if (itemContainer == SlownessPattern)
            {
                pageType = typeof(SlownessPatternPage);
            }
            else if (itemContainer == HighCPU)
            {
                pageType = typeof(HighCPUPage);
            }
            else if (itemContainer == HotReload)
            {
                pageType = typeof(HotReloadPage);
            }
            else if (itemContainer == HighMemroy)
            {
                pageType = typeof(HighMemoryPage);
            }
            else if (itemContainer == OOPBackgroundtask)
            {
                pageType = typeof(OOPBackgroundtaskPage);
            }
            else
                pageType = typeof(MainPage);

            ContentFrame.NavigateToType(pageType, null, navOptions);

        }
    }
}
