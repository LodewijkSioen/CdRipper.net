using System;
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
            var tagSource = new MusicBrainzTagSource(new MockMusicBrainzApi(), new MockCoverArtApi());

            var discTags = tagSource.GetTags(DummyData.SteekJeVingerInDeLucht.TableOfContents).ToList();

            Assert.That(discTags, Is.Not.Null);
            Assert.That(discTags.Count(), Is.EqualTo(1));
            Assert.That(discTags.First().Id, Is.EqualTo(DummyData.SteekJeVingerInDeLucht.MusicBrainzReleases.Keys.First()));
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
            var tagSource = new MusicBrainzTagSource(new MockMusicBrainzApi(), new MockCoverArtApi());

            var discTags = tagSource.GetTags(DummyData.AppelsEten.TableOfContents).ToList();

            Assert.That(discTags, Is.Not.Null);
            Assert.That(discTags.Count(), Is.EqualTo(1));

            var expectedAlbum = new AlbumIdentification
            {
                Id = "S0liNSPBm5gjOHw9JtmPPDhXynI-", //Stub has a special Id
                AlbumArtist = "Ketnet Band",
                AlbumTitle = "Appels Eten",
                Year = null,
                NumberOfTracks = 3
            };
            expectedAlbum.AddTrack(new TrackIdentification { Title = "Appels Eten q2", TrackNumber = 1 });
            expectedAlbum.AddTrack(new TrackIdentification { Title = "Appels Eten Instrumental q2", TrackNumber = 2 });
            expectedAlbum.AddTrack(new TrackIdentification { Title = "Appels Eten PB q2", TrackNumber = 3 });

            AssertAlbum(discTags.First(), expectedAlbum);
        }

        [Test]
        public void TestGettingAnUnknownDiscId()
        {
            var tagSource = new MusicBrainzTagSource(new MockMusicBrainzApi(), new MockCoverArtApi());

            var discTags = tagSource.GetTags(DummyData.UnknownCd.TableOfContents).ToList();

            Assert.That(discTags.Count, Is.EqualTo(1));
            AssertAlbum(discTags.First(), AlbumIdentification.GetEmpty(2));
        }

        [Test]
        public void TestGettingInfoForACdWithMultipleResults()
        {
            var tagSource = new MusicBrainzTagSource(new MockMusicBrainzApi(), new MockCoverArtApi());

            var discTags = tagSource.GetTags(DummyData.MuchAgainstEveryonesAdvice.TableOfContents).ToList();

            Assert.That(discTags.Count, Is.EqualTo(3));

            Assert.That(discTags.ElementAt(0).Id, Is.EqualTo("6d283259-8c9a-3558-9acc-d6c2e429c657"));
            Assert.That(discTags.ElementAt(1).Id, Is.EqualTo("7cf38b3b-d110-318a-ae18-1d4078c347ce"));
            Assert.That(discTags.ElementAt(2).Id, Is.EqualTo("d939579c-cd40-4dd7-8927-8030f7932cbc"));
        }

        [Test]
        public void TestGetInfoForACdWithCoverArt()
        {
            var tagSource = new MusicBrainzTagSource(new MockMusicBrainzApi(), new MockCoverArtApi());

            var discTags = tagSource.GetTags(DummyData.MuchAgainstEveryonesAdvice.TableOfContents).ToList();

            foreach (var tag in discTags)
            {
                var expected = DummyData.MuchAgainstEveryonesAdvice.AlbumArt[tag.Id] == null
                    ? null
                    : new Uri(DummyData.MuchAgainstEveryonesAdvice.AlbumArt[tag.Id]);
                Assert.That(tag.AlbumArt, Is.EqualTo(expected));
            }
        }

        private void AssertAlbum(AlbumIdentification actual, AlbumIdentification expected)
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id), "AlbumId");
            Assert.That(actual.AlbumArtist, Is.EqualTo(expected.AlbumArtist), "AlbumArtist");
            Assert.That(actual.AlbumTitle, Is.EqualTo(expected.AlbumTitle), "AlbumTitle");
            Assert.That(actual.Year, Is.EqualTo(expected.Year), "Year");
            Assert.That(actual.AlbumArt, Is.EqualTo(expected.AlbumArt));
            Assert.That(actual.NumberOfTracks, Is.EqualTo(expected.NumberOfTracks), "NumberOfTracks");
            Assert.That(actual.Tracks.Count(), Is.EqualTo(expected.Tracks.Count()), "Count of tracks on the album");
            for (int i = 0; i < expected.Tracks.Count(); i++)
            {
                var expectedTrack = expected.Tracks.ElementAt(i);
                var actualTrack = actual.Tracks.ElementAt(i);
                AssertTrack(actualTrack, expectedTrack);
            }
        }

        private void AssertTrack(TrackIdentification actualTrack, TrackIdentification expectedTrack)
        {
            Assert.That(actualTrack.Genre, Is.Null, "Genre is not yet implemented");

            Assert.That(actualTrack.Title, Is.EqualTo(expectedTrack.Title), "Title");
            Assert.That(actualTrack.Artist, Is.EqualTo(expectedTrack.Artist), "Artist");
            Assert.That(actualTrack.TrackNumber, Is.EqualTo(expectedTrack.TrackNumber), "TrackNumber");
            Assert.That(actualTrack.AlbumArtist, Is.EqualTo(expectedTrack.AlbumArtist), "AlbumArtist on track");
            Assert.That(actualTrack.AlbumTitle, Is.EqualTo(expectedTrack.AlbumTitle), "AlbumTitle on track");
            Assert.That(actualTrack.Year, Is.EqualTo(expectedTrack.Year), "Year on track");
            Assert.That(actualTrack.TotalNumberOfTracks, Is.EqualTo(expectedTrack.TotalNumberOfTracks), "TotalNumberOfTracks on track");
        }

        [Test, Explicit]
        public void GetDummyDataFromMusicBrainz()
        {
            var cd = DummyData.MuchAgainstEveryonesAdvice;
            var api = new MusicBrainzApi("http://musicbrainz.org/");
            var coverApi = new CoverArtArchiveApi("http://coverartarchive.org");

            var discIdResponse = api.GetReleasesByDiscId(MusicBrainzDiscIdCalculator.CalculateDiscId(cd.TableOfContents));
            Assert.That(discIdResponse.Json, Is.EqualTo(cd.GetReleaseByDiscIdResponse), discIdResponse.Json.Replace(@"""", @"\"""));

            if (cd.MusicBrainzReleases != null)
            {
                foreach (var release in cd.MusicBrainzReleases)
                {
                    var releaseResponse = api.GetRelease(release.Key);
                    Assert.That(releaseResponse.Json, Is.EqualTo(release.Value), releaseResponse.Json.Replace(@"""", @"\"""));
                }
            }
            if (cd.AlbumArt != null)
            {
                foreach (var art in cd.AlbumArt)
                {
                    var cover = coverApi.GetReleasesByDiscId(art.Key);
                    Assert.That(cover, Is.EqualTo(art.Value), cover);
                }
            }
        }
    }

    public class MockMusicBrainzApi : IMusicBrainzApi
    {
        private readonly Dictionary<string, string> _releasesForDiscId;
        private readonly Dictionary<string, string> _releases;

        public MockMusicBrainzApi()
        {
            _releasesForDiscId = new Dictionary<string, string>
            {
                {DummyData.SteekJeVingerInDeLucht.MusicBrainzDiscId, DummyData.SteekJeVingerInDeLucht.GetReleaseByDiscIdResponse},
                {DummyData.AppelsEten.MusicBrainzDiscId, DummyData.AppelsEten.GetReleaseByDiscIdResponse},
                {DummyData.MuchAgainstEveryonesAdvice.MusicBrainzDiscId, DummyData.MuchAgainstEveryonesAdvice.GetReleaseByDiscIdResponse}
            };
            _releases = DummyData.SteekJeVingerInDeLucht.MusicBrainzReleases
                .Concat(DummyData.MuchAgainstEveryonesAdvice.MusicBrainzReleases)
                .ToDictionary(k => k.Key, k => k.Value);
        }

        public MusicBrainzResponse GetReleasesByDiscId(string discId)
        {
            return _releasesForDiscId.ContainsKey(discId) ? new MusicBrainzResponse(true, _releasesForDiscId[discId]) : new MusicBrainzResponse(false, null);
        }

        public MusicBrainzResponse GetRelease(string releaseId)
        {
            return new MusicBrainzResponse(true, _releases[releaseId]);
        }
    }

    public class MockCoverArtApi : ICoverArtArchiveApi
    {
        private readonly Dictionary<string, string> _coverArt;

        public MockCoverArtApi()
        {
            _coverArt = DummyData.MuchAgainstEveryonesAdvice.AlbumArt;
        }

        public string GetReleasesByDiscId(string discId)
        {
            return _coverArt.ContainsKey(discId) ? _coverArt[discId] : null;
        }
    }
}