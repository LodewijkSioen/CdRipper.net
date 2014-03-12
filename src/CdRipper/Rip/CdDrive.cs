using System;
using System.Runtime.InteropServices;

namespace CdRipper.Rip
{
    public interface ICdDrive : IDisposable
    {
        bool IsCdInDrive();
        TableOfContents ReadTableOfContents();
        byte[] ReadSector(int startSector, int numberOfSectors);
        bool Lock();
        bool UnLock();
    }

    public class CdDrive : ICdDrive
    {
        private IntPtr _driveHandle;

        private string _driveName;

        public static ICdDrive Create(string driveName)
        {
            //Factory to force usage of interface
            return new CdDrive(driveName);
        }

        private CdDrive(string driveName)
        {
            if (Win32Functions.GetDriveType(driveName + ":\\") != Win32Functions.DriveTypes.DRIVE_CDROM)
            {
                throw new InvalidOperationException("Drive '" + driveName + "' is not an optical disc drive.");
            }

            _driveName = driveName;
            _driveHandle = CreateDriveHandle();
        }

        public bool IsCdInDrive()
        {
            uint dummy = 0;
            var check = Win32Functions.DeviceIoControl(_driveHandle, Win32Functions.IOCTL_STORAGE_CHECK_VERIFY, IntPtr.Zero, 0, IntPtr.Zero, 0, ref dummy, IntPtr.Zero);
            return check != 0;
        }

        public TableOfContents ReadTableOfContents()
        {
            var toc = new Win32Functions.CDROM_TOC();
            uint bytesRead = 0; 
            var succes = Win32Functions.DeviceIoControl(_driveHandle, Win32Functions.IOCTL_CDROM_READ_TOC, IntPtr.Zero, 0, toc, (uint)Marshal.SizeOf(toc), ref bytesRead, IntPtr.Zero) != 0;
            if (!succes)
            {
                throw new Exception("Reading the TOC failed.");
            }
            return TableOfContents.Create(toc);
        }        

        public byte[] ReadSector(int startSector, int numberOfSectors)
        {
            var rri = new Win32Functions.RAW_READ_INFO
            {
                TrackMode = Win32Functions.TRACK_MODE_TYPE.CDDA,
                SectorCount = (uint) numberOfSectors,
                DiskOffset = startSector * Constants.CB_CDROMSECTOR
            };

            uint bytesRead = 0;
            var buffer = new byte[Constants.CB_AUDIO * Constants.NSECTORS];
            var success = Win32Functions.DeviceIoControl(_driveHandle, Win32Functions.IOCTL_CDROM_RAW_READ, rri,
                (uint) Marshal.SizeOf(rri), buffer, (uint) numberOfSectors*Constants.CB_AUDIO, ref bytesRead,
                IntPtr.Zero);

            if (success != 0)
            {
                return buffer;
            }
            throw new Exception("Failed to read from disk");
        }

        private bool IsValidHandle(IntPtr handle)
        {
            return ((int) handle != -1) && ((int) handle != 0);
        }

        public bool Lock()
        {
            uint dummy = 0;
            var pmr = new Win32Functions.PREVENT_MEDIA_REMOVAL { PreventMediaRemoval = 1 };
            return Win32Functions.DeviceIoControl(_driveHandle, Win32Functions.IOCTL_STORAGE_MEDIA_REMOVAL, pmr, (uint)Marshal.SizeOf(pmr), IntPtr.Zero, 0, ref dummy, IntPtr.Zero) != 0;
        }

        public bool UnLock()
        {
            uint dummy = 0;
            var pmr = new Win32Functions.PREVENT_MEDIA_REMOVAL { PreventMediaRemoval = 0 };
            return Win32Functions.DeviceIoControl(_driveHandle, Win32Functions.IOCTL_STORAGE_MEDIA_REMOVAL, pmr, (uint)Marshal.SizeOf(pmr), IntPtr.Zero, 0, ref dummy, IntPtr.Zero) == 0;
        }

        private IntPtr CreateDriveHandle()
        {
            var handle = Win32Functions.CreateFile("\\\\.\\" + _driveName + ':', Win32Functions.GENERIC_READ,
                Win32Functions.FILE_SHARE_READ, IntPtr.Zero, Win32Functions.OPEN_EXISTING, 0, IntPtr.Zero);
            if (IsValidHandle(handle))
            {
                return handle;
            }
            throw new InvalidOperationException("Drive '" + _driveName + "' is currently opened.");
        }

        public void Dispose()
        {
            Win32Functions.CloseHandle(_driveHandle);
            _driveHandle = IntPtr.Zero;
            _driveName = null;
            GC.SuppressFinalize(this);
        }

        ~CdDrive()
        {
            Dispose();
        }
    }
}