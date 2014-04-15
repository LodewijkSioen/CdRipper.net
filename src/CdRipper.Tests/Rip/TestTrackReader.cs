using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
            var toc = DummyData.SteekJeVingerInDeLucht.TableOfContents;

            var dummyDrive = new DummyDrive(toc);
            var bytesRead = 0;

            using (var reader = new TrackReader(dummyDrive))
            {
                reader.ReadTrack(150, 25, buffer =>
                {
                    Assert.That(buffer, Has.All.EqualTo((byte)1));
                    bytesRead += buffer.Length;
                }, (read, bytes) => { }, CancellationToken.None).Wait();
            }

            Assert.That(dummyDrive.StartSectors[0], Is.EqualTo(0), "We don't want the 2s lead-in when reading from the disc");
            Assert.That(dummyDrive.StartSectors[1], Is.EqualTo(13), "We use 13 byte buffers (why?)");
            Assert.That(dummyDrive.NumberOfSectors[0], Is.EqualTo(13), "We use 13 byte buffers (why?)");
            Assert.That(dummyDrive.NumberOfSectors[1], Is.EqualTo(12), "Unless there are not enough bytes left to read");

            Assert.That(bytesRead, Is.EqualTo(25));
        }
    }

    public class DummyDrive : ICdDrive
    {
        private readonly TableOfContents _toc;

        public IList<int> StartSectors = new List<int>();
        public IList<int> NumberOfSectors = new List<int>();

        public DummyDrive(TableOfContents toc)
        {
            _toc = toc;
        }

        public void Dispose()
        {
            
        }

        public async Task<bool> IsCdInDrive()
        {
            return await Task.Run(() => true);
        }

        public async Task<TableOfContents> ReadTableOfContents()
        {
            return await Task.Run(() =>_toc);
        }

        public async Task<byte[]> ReadSector(int startSector, int numberOfSectors)
        {
            StartSectors.Add(startSector);
            NumberOfSectors.Add(numberOfSectors);

            var bytes = new byte[numberOfSectors];
            for (var i = 0; i < numberOfSectors; i++)
            {
                bytes.SetValue((byte)1, i);
            }
            return await Task.Run(() =>  bytes);
        }

        public async Task<bool> Lock()
        {
            return await Task.Run(() =>true);
        }

        public async Task<bool> UnLock()
        {
            return await Task.Run(() =>true);
        }

        public async Task<bool> Eject()
        {
            return await Task.Run(() => true);
        }
    }
}