using System;

namespace CdRipper
{
    public class TrackReader : IDisposable
    {
        private CdDrive _drive;
        private bool _isLocked;

        public TrackReader(CdDrive drive)
        {
            _drive = drive;
            _isLocked = drive.Lock();
        }

        public void ReadTrack(Track track, Action<byte[]> onDataRead, Action<uint, uint> onProgress)
        {
            var bytes2Read = (uint)(track.EndSector - track.StartSector) * Constants.CB_AUDIO;
            var bytesRead = (uint)0;

            if (onProgress != null)
            {
                onProgress(bytesRead, bytes2Read);
            }
            for (int sector = track.StartSector; (sector < track.EndSector); sector += Constants.NSECTORS)
            {
                var sectors2Read = ((sector + Constants.NSECTORS) < track.EndSector) ? Constants.NSECTORS : (track.EndSector - sector);
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
                _drive.UnLock();
            }
            _drive = null;
        }

        ~TrackReader()      
        {
            Dispose();
        }
    }
}