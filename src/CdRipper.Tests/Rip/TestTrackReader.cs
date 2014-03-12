using System;
using System.Collections;
using System.Linq;
using CdRipper.Rip;
using NUnit.Framework;

namespace CdRipper.Tests.Rip
{
    [TestFixture]
    public class TestTrackReader
    {
        [Test]
        public void CheckIfCorrectSectorsAreRead()
        {
            var toc = DummyData.SteekJeVingerInDeLucht.GetTableOfContents();
            var track = new Track(1, 150, 26);
            var data = new int[track.Sectors];

            var dummyDrive = new DummyDrive(toc, (offset, numberOfSectors) =>
            {
                foreach (var index in Enumerable.Range(offset, numberOfSectors))
                {
                    data.SetValue(1, index);
                }

                return new byte[numberOfSectors];
            });

            using (var reader = new TrackReader(dummyDrive))
            {
                reader.ReadTrack(track, buffer => { }, (read, bytes) => { });
            }

            Assert.That(data, Has.All.EqualTo(1));
        }
    }

    public class DummyDrive : ICdDrive
    {
        private readonly TableOfContents _toc;
        private readonly Func<int, int, byte[]> _readSectorImp;

        public DummyDrive(TableOfContents toc, Func<int, int, byte[]> readSectorImp = null)
        {
            _toc = toc;
            _readSectorImp = readSectorImp ?? ((offset, numberOfSectors) => new byte[numberOfSectors]) ;
        }

        public void Dispose()
        {
            
        }

        public bool IsCdInDrive()
        {
            return true;
        }

        public TableOfContents ReadTableOfContents()
        {
            return _toc;
        }

        public byte[] ReadSector(int startSector, int numberOfSectors)
        {
            return _readSectorImp(startSector, numberOfSectors);
        }

        public bool Lock()
        {
            return true;
        }

        public bool UnLock()
        {
            return true;
        }
    }
}