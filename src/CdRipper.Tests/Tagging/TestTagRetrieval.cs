﻿using System.Collections.Generic;
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
            Assert.That(discTags.First().Title, Is.EqualTo("Steek Je vinger in de lucht"));
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
        public void TestGettingAnUnknownDiscId()
        {
            var tagSource = new MusicBrainzTagSource(new MockMusicBrainzApi());

            var discTags = tagSource.GetTags("NotFound").ToList();

            Assert.That(discTags.Any(), Is.False);
        }

        [Test, Explicit]
        public void TestDummyData()
        {
            var releaseId = "1477983c-5e51-4946-89a3-4f8024988056";

            var api = new MusicBrainzApi("http://musicbrainz.org/");
            var mockedApi = new MockMusicBrainzApi();

            Assert.That(api.GetReleasesByDiscId(DummyData.SteekJeVingerInDeLucht.MusicBrainzDiscId), Is.EqualTo(mockedApi.GetReleasesByDiscId(DummyData.SteekJeVingerInDeLucht.MusicBrainzDiscId)));
            Assert.That(api.GetRelease(releaseId), Is.EqualTo(mockedApi.GetRelease(releaseId)));
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
                {DummyData.SteekJeVingerInDeLucht.MusicBrainzDiscId, "{\"releases\":[{\"country\":\"BE\",\"text-representation\":{\"script\":\"Latn\",\"language\":\"nld\"},\"status\":\"Official\",\"date\":\"2005\",\"cover-art-archive\":{\"count\":0,\"front\":false,\"artwork\":false,\"back\":false,\"darkened\":false},\"barcode\":\"9789020960976\",\"release-events\":[{\"area\":{\"disambiguation\":\"\",\"iso_3166_3_codes\":[],\"sort-name\":\"Belgium\",\"name\":\"Belgium\",\"id\":\"5b8a5ee5-0bb3-34cf-9a75-c27c44e341fc\",\"iso_3166_2_codes\":[],\"iso_3166_1_codes\":[\"BE\"]},\"date\":\"2005\"}],\"packaging\":null,\"disambiguation\":\"\",\"media\":[{\"track-count\":14,\"discs\":[{\"id\":\"xvIXvh0ibMHH1NNGkT_txTh.2f4-\",\"sectors\":167628}],\"format\":\"CD\",\"position\":1,\"title\":null}],\"id\":\"1477983c-5e51-4946-89a3-4f8024988056\",\"title\":\"Steek Je vinger in de lucht\",\"asin\":\"9020960970\",\"quality\":\"normal\"}],\"id\":\"xvIXvh0ibMHH1NNGkT_txTh.2f4-\",\"sectors\":167628}"}
            };
            _releases = new Dictionary<string, string>
            {
                {DummyData.SteekJeVingerInDeLucht.MusicBrainzReleaseId, "{\"status\":\"Official\",\"date\":\"2005\",\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"cover-art-archive\":{\"count\":0,\"front\":false,\"artwork\":false,\"back\":false,\"darkened\":false},\"packaging\":null,\"disambiguation\":\"\",\"media\":[{\"track-offset\":0,\"tracks\":[{\"length\":170280,\"number\":\"1\",\"recording\":{\"disambiguation\":\"\",\"length\":\"170280\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"81e6a71b-d2a6-490d-b14f-500f9d713f6e\",\"title\":\"Alles Goed\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Alles Goed\",\"id\":\"f3506584-2112-3287-bcab-084fdf5edef6\"},{\"length\":151973,\"number\":\"2\",\"recording\":{\"disambiguation\":\"\",\"length\":\"151973\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"40a8cb1a-cb42-44a7-aec7-e602eadf0071\",\"title\":\"Wakker\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Wakker\",\"id\":\"19ea53cb-705d-3395-9f2d-4f95a2f9053b\"},{\"length\":140573,\"number\":\"3\",\"recording\":{\"disambiguation\":\"\",\"length\":\"140573\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"05df56ee-d395-4b0d-9032-17634885549c\",\"title\":\"Droog Bed\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Droog Bed\",\"id\":\"a9d6ef47-98ca-3840-8b13-9ab84346ba13\"},{\"length\":156400,\"number\":\"4\",\"recording\":{\"disambiguation\":\"\",\"length\":\"156400\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"d2452bf0-db22-4dcb-b6d2-74437fd0c3ab\",\"title\":\"Postbode\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Postbode\",\"id\":\"9b7aa526-b4d3-370a-a2e6-24246156f63b\"},{\"length\":166626,\"number\":\"5\",\"recording\":{\"disambiguation\":\"\",\"length\":\"166626\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"0dc8d786-ed89-4107-88ff-83fc67d472c7\",\"title\":\"Vrooaar\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Vrooaar\",\"id\":\"87027bbe-1e2b-303f-b7f2-5fe3911e66c7\"},{\"length\":211306,\"number\":\"6\",\"recording\":{\"disambiguation\":\"\",\"length\":\"211306\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"6c2b319b-4054-40c9-8efe-b8869f7ca64b\",\"title\":\"Waarom\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Waarom\",\"id\":\"95c394ef-ace5-3393-bdf0-d4094842f7a2\"},{\"length\":144680,\"number\":\"7\",\"recording\":{\"disambiguation\":\"\",\"length\":\"144680\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"4e86d099-a9de-4c0f-9534-ef8a6cc00080\",\"title\":\"Wip Er Op Los\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Wip Er Op Los\",\"id\":\"ac47b438-f08e-303e-adc2-251b3d4e30e3\"},{\"length\":152773,\"number\":\"8\",\"recording\":{\"disambiguation\":\"\",\"length\":\"152773\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"83945da8-8381-4e8c-bc59-c633695da06f\",\"title\":\"Kling Klang\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Kling Klang\",\"id\":\"ba3d79bb-0f96-3975-97c5-1eca8d821e67\"},{\"length\":119506,\"number\":\"9\",\"recording\":{\"disambiguation\":\"\",\"length\":\"119506\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"8b48ae3f-b80b-41f8-8313-685e274d5e5a\",\"title\":\"Om Ter Eerst\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Om Ter Eerst\",\"id\":\"ee971eb6-c814-30bc-aa17-4d4ff709bec9\"},{\"length\":225946,\"number\":\"10\",\"recording\":{\"disambiguation\":\"\",\"length\":\"225946\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"4ef34cd6-1722-420c-996e-f723baf20ed1\",\"title\":\"Blinkeuh\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Blinkeuh\",\"id\":\"b1345a63-1f24-39ce-a931-2fa053d40bfb\"},{\"length\":212573,\"number\":\"11\",\"recording\":{\"disambiguation\":\"\",\"length\":\"212573\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"64a948f0-fb79-4e54-90e1-ed77fed9e84c\",\"title\":\"Pesten Is Een Pest\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Pesten Is Een Pest\",\"id\":\"20374f75-276a-3e1a-aea6-c2b564b3f72e\"},{\"length\":111146,\"number\":\"12\",\"recording\":{\"disambiguation\":\"\",\"length\":\"111146\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"2d7050aa-fa9c-45b4-9996-0d52cd44561a\",\"title\":\"Ik Stuur Mezelf Wel Op\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Ik Stuur Mezelf Wel Op\",\"id\":\"a62e4faa-e9e4-3d19-bbca-85efb07b21ee\"},{\"length\":129173,\"number\":\"13\",\"recording\":{\"disambiguation\":\"\",\"length\":\"129173\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"87e1f9b2-d95a-4d13-ac16-5685b20456ee\",\"title\":\"Zwemmie Zwem\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Zwemmie Zwem\",\"id\":\"9dc6c8ec-2af4-3bb6-aeb3-ddf5664944ff\"},{\"length\":140040,\"number\":\"14\",\"recording\":{\"disambiguation\":\"\",\"length\":\"140040\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"eefaf35d-6951-4a13-8bdb-e17c870bf4f8\",\"title\":\"Steek Je Vinger in de Lucht\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Steek Je Vinger in de Lucht\",\"id\":\"991a9a43-1ce1-34c2-a266-d0e8202b35e7\"}],\"track-count\":14,\"discs\":[{\"id\":\"xvIXvh0ibMHH1NNGkT_txTh.2f4-\",\"sectors\":167628}],\"format\":\"CD\",\"position\":1,\"title\":null}],\"id\":\"1477983c-5e51-4946-89a3-4f8024988056\",\"asin\":\"9020960970\",\"quality\":\"normal\",\"country\":\"BE\",\"text-representation\":{\"script\":\"Latn\",\"language\":\"nld\"},\"barcode\":\"9789020960976\",\"release-events\":[{\"area\":{\"disambiguation\":\"\",\"iso_3166_3_codes\":[],\"sort-name\":\"Belgium\",\"name\":\"Belgium\",\"id\":\"5b8a5ee5-0bb3-34cf-9a75-c27c44e341fc\",\"iso_3166_2_codes\":[],\"iso_3166_1_codes\":[\"BE\"]},\"date\":\"2005\"}],\"title\":\"Steek Je vinger in de lucht\",\"label-info\":[{\"label\":{\"disambiguation\":\"Book publisher, also publishes books that come with a CD\",\"sort-name\":\"Lannoo\",\"name\":\"Lannoo\",\"id\":\"d4fabb83-7809-4296-aac1-539bd9ed0923\",\"label-code\":null},\"catalog-number\":\"ISBN 9020960970\"}]}"}
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