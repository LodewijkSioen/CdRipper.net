namespace CdRipper.Tagging
{
    public class TrackIdentification
    {
        private AlbumIdentification _album;

        public TrackIdentification()
        {
            _album = new AlbumIdentification();
        }

        public string Artist { get; set; }
        public string Title { get; set; }
        public int TrackNumber { get; set; }
        public string Genre { get; set; }
        
        public string AlbumArtist { get { return _album.AlbumArtist; } }
        public string AlbumTitle { get { return _album.AlbumTitle; } }
        public string Year { get { return _album.Year; } }
        public int TotalNumberOfTracks { get { return _album.NumberOfTracks; } }

        internal void SetAlbum(AlbumIdentification album)
        {
            _album = album;
        }

        public static TrackIdentification GetEmpty(AlbumIdentification album, int trackNumber = 0)
        {
            return new TrackIdentification()
            {
                Title = "Unknown Title",
                Artist = "Unknown Artist",
                TrackNumber = trackNumber,
            };
        }
    }
}