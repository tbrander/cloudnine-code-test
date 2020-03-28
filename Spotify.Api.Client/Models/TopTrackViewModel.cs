using System.Collections.Generic;

namespace Spotify.Api.Client.Models
{
    public class TopTrackViewModel
    {
        public string AlbumId { get; set; }
        public string AlbumName { get; set; }
        public string ArtistName { get; set; }
        public List<TrackViewModel> TrackList { get; set; }
        public string AlbumImageSource { get; set; }
        public string SpotifyUrl { get; set; }

        public TopTrackViewModel()
        {
            TrackList = new List<TrackViewModel>();
        }

    }

    public class TrackViewModel
    {
        public string TrackId { get; set; }
        public string TrackName { get; set; }
        public int TrackNumber { get; set; }
        public decimal Duration { get; set; }
    }
}