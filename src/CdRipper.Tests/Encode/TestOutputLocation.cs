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
                FileNameMask = "track-{title}-{artist}-{tracknumber}-{genre}-{disctitle}-{albumartist}-{numberoftracks}-{year}.mp3"
            };

            var filename = output.CreateFileName(new TrackIdentification
            {
                Title = "title",
                Artist = "artist",
                TrackNumber = 4,
                Genre = "genre",
                AlbumTitle = "disctitle",
                AlbumArtist = "albumartist",
                TotalNumberOfTracks = 25,
                Year = "1854"
            });

            Assert.That(filename, Is.EqualTo(@"c:\test\track-title-artist-04-genre-disctitle-albumartist-25-1854.mp3"));
        }

        [Test]
        public void TestReplaceNullTokens()
        {
            var output = new OutputLocation
            {
                BaseDirectory = @"c:\test",
                FileNameMask = "track-{title}-{disctitle}.mp3"
            };

            var filename = output.CreateFileName(new TrackIdentification());

            Assert.That(filename, Is.EqualTo(@"c:\test\track--.mp3"));
        }

        [Test]
        public void TestIllegalCharactersInFileName()
        {
            var output = new OutputLocation
            {
                BaseDirectory = @"c:\test",
                FileNameMask = @"tr//*/??
ack.mp3"
            };

            var filename = output.CreateFileName(new TrackIdentification());

            Assert.That(filename, Is.EqualTo(@"c:\test\track.mp3"));

        }
    }
}