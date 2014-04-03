using System;
using CdRipper.Rip;
using CdRipper.Tagging;
using NUnit.Framework;

namespace CdRipper.Tests
{
    [TestFixture]
    public class TestCorrectnessOfDummyData
    {
        [Test, Explicit]
        public void TestDummyData()
        {
            var dummyToc = DummyData.MuchAgainstEveryonesAdvice.TableOfContents;

            using (var drive = CdDrive.Create("f"))
            {
                var realToc = drive.ReadTableOfContents().Result;

                for (var i = 0; i < realToc.Tracks.Count; i++)
                {
                    var realTrack = realToc.Tracks[i];
                    var dummyTrack = dummyToc.Tracks[i];

                    Assert.That(realTrack.TrackNumber, Is.EqualTo(dummyTrack.TrackNumber), "Tracknumber");
                    Assert.That(realTrack.Offset, Is.EqualTo(dummyTrack.Offset), "Offset of track " + realTrack.TrackNumber);
                    Assert.That(realTrack.Sectors, Is.EqualTo(dummyTrack.Sectors), "Sectors of track " + realTrack.TrackNumber);
                    Assert.That(realTrack.Length, Is.EqualTo(dummyTrack.Length), "Length of track " + realTrack.TrackNumber);
                }
            }
        }

        [Test, Explicit]
        public void GenerateDummyData()
        {
            using (var nativeDrive = CdDrive.Create("f"))
            {
                var toc = nativeDrive.ReadTableOfContents().Result;
                foreach (var track in toc.Tracks)
                {
                    Console.WriteLine("new Track({0}, {1}, {2}),", track.TrackNumber, track.Offset, track.Sectors);
                }
                Console.WriteLine("MusicBrainzDiscId= " + MusicBrainzDiscIdCalculator.CalculateDiscId(toc));
            }
        }
    }
}