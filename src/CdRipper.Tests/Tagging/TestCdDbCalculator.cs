using System.Collections.Generic;
using CdRipper.Rip;
using CdRipper.Tagging;
using NUnit.Framework;

namespace CdRipper.Tests.Tagging
{
    [TestFixture]
    public class TestCdDbCalculator
    {
        [TestCaseSource("GetTestMusicBrainz")]
        public string TestCalculationOfDiscIdWithMusicBrainz(TableOfContents toc)
        {
            return MusicBrainzDiscIdCalculator.CalculateDiscId(toc);
        }

        public IEnumerable<TestCaseData> GetTestMusicBrainz()
        {
            yield return new TestCaseData(DummyData.GetTableOfContentsForJuniorEuroSong2011())
                .SetName("Junior Eurosong 2011")
                .Returns("WMnoPf6.FF0PALnVth5jCRT1LxI-");

            yield return new TestCaseData(DummyData.GetTableOfContentsForSteekJeVingerInDeLucht())
                .SetName("Steek je vinger in de lucht")
                .Returns(DummyData.MusicBrainzDiscIdSteekJeVingerInDeLucht);
        }

        [TestCaseSource("GetTestCdDb"), Explicit("Still Failing")]
        public string TestCalculationOfDiscIdWithCdDb(TableOfContents toc)
        {
            return MusicBrainzDiscIdCalculator.CalculateDiscId(toc);
        }

        public IEnumerable<TestCaseData> GetTestCdDb()
        {
            yield return new TestCaseData(DummyData.GetTableOfContentsForJuniorEuroSong2011())
                .SetName("Junior Eurosong 2011")
                .Returns("290d0414");

            yield return new TestCaseData(DummyData.GetTableOfContentsForSteekJeVingerInDeLucht())
                .SetName("Steek je vinger in de lucht")
                .Returns("c708b90e");
        }
    }
}
