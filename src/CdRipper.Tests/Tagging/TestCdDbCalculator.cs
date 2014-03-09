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
            yield return new TestCaseData(DummyData.JuniorEuroSong2011.GetTableOfContents())
                .SetName("Junior Eurosong 2011")
                .Returns(DummyData.JuniorEuroSong2011.MusicBrainzDiscId);

            yield return new TestCaseData(DummyData.SteekJeVingerInDeLucht.GetTableOfContents())
                .SetName("Steek je vinger in de lucht")
                .Returns(DummyData.SteekJeVingerInDeLucht.MusicBrainzDiscId);
        }

        [TestCaseSource("GetTestCdDb"), Explicit("Still Failing")]
        public string TestCalculationOfDiscIdWithCdDb(TableOfContents toc)
        {
            return MusicBrainzDiscIdCalculator.CalculateDiscId(toc);
        }

        public IEnumerable<TestCaseData> GetTestCdDb()
        {
            yield return new TestCaseData(DummyData.JuniorEuroSong2011.GetTableOfContents())
                .SetName("Junior Eurosong 2011")
                .Returns(DummyData.JuniorEuroSong2011.CdDcDiscId);

            yield return new TestCaseData(DummyData.SteekJeVingerInDeLucht.GetTableOfContents())
                .SetName("Steek je vinger in de lucht")
                .Returns(DummyData.SteekJeVingerInDeLucht.CdDbDiscId);
        }
    }
}
