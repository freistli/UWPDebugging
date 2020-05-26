using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System.Diagnostics;

namespace UWPDebugging
{
    class Logging
    {
        static int instanceCounter = 0;
        private static Logging singleInstance = null;
        private static readonly object lockObject = new object();
        public static string LoggingPath;
        public Logger logger;
        private Logging()
        {
            instanceCounter++;

            Debug.WriteLine("Instances created " + instanceCounter);
        }
        
        public static Logging SingleInstance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (lockObject)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new Logging();

                            if (singleInstance.logger == null)
                            {
                                StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

                                StorageFile storageFile = storageFolder.CreateFileAsync("temp.txt", CreationCollisionOption.ReplaceExisting).AsTask().Result;

                                string dpath = Path.GetDirectoryName(storageFile.Path);

                                LoggingPath = dpath + "\\log.txt"; 
                                singleInstance.logger = new LoggerConfiguration()
                                    .WriteTo.File(LoggingPath)
                                    .CreateLogger();
                            }
                        }

                    }
                }
                return singleInstance;
            }
        }
        public void LogMessage(string message)
        {
            var processId = ProcessDiagnosticInfo.GetForCurrentProcess().ProcessId;
            singleInstance.logger.Information(processId + " " + message);

            Debug.WriteLine("Message " + message);
        }
    }

}

