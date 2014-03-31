using System.Collections.Generic;
using System.Linq;
using CdRipper.Tagging;
using NUnit.Framework;

namespace CdRipper.Tests.Tagging
{
    [TestFixture]
    public class TestTagRetrieval
    {
        [Test]
        public void TestGettingTheDiscTags()
        {
            var tagSource = new MusicBrainzTagSource(new MockMusicBrainzApi());

            var discTags = tagSource.GetTags(DummyData.SteekJeVingerInDeLucht.MusicBrainzDiscId).ToList();

            Assert.That(discTags, Is.Not.Null);
            Assert.That(discTags.Count(), Is.EqualTo(1));
            Assert.That(discTags.First().AlbumArtist, Is.EqualTo("Jan De Smet"));
            Assert.That(discTags.First().AlbumTitle, Is.EqualTo("Steek Je vinger in de lucht"));
            Assert.That(discTags.First().Year, Is.EqualTo("2005"));
            Assert.That(discTags.First().NumberOfTracks, Is.EqualTo(14));
            Assert.That(discTags.First().Tracks.Count(), Is.EqualTo(14));
            Assert.That(discTags.First().Tracks.ElementAt(0).Title, Is.EqualTo("Alles Goed"));
            Assert.That(discTags.First().Tracks.ElementAt(0).Artist, Is.EqualTo("Jan De Smet"));
            Assert.That(discTags.First().Tracks.ElementAt(0).TrackNumber, Is.EqualTo(1));
            Assert.That(discTags.First().Tracks.ElementAt(13).Title, Is.EqualTo("Steek Je Vinger in de Lucht"));
            Assert.That(discTags.First().Tracks.ElementAt(13).Artist, Is.EqualTo("Jan De Smet"));
            Assert.That(discTags.First().Tracks.ElementAt(13).TrackNumber, Is.EqualTo(14));
        }

        [Test]
        public void TestGettingTheDiscTagsForACdStub()
        {
            var tagSource = new MusicBrainzTagSource(new MockMusicBrainzApi());

            var discTags = tagSource.GetTags(DummyData.AppelsEten.MusicBrainzDiscId).ToList();

            Assert.That(discTags, Is.Not.Null);
            Assert.That(discTags.Count(), Is.EqualTo(1));

            Assert.That(discTags.First().AlbumArtist, Is.EqualTo("Ketnet Band"));
            Assert.That(discTags.First().AlbumTitle, Is.EqualTo("Appels Eten"));
            Assert.That(discTags.First().Year, Is.Null);
            Assert.That(discTags.First().NumberOfTracks, Is.EqualTo(3));
            Assert.That(discTags.First().Tracks.Count(), Is.EqualTo(3));
            Assert.That(discTags.First().Tracks.ElementAt(0).Title, Is.EqualTo("Appels Eten q2"));
            Assert.That(discTags.First().Tracks.ElementAt(0).Artist, Is.Null);
            Assert.That(discTags.First().Tracks.ElementAt(0).TrackNumber, Is.EqualTo(1));
            Assert.That(discTags.First().Tracks.ElementAt(1).Title, Is.EqualTo("Appels Eten Instrumental q2"));
            Assert.That(discTags.First().Tracks.ElementAt(1).Artist, Is.Null);
            Assert.That(discTags.First().Tracks.ElementAt(1).TrackNumber, Is.EqualTo(2));
            Assert.That(discTags.First().Tracks.ElementAt(2).Title, Is.EqualTo("Appels Eten PB q2"));
            Assert.That(discTags.First().Tracks.ElementAt(2).Artist, Is.Null);
            Assert.That(discTags.First().Tracks.ElementAt(2).TrackNumber, Is.EqualTo(3));
        }

        [Test]
        public void TestGettingAnUnknownDiscId()
        {
            var tagSource = new MusicBrainzTagSource(new MockMusicBrainzApi());

            var discTags = tagSource.GetTags("NotFound").ToList();

            Assert.That(discTags.Any(), Is.False);
        }

        [Test, Explicit]
        public void TestDummyData()
        {
            var dummyCd = DummyData.AppelsEten;

            var api = new MusicBrainzApi("http://musicbrainz.org/");
            var mockedApi = new MockMusicBrainzApi();

            Assert.That(api.GetReleasesByDiscId(dummyCd.MusicBrainzDiscId), Is.EqualTo(mockedApi.GetReleasesByDiscId(dummyCd.MusicBrainzDiscId)));
            if (dummyCd.MusicBrainzReleaseId != null)
            {
                Assert.That(api.GetRelease(dummyCd.MusicBrainzReleaseId), Is.EqualTo(mockedApi.GetRelease(dummyCd.MusicBrainzReleaseId)));
            }
        }
    }

    public class MockMusicBrainzApi : IIMusicBrainzApi
    {
        private readonly Dictionary<string, string> _releasesForDiscId;
        private readonly Dictionary<string, string> _releases;

        public MockMusicBrainzApi()
        {
            _releasesForDiscId = new Dictionary<string, string>
            {
                {DummyData.SteekJeVingerInDeLucht.MusicBrainzDiscId, DummyData.SteekJeVingerInDeLucht.GetReleaseByDiscIdResponse},
                {DummyData.AppelsEten.MusicBrainzDiscId, DummyData.AppelsEten.GetReleaseByDiscIdResponse}
            };
            _releases = new Dictionary<string, string>
            {
                {DummyData.SteekJeVingerInDeLucht.MusicBrainzReleaseId, DummyData.SteekJeVingerInDeLucht.GetReleaseResponse}
            };
        }

        public MusicBrainzResponse GetReleasesByDiscId(string discId)
        {
            return discId == "NotFound" ? new MusicBrainzResponse(false, null) : new MusicBrainzResponse(true, _releasesForDiscId[discId]);
        }

        public MusicBrainzResponse GetRelease(string releaseId)
        {
            return new MusicBrainzResponse(true, _releases[releaseId]);
        }
    }
}