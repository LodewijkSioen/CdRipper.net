using System;
using System.Linq;
using CdRipper.Rip;
using CdRipper.Tagging;
using DiscId;
using NUnit.Framework;

namespace CdRipper.Tests
{
    [TestFixture]
    public class TestFixtureDummyData
    {
        [Test, Explicit]
        public void TestDummyData()
        {
            var dummyToc = DummyData.GetTableOfContentsForSteekJeVingerInDeLucht();

            using (var drive = new CdDrive("f"))
            {
                var realToc = drive.ReadTableOfContents();

                for (var i = 0; i < realToc.Tracks.Count; i++)
                {
                    var realTrack = realToc.Tracks[i];
                    var dummyTrack = dummyToc.Tracks[i];

                    Assert.That(realTrack.TrackNumber, Is.EqualTo(dummyTrack.TrackNumber), "Tracknumber " + i);
                    Assert.That(realTrack.Offset, Is.EqualTo(dummyTrack.Offset), "Startsector " + i);
                    Assert.That(realTrack.Sectors, Is.EqualTo(dummyTrack.Sectors), "EndSector " + i);
                    Assert.That(realTrack.Length, Is.EqualTo(dummyTrack.Length), "length " + i);
                }
            }
        }

        [Test, Explicit]
        public void CompareNativeImplWithLibDiscId()
        {
            TableOfContents toc;

            using (var nativeDrive = new CdDrive("f"))
            {
                toc = nativeDrive.ReadTableOfContents();
            }
            using (var libDrive = Disc.Read("f:"))
            {
                Assert.That(toc.Tracks.Count, Is.EqualTo(libDrive.Tracks.Count()));
                Assert.That(FreeDbDiscIdCalculator.CalculateDiscId(toc), Is.EqualTo(libDrive.FreedbId));

                foreach (var track in toc.Tracks)
                {
                    var libTrack = libDrive.Tracks.Single(l => l.Number == track.TrackNumber);
                    Assert.That(track.Offset, Is.EqualTo(libTrack.Offset), "Startsector of track" + track.TrackNumber);
                    Assert.That(track.Sectors, Is.EqualTo(libTrack.Sectors));
                    Assert.That(track.Sectors, Is.EqualTo(libTrack.Sectors), "EndSector of track " + track.TrackNumber);
                }
            }

        }

        [Test, Explicit]
        public void GenerateDummyData()
        {
            using (var nativeDrive = new CdDrive("f"))
            {
                var toc = nativeDrive.ReadTableOfContents();
                foreach (var track in toc.Tracks)
                {
                    Console.WriteLine("new Track({0}, {1}, {2}),", track.TrackNumber, track.Offset, track.Sectors);
                }
            }
        }
    }
}