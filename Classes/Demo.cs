using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.UI.Composition;
using Windows.UI.Composition.Scenes;
using Windows.Storage.Streams;
using Windows.Media.Devices;
using System.Diagnostics;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml;
using System.Numerics;

namespace UWPDebugging.Classes
{
    class Demo
    {
        public SceneLoaderComponent.SceneLoader loader;
        public SceneVisual sceneVisual;
        public Compositor compositor;
        public ContainerVisual hostVisual;

       
        public async Task<Windows.Storage.Streams.IBuffer> ReadFileBuffer(string path)
        {            
            Uri uri = new Uri(path);
            StorageFile sf = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var buffer = await FileIO.ReadBufferAsync(sf);
            return buffer;           
        }

        public async Task CrossThreads()
        {
            await CrossThread1().ConfigureAwait(false);
            await Task.Delay(2000);
        }
        public async Task CrossThread1()
        {
            await CrossThread2().ConfigureAwait(false);
            await Task.Delay(2000);
        }
        public async Task CrossThread2()
        {
            await CrossThread3().ConfigureAwait(false);
            await Task.Delay(2000);
        }
        public async Task CrossThread3()
        {
            Compositor compositor = null;

            try
            {
                Uri uri = new Uri("ms-appx:///assets/Telescope.gltf");
                StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
                var buffer = await FileIO.ReadBufferAsync(storageFile);

                //Demo::3 back to UI thread again can cause hang
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                {
                    compositor = new Compositor();
                    var sceneNode = loader.Load(buffer, compositor);
                }));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
               if(compositor!=null)
                  compositor.Dispose();
            }
            await Task.Delay(2000);
        }

        public async Task CrossThread4(UIElement mc)
        {

            try
            {
                Uri uri = new Uri("ms-appx:///assets/Telescope.gltf");
                StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);


                compositor = Window.Current.Compositor;

                IBuffer buffer = await FileIO.ReadBufferAsync(storageFile);
               
                loader = new SceneLoaderComponent.SceneLoader();
                var sceneNode = loader.Load(buffer, compositor);
                
                
                hostVisual = compositor.CreateContainerVisual();
                hostVisual.RelativeSizeAdjustment = Vector2.One;
                hostVisual.Offset = new Vector3(0, 50, 0);
                ElementCompositionPreview.SetElementChildVisual(mc, hostVisual);
                
                sceneVisual = SceneVisual.Create(compositor);
                sceneVisual.Root = SceneNode.Create(compositor);
                                
                sceneVisual.Root.Children.Add(sceneNode);

               
                var rotationAnimation = compositor.CreateScalarKeyFrameAnimation();
                rotationAnimation.InsertKeyFrame(1.0f, 360.0f, compositor.CreateLinearEasingFunction());
                rotationAnimation.Duration = TimeSpan.FromSeconds(16);
                rotationAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

                sceneVisual.Root.Transform.RotationAxis = new Vector3(0.0f, 1.0f, 0.2f);
                sceneVisual.Root.Transform.StartAnimation("RotationAngleInDegrees", rotationAnimation);

                hostVisual.Children.InsertAtTop(sceneVisual);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                                 
            }
           
        }
    }
}
