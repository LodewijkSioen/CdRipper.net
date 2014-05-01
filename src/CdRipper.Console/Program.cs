using System.Threading;
using System.Threading.Tasks;
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
       static readonly MusicBrainzTagSource TagSource = new MusicBrainzTagSource(new MusicBrainzApi("http://musicbrainz.org/"));


        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("enter the driveletter to read or ctrl-c to quit");
                var driveletter = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();
                try
                {
                    RipWithMusicBrainz(driveletter).Wait();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }            
        }

        static async Task RipWithMusicBrainz(string driveletter)
        {
            using (var drive = CdDrive.Create(driveletter))
            {
                var toc = await drive.ReadTableOfContents();
                if (toc == null)
                {
                    Console.WriteLine("No CD in drive!");
                    return;
                }

                var discId = TagSource.GetTags(toc).ToList();

                if (discId.Count == 0)
                {
                    Console.WriteLine("No matching cd found in MusicBrainz");
                    return;
                }

                var discNumber = 0;
                if (discId.Count > 1)
                {
                    Console.WriteLine("Multiple matching CD's found in MusicBrainz");
                    for (int i = 0; i < discId.Count; i++)
                    {
                        Console.WriteLine("{0}: {1} - {2}", i+1, discId[i].AlbumArtist, discId[i].AlbumTitle);
                    }
                    Console.WriteLine("Enter the number of the correct cd");
                    discNumber = Convert.ToInt32(Console.ReadLine()) -1;
                }

                foreach (var track in toc.Tracks)
                {
                    Console.WriteLine("track {0}: {1} (lenth={2}-{3})", track.TrackNumber, discId[discNumber].Tracks.First(s => s.TrackNumber == track.TrackNumber).Title, track.Offset, track.Offset + track.Sectors);
                }

                Console.WriteLine("Enter tracknumber to rip");
                var trackNumber = Convert.ToInt32(Console.ReadLine());

                using (var trackReader = new TrackReader(drive))
                {
                    using (var encoder = new LameMp3Encoder(new EncoderSettings
                    {
                        Output = new OutputLocation
                        {
                            BaseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                            FileNameMask = @"encoding\{albumartist}\{albumtitle}\{tracknumber}-{title}.mp3"
                        },
                        Mp3Settings = new Mp3Settings(),
                        Track = discId[discNumber].Tracks.First(s => s.TrackNumber == trackNumber)
                    }))
                    {
                        var cts = new CancellationTokenSource();
                        trackReader.Progress += (i, a) => Console.WriteLine("{0} of {1} read", i, a);
                        
                        var track = toc.Tracks.First(t => t.TrackNumber == trackNumber);
                        await trackReader.ReadTrack(track, b => encoder.Write(b), cts.Token);
                    }
                }
                await drive.Eject();
            }
        }
    }
}
