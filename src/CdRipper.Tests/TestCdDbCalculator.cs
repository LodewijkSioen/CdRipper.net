using System;
using System.Collections.Generic;
using NUnit.Framework;
using CdRipper.CdDb;
using CdRipper.Rip;

namespace CdRipper.Tests
{
    [TestFixture]
    public class TestCdDbCalculator
    {
        [TestCaseSource("GetTestCds")]
        public string TestCalculationOfDiscId(TableOfContents toc)
        {
            return DiscIdCalculator.CalculateDiscId(toc);
        }

        public IEnumerable<TestCaseData> GetTestCds()
        {
            yield return new TestCaseData(DummyData.GetTableOfContentsForJuniorEuroSong2011())
                .SetName("Junior Eurosong 2011")
                .Returns("290D0414");

            yield return new TestCaseData(DummyData.GeTableOfContentsForPearlJamVs())
                .SetName("Pearl Jam Vs")
                .Returns("970ADA0C");

            yield return new TestCaseData(DummyData.GetTableOfContentsForSteekJeVingerInDeLucht())
                .SetName("Steek je vinger in de lucht")
                .Returns("C708B90E");
        }
        
        [Test, Explicit]
        public void TestDummyData()
        {
            var dummyToc = DummyData.GetTableOfContentsForSteekJeVingerInDeLucht();

            using (var drive = new CdDrive("e"))
            {
                var realToc = drive.ReadTableOfContents();

                for (var i = 0; i < realToc.Tracks.Count; i++)
                {
                    var realTrack = realToc.Tracks[i];
                    var dummyTrack = dummyToc.Tracks[i];

                    Assert.That(realTrack.TrackNumber, Is.EqualTo(dummyTrack.TrackNumber), "Tracknumber");
                    Assert.That(realTrack.StartSector, Is.EqualTo(dummyTrack.StartSector), "Startsector");
                    Assert.That(realTrack.EndSector, Is.EqualTo(dummyTrack.EndSector), "EndSector"); 
                    Assert.That(realTrack.Length, Is.EqualTo(dummyTrack.Length), "length");
                }
            }
        }
    }

    public static class DummyData
    {
        public static TableOfContents GeTableOfContentsForPearlJamVs()
        {
            return new TableOfContents(new List<Track>
            {
                new Track(1, 150, 14671),
                new Track(2, 14672, 27366),
                new Track(3, 27367, 45029),
                new Track(4, 45030, 60544),
                new Track(5, 60545, 76706),
                new Track(6, 76707, 103644),
                new Track(7, 103645, 116429),
                new Track(8, 116430, 137729),
                new Track(9, 137730, 156886),
                new Track(10, 156887, 171576),
                new Track(11, 171577, 185791),
                new Track(12, 185792, 208499)
            });
        }

        public static TableOfContents GetTableOfContentsForJuniorEuroSong2011()
        {
            return new TableOfContents(new List<Track>
            {
                new Track(1, 0+150, 12835+150),
                new Track(2, 12836+150, 25108+150),
                new Track(3, 25109+150, 37840+150),
                new Track(4, 37841+150, 49193+150),
                new Track(5, 49194+150, 61342+150),
                new Track(6, 61343+150, 73837+150),
                new Track(7, 73838+150, 86323+150),
                new Track(8, 86324+150, 99086+150),
                new Track(9, 99087+150, 111527+150),
                new Track(10, 111528+150, 124946+150),
                new Track(11, 124947+150, 137790+150),
                new Track(12, 137791+150, 150074+150),
                new Track(13, 150075+150, 162840+150),
                new Track(14, 162841+150, 174174+150),
                new Track(15, 174175+150, 186361+150),
                new Track(16, 186362+150, 198830+150),
                new Track(17, 198831+150, 211316+150),
                new Track(18, 211317+150, 224116+150),
                new Track(19, 224117+150, 236562+150),
                new Track(20, 236563+150, 249913+150)
            });
        }

        public static TableOfContents GetTableOfContentsForSteekJeVingerInDeLucht()
        {
            return new TableOfContents(new List<Track>
            {       
                new Track(1, 150, 12920),
                new Track(2, 12921, 24318),
                new Track(3, 24319, 34861),
                new Track(4, 34862, 46591),
                new Track(5, 46592, 59088),
                new Track(6, 59089, 74936),
                new Track(7, 74937, 85787),
                new Track(8, 85788, 97245),
                new Track(9, 97246, 106208),
                new Track(10, 106209, 123154),
                new Track(11, 123155, 139097),
                new Track(12, 139098, 147433),
                new Track(13, 147434, 157121),
                new Track(14, 157122, 167627),
            });
        }

    }
}
