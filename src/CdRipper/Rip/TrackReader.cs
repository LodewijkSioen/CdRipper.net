using System;

namespace CdRipper.Rip
{
    public delegate void OnReadingTrack(byte[] buffer);
    public delegate void OnTrackReadingProgress(uint bytesRead, uint totalBytes);

    public class TrackReader : IDisposable
    {
        private CdDrive _drive;
        private bool _isLocked;

        public TrackReader(string driveName)
        {
            _drive = new CdDrive(driveName);
            _isLocked = _drive.Lock();
        }

        public TrackReader(CdDrive drive)
        {
            _drive = drive;
            _isLocked = drive.Lock();
        }

        public void ReadTrack(Track track, OnReadingTrack onDataRead, OnTrackReadingProgress onProgress)
        {
            ReadTrack(track.StartSector, track.EndSector, onDataRead, onProgress);
        }

        public void ReadTrack(int startSector, int endSector, OnReadingTrack onDataRead, OnTrackReadingProgress onProgress)
        {
            var bytes2Read = (uint)(endSector - startSector) * Constants.CB_AUDIO;
            var bytesRead = (uint)0;

            if (onProgress != null)
            {
                onProgress(bytesRead, bytes2Read);
            }

            for (int sector = startSector; (sector < endSector); sector += Constants.NSECTORS)
            {
                var sectors2Read = ((sector + Constants.NSECTORS) < endSector) ? Constants.NSECTORS : (endSector - sector);
                var buffer = _drive.ReadSector(sector, sectors2Read);
                
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