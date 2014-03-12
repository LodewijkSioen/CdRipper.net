using System;
using CdRipper.Rip;
using NUnit.Framework;

namespace CdRipper.Tests
{
    [TestFixture]
    public class TestCorrectnessOfDummyData
    {
        [Test, Explicit]
        public void TestDummyData()
        {
            var dummyToc = DummyData.SteekJeVingerInDeLucht.GetTableOfContents();

            using (var drive = CdDrive.Create("f"))
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
        public void GenerateDummyData()
        {
            using (var nativeDrive = CdDrive.Create("f"))
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