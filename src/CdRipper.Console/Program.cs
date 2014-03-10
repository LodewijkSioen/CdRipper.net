using CdRipper.Encode;
using CdRipper.Rip;
using System;
using System.IO;
using System.Linq;
using CdRipper.Tagging;
namespace CdRipper.TestConsole
{
    class Program
    {
       static MusicBrainzTagSource _tagSource = new MusicBrainzTagSource(new MusicBrainzApi("http://musicbrainz.org/"));


        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("enter the driveletter to read or ctrl-c to quit");
                var driveletter = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();
                try
                {
                    RipWithMusicBrainz(driveletter);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static void RipWithMusicBrainz(string driveletter)
        {
            using (var drive = new CdDrive(driveletter))
            {
                var toc = drive.ReadTableOfContents();
                var discTag = _tagSource.GetTags(MusicBrainzDiscIdCalculator.CalculateDiscId(toc)).ToList();

                Console.WriteLine("number of tracks:" + toc.Tracks.Count());
                Console.WriteLine("MusicBrainz id:" + MusicBrainzDiscIdCalculator.CalculateDiscId(toc));
                Console.WriteLine("{0} - {1}", discTag[0].Artist, discTag[0].Title);

                foreach (var track in toc.Tracks)
                {
                    Console.WriteLine("track {0}: {1} (lenth={2}-{3})", track.TrackNumber, discTag[0].Tracks.First(s => s.TrackNumber == track.TrackNumber).Title, track.Offset, track.Offset + track.Sectors);
                }

                Console.WriteLine("Enter tracknumber to rip");
                var trackNumber = Convert.ToInt32(Console.ReadLine());

                using (var trackReader = new TrackReader(driveletter))
                {
                    var output=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), String.Format(@"encoding\track{0:00}.mp3", trackNumber));
                    using (var encoder = new LameMp3Encoder(new EncoderSettings
                    {
                        OutputFile = output,
                        Track = discTag[0].Tracks.First(s => s.TrackNumber == trackNumber)
                    }))
                    {
                        var track = toc.Tracks.First(t => t.TrackNumber == trackNumber);
                        trackReader.ReadTrack(track.Offset, track.Sectors,
                            b =>
                            {
                                encoder.Write(b);
                            },
                            (i, a) =>
                            {
                                Console.WriteLine("{0} of {1} read", i, a);
                            });
                    }
                }
            }
        }
    }
}
