using System.Collections.Generic;
using CdRipper.Rip;

namespace CdRipper.Tests
{
    public class DummyCd
    {
        public TableOfContents TableOfContents { get; set; }
        public string MusicBrainzDiscId { get; set; }
        public string MusicBrainzReleaseId { get; set; }
        public string CdDbDiscId { get; set; }
        public string GetReleaseByDiscIdResponse { get; set; }
        public string GetReleaseResponse { get; set; }
    }

    public static class DummyData
    {
        public static DummyCd UnknownCd
        {
            get
            {
                return new DummyCd
                {
                    TableOfContents = new TableOfContents(new[]{
                        new Track(1, 150, 2000),
                        new Track(1, 2150, 4000),
                    })
                };
            }
        }

        /// <summary>
        /// Pregap track <see cref="http://wiki.musicbrainz.org/ReleasesWithPregapTracks"/> and some wierd stuff at the end
        /// </summary>
        public static DummyCd MuchAgainstEveryonesAdvice
        {
            get
            {
                return new DummyCd
                {
                    TableOfContents = new TableOfContents(new[]
                    {
                        new Track(1,  4462  , 13998),
                        new Track(2,  18460 , 14337),
                        new Track(3,  32797 , 15715),
                        new Track(4,  48512 , 12620),
                        new Track(5,  61132 , 9313 ),
                        new Track(6,  70445 , 20057),
                        new Track(7,  90502 , 12168),
                        new Track(8,  102670, 17097),
                        new Track(9,  119767, 19490),
                        new Track(10, 139257, 18790),
                        new Track(11, 158047, 11568),
                        new Track(12, 169615, 19795),
                        new Track(13, 189410, 16427),
                        new Track(14, 205837, 20543), 
                    }),
                    MusicBrainzDiscId = "s3I6bSQb4qj2otl9xj8YpCBZeI0-",
                    MusicBrainzReleaseId = "6d283259-8c9a-3558-9acc-d6c2e429c657",
                };
            }
        }

        /// <summary>
        /// This disc returns a CdStub <see cref="http://musicbrainz.org/doc/CD_Stub"/>
        /// </summary>
        public static DummyCd AppelsEten
        {
            get
            {
                return new DummyCd
                {
                    TableOfContents = new TableOfContents(new List<Track>
                    {
                        new Track(1, 150, 12212),
                        new Track(2, 12362, 12233),
                        new Track(3, 24595, 12107),
                    }),
                    MusicBrainzDiscId = "S0liNSPBm5gjOHw9JtmPPDhXynI-",
                    GetReleaseByDiscIdResponse = "{\"disambiguation\":\"Kom op appels!\",\"track-count\":null,\"tracks\":[{\"length\":162826,\"artist\":null,\"title\":\"Appels Eten q2\"},{\"length\":163106,\"artist\":null,\"title\":\"Appels Eten Instrumental q2\"},{\"length\":161426,\"artist\":null,\"title\":\"Appels Eten PB q2\"}],\"artist\":\"Ketnet Band\",\"barcode\":null,\"title\":\"Appels Eten\",\"id\":\"S0liNSPBm5gjOHw9JtmPPDhXynI-\"}"
                };
            }
        }

