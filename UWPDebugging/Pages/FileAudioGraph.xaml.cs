//Sample Code is provided for the purpose of illustration only and is not intended
//to be used in a production environment. THIS SAMPLE CODE AND ANY RELATED INFORMATION
//ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, 
//INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS
//FOR A PARTICULAR PURPOSE. We grant You a nonexclusive, royalty-free right to use and
//modify the Sample Code and to reproduce and distribute the object code form of the
//Sample Code, provided that. You agree: (i) to not use Our name, logo, or trademarks 
//to market Your software product in which the Sample Code is embedded; (ii) to include
//a valid copyright notice on Your software product in which the Sample Code is embedded;
//and (iii) to indemnify, hold harmless, and defend Us and Our suppliers from and against
//any claims or lawsuits, including attorneys’ fees, that arise or result from the use or
//distribution of the Sample Code

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Audio;
using Windows.Media.Capture;
using Windows.Media.Playback;
using Windows.Media.Render;
using Windows.Storage;
using Windows.Storage.Pickers;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPDebugging.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FileAudioGraph : Page
    {
        private AudioGraph graph;
        private AudioFileInputNode fileInput;
        private AudioDeviceOutputNode deviceOutput;
        AudioStateMonitor gameChatAudioStateMonitor;

        MediaPlayer Player ;

        MediaPlaybackList PlaybackList
        {
            get { return Player.Source as MediaPlaybackList; }
            set { Player.Source = value; }
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await CreateAudioGraph(); 
            string deviceId = Windows.Media.Devices.MediaDevice.GetDefaultAudioCaptureId(Windows.Media.Devices.AudioDeviceRole.Communications);
            gameChatAudioStateMonitor = AudioStateMonitor.CreateForCaptureMonitoringWithCategoryAndDeviceId(MediaCategory.Other, deviceId);
            gameChatAudioStateMonitor.SoundLevelChanged += OnSoundLevelChanged;

            LogPath.Text = Logging.LoggingPath;
         }

        private void OnSoundLevelChanged(AudioStateMonitor sender, object args)
        {
            switch (sender.SoundLevel)
            {
                case SoundLevel.Full:
                    Logging.SingleInstance.LogMessage("Microphone Volume is full");
                    break;
                case SoundLevel.Muted:
                    Logging.SingleInstance.LogMessage("Microphone Volume is muted");
                    break;
                case SoundLevel.Low:
                    // Audio capture should never be "ducked", only muted or full volume.
                    Logging.SingleInstance.LogMessage("Microphone Volume is low");
                    break;
            }
        }

        public FileAudioGraph()
        {
            this.InitializeComponent();

            if (Player == null)
            {
                Logging.SingleInstance.LogMessage("New Media Player");
                Player = new MediaPlayer();
                PlaybackList = new MediaPlaybackList();
            }

            ((UWPDebugging.App)App.Current).StartAnimator += OnStartAnimatorMessage;


        }

        private void OnStartAnimatorMessage(object sender, object e)
        {
            Logging.SingleInstance.LogMessage("AppService triggered start Animator message in Audio Graph page");
            expandingPanel.StartAnimation();
            compositionPanel.StartAnimation();
        }

        private async Task CreateAudioGraph()
        {
            // Create an AudioGraph with default settings
            AudioGraphSettings settings = new AudioGraphSettings(AudioRenderCategory.Media);
            CreateAudioGraphResult result = await AudioGraph.CreateAsync(settings);

            if (result.Status != AudioGraphCreationStatus.Success)
            {
                // Cannot create graph
                Logging.SingleInstance.LogMessage("AudioGraph Creation Error because " + result.Status);
                return;
            }

            graph = result.Graph;

            // Create a device output node
            CreateAudioDeviceOutputNodeResult deviceOutputNodeResult = await graph.CreateDeviceOutputNodeAsync();

            if (deviceOutputNodeResult.Status != AudioDeviceNodeCreationStatus.Success)
            {
                // Cannot create device output node
                Logging.SingleInstance.LogMessage(String.Format("Device Output unavailable because {0}", deviceOutputNodeResult.Status.ToString()));
                return;
            }

            deviceOutput = deviceOutputNodeResult.DeviceOutputNode;
            Logging.SingleInstance.LogMessage("Device Output Node successfully created");
            
        }
        private async void PlayAudioGraph_Click(object sender, RoutedEventArgs e)
        {
            // If another file is already loaded into the FileInput node
            if (fileInput != null)
            {
                // Release the file and dispose the contents of the node
                fileInput.Dispose();
                
            }

            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            filePicker.FileTypeFilter.Add(".mp3");
            filePicker.FileTypeFilter.Add(".wav");
            filePicker.FileTypeFilter.Add(".wma");
            filePicker.FileTypeFilter.Add(".m4a");
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            StorageFile file = await filePicker.PickSingleFileAsync();

            // File can be null if cancel is hit in the file picker
            if (file == null)
            {
                return;
            }

            CreateAudioFileInputNodeResult fileInputResult = await graph.CreateFileInputNodeAsync(file);
            if (AudioFileNodeCreationStatus.Success != fileInputResult.Status)
            {
                // Cannot read input file
                Logging.SingleInstance.LogMessage("Cannot read input file because  " + fileInputResult.Status);
                return;
            }

            fileInput = fileInputResult.FileInputNode;

            if (fileInput.Duration <= TimeSpan.FromSeconds(3))
            {
                // Imported file is too short
                Logging.SingleInstance.LogMessage("Please pick an audio file which is longer than 3 seconds " + fileInputResult.Status);
                fileInput.Dispose();
                fileInput = null;
                  return;
            }

            fileInput.AddOutgoingConnection(deviceOutput);
        

            // Trim the file: set the start time to 3 seconds from the beginning
            // fileInput.EndTime can be used to trim from the end of file
            fileInput.StartTime = TimeSpan.FromSeconds(3);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            graph.Start();
            expandingPanel.StartAnimation();
            compositionPanel.StartAnimation();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if(fileInput!=null)
                fileInput.LoopCount = 0;
            graph.Stop();
        }

        private void Loop_Click(object sender, RoutedEventArgs e)
        {
            if (fileInput != null)
            {
                // If turning on looping, make sure the file hasn't finished playback yet
                if (fileInput.Position >= fileInput.Duration)
                {
                    // If finished playback, seek back to the start time we set
                    fileInput.Seek(fileInput.StartTime.Value);
                }
                fileInput.LoopCount = null; // infinite looping
            }
        }
    }
}
