using System.Linq;
using CdRipper.Encode;
using CdRipper.Tagging;
using NUnit.Framework;

namespace CdRipper.Tests.Encode
{
    [TestFixture]
    public class TestOutputLocation
    {
        [Test]
        public void TestReplaceTokens()
        {
            var output = new OutputLocation
            {
                BaseDirectory = @"c:\test",
                FileNameMask = @"track\{title}\{artist}-{tracknumber}\{genre}-{albumtitle}.{albumartist}-{numberoftracks}-{year}.mp3"
            };

            var album = new AlbumIdentification
            {
                AlbumTitle = "album title",
                AlbumArtist = "album artist",
                NumberOfTracks = 25,
                Year = "1854"
            };
            album.AddTrack(new TrackIdentification()
            {
                Title = "title",
                Artist = "artist",
                TrackNumber = 4,
                Genre = "genre",
            });

            var filename = output.CreateFileName(album.Tracks.First());

            Assert.That(filename, Is.EqualTo(@"c:\test\track\title\artist-04\genre-album title.album artist-25-1854.mp3"));
        }

        [Test]
        public void TestReplaceNullTokens()
        {
            var output = new OutputLocation
            {
                BaseDirectory = @"c:\test",
                FileNameMask = "track-{title}-{albumtitle}.mp3"
            };

            var album = new AlbumIdentification();
            album.AddTrack(new TrackIdentification());

            var filename = output.CreateFileName(album.Tracks.First());

            Assert.That(filename, Is.EqualTo(@"c:\test\track--.mp3"));
        }

        [Test]
        public void TestIllegalCharactersInFileName()
        {
            var output = new OutputLocation
            {
                BaseDirectory = @"c:\test",
                FileNameMask = @"tr*??//
ack.mp3"
            };

            
            var track = new TrackIdentification();
            var filename = output.CreateFileName(track);

            Assert.That(filename, Is.EqualTo(@"c:\test\tr//ack.mp3"));

        }
    }
}