        /// <summary>
        /// This one returns a real Release 
        /// </summary>
        public static DummyCd SteekJeVingerInDeLucht
        {
            get
            {
                return new DummyCd
                {
                    TableOfContents = new TableOfContents(new List<Track>
                    {
                        new Track(1, 150, 12771),
                        new Track(2, 12921, 11398),
                        new Track(3, 24319, 10543),
                        new Track(4, 34862, 11730),
                        new Track(5, 46592, 12497),
                        new Track(6, 59089, 15848),
                        new Track(7, 74937, 10851),
                        new Track(8, 85788, 11458),
                        new Track(9, 97246, 8963),
                        new Track(10, 106209, 16946),
                        new Track(11, 123155, 15943),
                        new Track(12, 139098, 8336),
                        new Track(13, 147434, 9688),
                        new Track(14, 157122, 10506),
                    }),

                    MusicBrainzDiscId = "xvIXvh0ibMHH1NNGkT_txTh.2f4-",
                    MusicBrainzReleaseId = "1477983c-5e51-4946-89a3-4f8024988056",
                    CdDbDiscId = "c708b90e",
                    GetReleaseByDiscIdResponse = "{\"releases\":[{\"country\":\"BE\",\"text-representation\":{\"script\":\"Latn\",\"language\":\"nld\"},\"status\":\"Official\",\"date\":\"2005\",\"cover-art-archive\":{\"count\":0,\"front\":false,\"artwork\":false,\"back\":false,\"darkened\":false},\"barcode\":\"9789020960976\",\"release-events\":[{\"area\":{\"disambiguation\":\"\",\"iso_3166_3_codes\":[],\"sort-name\":\"Belgium\",\"name\":\"Belgium\",\"id\":\"5b8a5ee5-0bb3-34cf-9a75-c27c44e341fc\",\"iso_3166_2_codes\":[],\"iso_3166_1_codes\":[\"BE\"]},\"date\":\"2005\"}],\"packaging\":null,\"disambiguation\":\"\",\"media\":[{\"track-count\":14,\"discs\":[{\"id\":\"xvIXvh0ibMHH1NNGkT_txTh.2f4-\",\"sectors\":167628}],\"format\":\"CD\",\"position\":1,\"title\":null}],\"id\":\"1477983c-5e51-4946-89a3-4f8024988056\",\"title\":\"Steek Je vinger in de lucht\",\"asin\":\"9020960970\",\"quality\":\"normal\"}],\"id\":\"xvIXvh0ibMHH1NNGkT_txTh.2f4-\",\"sectors\":167628}",
                    GetReleaseResponse = "{\"status\":\"Official\",\"date\":\"2005\",\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"cover-art-archive\":{\"count\":0,\"front\":false,\"artwork\":false,\"back\":false,\"darkened\":false},\"packaging\":null,\"disambiguation\":\"\",\"media\":[{\"track-offset\":0,\"tracks\":[{\"length\":170280,\"number\":\"1\",\"recording\":{\"disambiguation\":\"\",\"length\":\"170280\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"81e6a71b-d2a6-490d-b14f-500f9d713f6e\",\"title\":\"Alles Goed\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Alles Goed\",\"id\":\"f3506584-2112-3287-bcab-084fdf5edef6\"},{\"length\":151973,\"number\":\"2\",\"recording\":{\"disambiguation\":\"\",\"length\":\"151973\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"40a8cb1a-cb42-44a7-aec7-e602eadf0071\",\"title\":\"Wakker\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Wakker\",\"id\":\"19ea53cb-705d-3395-9f2d-4f95a2f9053b\"},{\"length\":140573,\"number\":\"3\",\"recording\":{\"disambiguation\":\"\",\"length\":\"140573\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"05df56ee-d395-4b0d-9032-17634885549c\",\"title\":\"Droog Bed\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Droog Bed\",\"id\":\"a9d6ef47-98ca-3840-8b13-9ab84346ba13\"},{\"length\":156400,\"number\":\"4\",\"recording\":{\"disambiguation\":\"\",\"length\":\"156400\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"d2452bf0-db22-4dcb-b6d2-74437fd0c3ab\",\"title\":\"Postbode\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Postbode\",\"id\":\"9b7aa526-b4d3-370a-a2e6-24246156f63b\"},{\"length\":166626,\"number\":\"5\",\"recording\":{\"disambiguation\":\"\",\"length\":\"166626\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"0dc8d786-ed89-4107-88ff-83fc67d472c7\",\"title\":\"Vrooaar\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Vrooaar\",\"id\":\"87027bbe-1e2b-303f-b7f2-5fe3911e66c7\"},{\"length\":211306,\"number\":\"6\",\"recording\":{\"disambiguation\":\"\",\"length\":\"211306\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"6c2b319b-4054-40c9-8efe-b8869f7ca64b\",\"title\":\"Waarom\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Waarom\",\"id\":\"95c394ef-ace5-3393-bdf0-d4094842f7a2\"},{\"length\":144680,\"number\":\"7\",\"recording\":{\"disambiguation\":\"\",\"length\":\"144680\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"4e86d099-a9de-4c0f-9534-ef8a6cc00080\",\"title\":\"Wip Er Op Los\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Wip Er Op Los\",\"id\":\"ac47b438-f08e-303e-adc2-251b3d4e30e3\"},{\"length\":152773,\"number\":\"8\",\"recording\":{\"disambiguation\":\"\",\"length\":\"152773\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"83945da8-8381-4e8c-bc59-c633695da06f\",\"title\":\"Kling Klang\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Kling Klang\",\"id\":\"ba3d79bb-0f96-3975-97c5-1eca8d821e67\"},{\"length\":119506,\"number\":\"9\",\"recording\":{\"disambiguation\":\"\",\"length\":\"119506\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"8b48ae3f-b80b-41f8-8313-685e274d5e5a\",\"title\":\"Om Ter Eerst\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Om Ter Eerst\",\"id\":\"ee971eb6-c814-30bc-aa17-4d4ff709bec9\"},{\"length\":225946,\"number\":\"10\",\"recording\":{\"disambiguation\":\"\",\"length\":\"225946\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"4ef34cd6-1722-420c-996e-f723baf20ed1\",\"title\":\"Blinkeuh\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Blinkeuh\",\"id\":\"b1345a63-1f24-39ce-a931-2fa053d40bfb\"},{\"length\":212573,\"number\":\"11\",\"recording\":{\"disambiguation\":\"\",\"length\":\"212573\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"64a948f0-fb79-4e54-90e1-ed77fed9e84c\",\"title\":\"Pesten Is Een Pest\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Pesten Is Een Pest\",\"id\":\"20374f75-276a-3e1a-aea6-c2b564b3f72e\"},{\"length\":111146,\"number\":\"12\",\"recording\":{\"disambiguation\":\"\",\"length\":\"111146\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"2d7050aa-fa9c-45b4-9996-0d52cd44561a\",\"title\":\"Ik Stuur Mezelf Wel Op\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Ik Stuur Mezelf Wel Op\",\"id\":\"a62e4faa-e9e4-3d19-bbca-85efb07b21ee\"},{\"length\":129173,\"number\":\"13\",\"recording\":{\"disambiguation\":\"\",\"length\":\"129173\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"87e1f9b2-d95a-4d13-ac16-5685b20456ee\",\"title\":\"Zwemmie Zwem\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Zwemmie Zwem\",\"id\":\"9dc6c8ec-2af4-3bb6-aeb3-ddf5664944ff\"},{\"length\":140040,\"number\":\"14\",\"recording\":{\"disambiguation\":\"\",\"length\":\"140040\",\"video\":0,\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"id\":\"eefaf35d-6951-4a13-8bdb-e17c870bf4f8\",\"title\":\"Steek Je Vinger in de Lucht\"},\"artist-credit\":[{\"artist\":{\"disambiguation\":\"\",\"sort-name\":\"De Smet, Jan\",\"name\":\"Jan De Smet\",\"id\":\"b049db53-6b19-405e-8b8e-ddfc4682bf95\"},\"name\":\"Jan De Smet\",\"joinphrase\":\"\"}],\"title\":\"Steek Je Vinger in de Lucht\",\"id\":\"991a9a43-1ce1-34c2-a266-d0e8202b35e7\"}],\"track-count\":14,\"discs\":[{\"id\":\"xvIXvh0ibMHH1NNGkT_txTh.2f4-\",\"sectors\":167628}],\"format\":\"CD\",\"position\":1,\"title\":null}],\"id\":\"1477983c-5e51-4946-89a3-4f8024988056\",\"asin\":\"9020960970\",\"quality\":\"normal\",\"country\":\"BE\",\"text-representation\":{\"script\":\"Latn\",\"language\":\"nld\"},\"barcode\":\"9789020960976\",\"release-events\":[{\"area\":{\"disambiguation\":\"\",\"iso_3166_3_codes\":[],\"sort-name\":\"Belgium\",\"name\":\"Belgium\",\"id\":\"5b8a5ee5-0bb3-34cf-9a75-c27c44e341fc\",\"iso_3166_2_codes\":[],\"iso_3166_1_codes\":[\"BE\"]},\"date\":\"2005\"}],\"title\":\"Steek Je vinger in de lucht\",\"label-info\":[{\"label\":{\"disambiguation\":\"Book publisher, also publishes books that come with a CD\",\"sort-name\":\"Lannoo\",\"name\":\"Lannoo\",\"id\":\"d4fabb83-7809-4296-aac1-539bd9ed0923\",\"label-code\":null},\"catalog-number\":\"ISBN 9020960970\"}]}"
                };
            }
        }

        public static DummyCd JuniorEuroSong2011
        {
            get
            {
                return new DummyCd
                {
                    TableOfContents = new TableOfContents(new List<Track>
                    {
                        new Track(1, 150, 12836),
                        new Track(2, 12986, 12273),
                        new Track(3, 25259, 12732),
                        new Track(4, 37991, 11353),
                        new Track(5, 49344, 12149),
                        new Track(6, 61493, 12495),
                        new Track(7, 73988, 12486),
                        new Track(8, 86474, 12763),
                        new Track(9, 99237, 12441),
                        new Track(10, 111678, 13419),
                        new Track(11, 125097, 12844),
                        new Track(12, 137941, 12284),
                        new Track(13, 150225, 12766),
                        new Track(14, 162991, 11334),
                        new Track(15, 174325, 12187),
                        new Track(16, 186512, 12469),
                        new Track(17, 198981, 12486),
                        new Track(18, 211467, 12800),
                        new Track(19, 224267, 12446),
                        new Track(20, 236713, 13351),
                    }),
                    MusicBrainzDiscId = "WMnoPf6.FF0PALnVth5jCRT1LxI-",
                    CdDbDiscId = "290d0414"
                };
            }
        }
    }
}