using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPDebugging.Classes
{
    public class Recording
    {
        public string ArtistName { get; set; }
        public string CompositionName { get; set; }
        public DateTime ReleaseDateTime { get; set; }
        public Recording()
        {
            this.ArtistName = "Wolfgang Amadeus Mozart";
            this.CompositionName = "Andante in C for Piano";
            this.ReleaseDateTime = new DateTime(1761, 1, 1);
        }
        public string OneLineSummary
        {
            get
            {
                return $"{this.CompositionName} by {this.ArtistName}, released: "
                    + this.ReleaseDateTime.ToString("d");
            }
        }
    }
    public class RecordingViewModel
    {
        private Recording defaultRecording = new Recording();
        public Recording DefaultRecording { get { return this.defaultRecording; } }

        private ObservableCollection<Recording> recordings01 = new ObservableCollection<Recording>();
        private ObservableCollection<Recording> recordings02 = new ObservableCollection<Recording>();
        public ObservableCollection<Recording> Recordings01 { get { return this.recordings01; } }
        public ObservableCollection<Recording> Recordings02 { get { return this.recordings02; } }

        public RecordingViewModel()
        {
            this.recordings01.Add(new Recording()
            {
                ArtistName = "Johann Sebastian Bach",
                CompositionName = "Mass in B minor",
                ReleaseDateTime = new DateTime(1748, 7, 8)
            });
            this.recordings01.Add(new Recording()
            {
                ArtistName = "Ludwig van Beethoven",
                CompositionName = "Third Symphony",
                ReleaseDateTime = new DateTime(1805, 2, 11)
            });
            this.recordings01.Add(new Recording()
            {
                ArtistName = "George Frideric Handel",
                CompositionName = "Serse",
                ReleaseDateTime = new DateTime(1737, 12, 3)
            });

            for (int i = 0; i < 10; i++)
            {
                this.recordings02.Add(new Recording()
                {
                });
            }
        }
    }
}
