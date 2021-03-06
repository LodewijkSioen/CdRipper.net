﻿using System;
using CdRipper.Rip;

namespace CdRipper.Tagging
{
    [Obsolete("This is still buggy")]
    public static class FreeDbDiscIdCalculator
    {
        //http://en.wikipedia.org/wiki/CDDB
        //http://introcs.cs.princeton.edu/java/51data/CDDB.java.html
        public static string CalculateDiscId(TableOfContents toc)
        {   
            var numberOfTracks = toc.Tracks.Count;
            var totalLength = 1;//Where this one second comes from I do not know
            var starttimes = 0;

            foreach (var track in toc.Tracks)
            {
                totalLength += (int)Math.Floor(track.Length.TotalSeconds);
                starttimes += SumOfDigits(track.Offset/75);
            }

            var checkSumStartTimes = starttimes % 255;

            return string.Format("{0}{1}{2}",
                checkSumStartTimes.ToString("X").PadLeft(2, '0'),
                totalLength.ToString("X").PadLeft(4, '0'),
                numberOfTracks.ToString("X").PadLeft(2, '0')).ToLowerInvariant();
        }

        private static int SumOfDigits(int number)
        {
            var sum = 0;
            while (number > 0)
            {
                sum = sum + number % 10;
                number = number / 10;
            }
            return sum;
        }
    }
}