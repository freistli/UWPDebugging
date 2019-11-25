using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
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
    public sealed partial class HighCPUPage : Page
    {
        [DllImport("kernel32.dll")]
        static extern uint GetTickCount();

        [DllImport("kernel32.dll")]
        static extern void Sleep(uint dwMilliseconds);

        CancellationTokenSource cts;

        async Task HighCPU(CancellationToken token)
        {
            int count = 200;
            double[]busytime = new double[count];
            double[] idletime = new double[count];
            double radian = 0.0;
            double interval = 100;
            for (int i = 0;i <count;i ++)
            {
                busytime[i] = interval/2 + Math.Sin(2*Math.PI*radian)* interval/2;
                idletime[i] = interval - busytime[i];
                radian += 0.01;
            }
            long elapsedTicks = 0;
            int j = 0;
            while (!token.IsCancellationRequested)
            {
                j = j % count;
                elapsedTicks = GetTickCount();
                while (GetTickCount() - elapsedTicks <= busytime[j]) ;
                Sleep((uint)idletime[j]);
                j++;
            }
        }
        public HighCPUPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)       
        {

            // Instantiate the CancellationTokenSource.
            cts = new CancellationTokenSource();
            try
            {
                //High CPU task is created in another thread
                await Task.Run(async () => await HighCPU(cts.Token));
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Manually Cancelled");
            }
            finally
            {
                cts = null;
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {

            if (cts != null)
                cts.Cancel();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Instantiate the CancellationTokenSource.
            cts = new CancellationTokenSource();
            try
            {
                //High CPU task running in the UI thread
                 await HighCPU(cts.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Manually Cancelled");
            }
            finally
            {
                cts = null;
            }
        }
    }
}
