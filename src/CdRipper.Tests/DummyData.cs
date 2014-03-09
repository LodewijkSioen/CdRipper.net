using System.Collections.Generic;
using CdRipper.Rip;

namespace CdRipper.Tests
{
    public static class DummyData
    {
        public static TableOfContents GetTableOfContentsForJuniorEuroSong2011()
        {
            return new TableOfContents(new List<Track>
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
            });
        }

        public static TableOfContents GetTableOfContentsForSteekJeVingerInDeLucht()
        {
            return new TableOfContents(new List<Track>
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
            });
        }

        public static string MusicBrainzDiscIdSteekJeVingerInDeLucht { get {return "xvIXvh0ibMHH1NNGkT_txTh.2f4-";} }
    }
}