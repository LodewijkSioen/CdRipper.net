using System;

namespace CdRipper
{
    public class Track
    {   
        public Track(int trackNumber, int startSector, int endSector)
        {
            TrackNumber = trackNumber;
            StartSector = startSector;
            EndSector = endSector;
        }

        public int TrackNumber { get; private set; }
        public int StartSector { get; private set; }
        public int EndSector { get; private set; }

        public TimeSpan Length
        {
            get { return TimeSpan.FromSeconds(Math.Round((EndSector/75d) - (StartSector/75d))); }
        }
    }
}