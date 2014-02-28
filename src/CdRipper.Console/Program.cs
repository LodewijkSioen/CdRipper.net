using System;

namespace CdRipper.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("enter the driveletter to read or ctrl-c to quit");
                var driveletter = Console.ReadKey();
                Console.WriteLine();
                try
                {
                    using (var drive = new CdDrive(driveletter.KeyChar.ToString()))
                    {
                        if (!drive.IsCdInDrive())
                        {
                            Console.WriteLine("No CD in drive");
                            continue;
                        }

                        var toc = drive.ReadTableOfContents();
                        Console.WriteLine("number of tracks:" + toc.Tracks.Count);
                        Console.WriteLine("CDDB id:" + CdRipper.DiscIdCalculator.CalculateDiscId(toc));
                        foreach (var track in toc.Tracks)
                        {
                            Console.WriteLine("track {0}: lenth={1}-{2}", track.TrackNumber, track.StartSector, track.EndSector);
                        }
                        Console.WriteLine("Enter tracknumber to rip");
                        var trackNumber = Convert.ToInt32(Console.ReadLine());
                        using (var trackReader = new TrackReader(drive))
                        {                            
                            trackReader.ReadTrack(toc.Tracks[trackNumber], b => { }, (i, a) =>
                            {
                                Console.WriteLine("{0} of {1} read", i, a);
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
