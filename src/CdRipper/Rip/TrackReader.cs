using System;

namespace CdRipper.Rip
{
    public delegate void OnReadingTrack(byte[] buffer);
    public delegate void OnTrackReadingProgress(uint bytesRead, uint totalBytes);

    public class TrackReader : IDisposable
    {
        private ICdDrive _drive;
        private bool _isLocked;
       
        public TrackReader(ICdDrive drive)
        {
            _drive = drive;
            _isLocked = drive.Lock();
        }

        public void ReadTrack(Track track, OnReadingTrack onDataRead, OnTrackReadingProgress onProgress)
        {
            ReadTrack(track.Offset, track.Sectors, onDataRead, onProgress);
        }

        public void ReadTrack(int offset, int sectors, OnReadingTrack onDataRead, OnTrackReadingProgress onProgress)
        {
            var bytes2Read = (uint)(sectors) * Constants.CB_AUDIO;
            var bytesRead = (uint)0;

            if (onProgress != null)
            {
                onProgress(bytesRead, bytes2Read);
            }

            for (int sector = 0; (sector < sectors); sector += Constants.NSECTORS)
            {
                var sectors2Read = ((sector + Constants.NSECTORS) < sectors) ? Constants.NSECTORS : (sectors - sector);
                var buffer = _drive.ReadSector(offset - 150 + sector, sectors2Read);//No 2 second lead in for reading the track
                
                onDataRead(buffer);
                bytesRead += (uint)(Constants.CB_AUDIO * sectors2Read);

                if (onProgress != null)
                {
                    onProgress(bytesRead, bytes2Read);
                }
            }
        }

        public void Dispose()
        {
            if (_isLocked)
            {
                _isLocked = !_drive.UnLock();
            }
            _drive = null;
        }

        ~TrackReader()      
        {
            Dispose();
        }
    }
}