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
            yield return new TestCaseData(DummyData.JuniorEuroSong2011.TableOfContents)
                .SetName("Junior Eurosong 2011")
                .Returns(DummyData.JuniorEuroSong2011.MusicBrainzDiscId);

            yield return new TestCaseData(DummyData.SteekJeVingerInDeLucht.TableOfContents)
                .SetName("Steek je vinger in de lucht")
                .Returns(DummyData.SteekJeVingerInDeLucht.MusicBrainzDiscId);

            yield return new TestCaseData(DummyData.MuchAgainstEveryonesAdvice.TableOfContents)
                .SetName("Much Against Everyone's Advice")
                .Returns(DummyData.MuchAgainstEveryonesAdvice.MusicBrainzDiscId);
        }

        [TestCaseSource("GetTestCdDb"), Explicit("Still Failing")]
        public string TestCalculationOfDiscIdWithCdDb(TableOfContents toc)
        {
            return MusicBrainzDiscIdCalculator.CalculateDiscId(toc);
        }

        public IEnumerable<TestCaseData> GetTestCdDb()
        {
            yield return new TestCaseData(DummyData.JuniorEuroSong2011.TableOfContents)
                .SetName("Junior Eurosong 2011")
                .Returns(DummyData.JuniorEuroSong2011.CdDbDiscId);

            yield return new TestCaseData(DummyData.SteekJeVingerInDeLucht.TableOfContents)
                .SetName("Steek je vinger in de lucht")
                .Returns(DummyData.SteekJeVingerInDeLucht.CdDbDiscId);
        }
    }
}
