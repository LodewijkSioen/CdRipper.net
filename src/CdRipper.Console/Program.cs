﻿using CdRipper.CdDb;
using CdRipper.Encode;
using CdRipper.Rip;
using System;
using System.IO;
using System.Linq;
using DiscId;

namespace CdRipper.TestConsole
{
    class Program
    {
        private const bool withmusicbrainz = true;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("enter the driveletter to read or ctrl-c to quit");
                var driveletter = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();
                try
                {
                    if (withmusicbrainz)
                    {
                        RipWithMusicBrainz(driveletter);
                    }
                    else
                    {
                        RipWithNativeMethods(driveletter);    
                    }
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static void RipWithMusicBrainz(string driveletter)
        {
            using (var drive = Disc.Read(driveletter + ":"))
            {
                Console.WriteLine("number of tracks:" + drive.Tracks.Count());
                Console.WriteLine("CDDB id:" + drive.FreedbId);
                Console.WriteLine("MusicBrainz id:" + drive.Id);
                foreach (var track in drive.Tracks)
                {
                    Console.WriteLine("track {0}: lenth={1}-{2}", track.Number, track.Offset, track.Offset + track.Sectors);
                }

                Console.WriteLine("Enter tracknumber to rip");
                var trackNumber = Convert.ToInt32(Console.ReadLine());

                using (var trackReader = new TrackReader(driveletter))
                {
                    using (var encoder = new LameMp3Encoder(new EncoderSettings
                    {
                        OutputFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), String.Format(@"encoding\track{0:##}.mp3", trackNumber))
                    }))
                    {
                        var track = drive.Tracks.First(t => t.Number == trackNumber);
                        trackReader.ReadTrack(track.Offset, track.Offset+track.Sectors,
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

        static void RipWithNativeMethods(string driveletter)
        {
            using (var drive = new CdDrive(driveletter))
            {
                if (!drive.IsCdInDrive())
                {
                    Console.WriteLine("No CD in drive");
                    return;
                }

                var toc = drive.ReadTableOfContents();
                Console.WriteLine("number of tracks:" + toc.Tracks.Count);
                Console.WriteLine("CDDB id:" + FreeDbDiscIdCalculator.CalculateDiscId(toc));
                foreach (var track in toc.Tracks)
                {
                    Console.WriteLine("track {0}: lenth={1}-{2}", track.TrackNumber, track.StartSector, track.EndSector);
                }
                Console.WriteLine("Enter tracknumber to rip");
                var trackNumber = Convert.ToInt32(Console.ReadLine());
                using (var trackReader = new TrackReader(drive))
                {
                    using (var encoder = new LameMp3Encoder(new EncoderSettings
                    {
                        OutputFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), String.Format(@"encoding\track{0:##}.mp3", trackNumber))
                    }))
                    {
                        trackReader.ReadTrack(toc.Tracks.First(t => t.TrackNumber == trackNumber),
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
