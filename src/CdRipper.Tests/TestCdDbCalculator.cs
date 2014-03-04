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
            return FreeDbDiscIdCalculator.CalculateDiscId(toc);
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
                //TODO: -150
                new Track(1, 150-150, 14671-150),
                new Track(2, 14672-150, 27366-150),
                new Track(3, 27367-150, 45029-150),
                new Track(4, 45030-150, 60544-150),
                new Track(5, 60545-150, 76706-150),
                new Track(6, 76707-150, 103644-150),
                new Track(7, 103645-150, 116429-150),
                new Track(8, 116430-150, 137729-150),
                new Track(9, 137730-150, 156886-150),
                new Track(10, 156887-150, 171576-150),
                new Track(11, 171577-150, 185791-150),
                new Track(12, 185792-150, 208499-150)
            });
        }

        public static TableOfContents GetTableOfContentsForJuniorEuroSong2011()
        {
            return new TableOfContents(new List<Track>
            {
                new Track(1, 0, 12835),
                new Track(2, 12836, 25108),
                new Track(3, 25109, 37840),
                new Track(4, 37841, 49193),
                new Track(5, 49194, 61342),
                new Track(6, 61343, 73837),
                new Track(7, 73838, 86323),
                new Track(8, 86324, 99086),
                new Track(9, 99087, 111527),
                new Track(10, 111528, 124946),
                new Track(11, 124947, 137790),
                new Track(12, 137791, 150074),
                new Track(13, 150075, 162840),
                new Track(14, 162841, 174174),
                new Track(15, 174175, 186361),
                new Track(16, 186362, 198830),
                new Track(17, 198831, 211316),
                new Track(18, 211317, 224116),
                new Track(19, 224117, 236562),
                new Track(20, 236563, 249913)
            });
        }

        public static TableOfContents GetTableOfContentsForSteekJeVingerInDeLucht()
        {
            return new TableOfContents(new List<Track>
            {       
                //TODO: -150
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
