using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Spotify.Api.Web.Models.RecommendationView
{
    public class RecommendationViewModel
    {
        public List<string> Genres { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public List<Track> Tracks { get; set; }

        public RecommendationViewModel()
        {
            this.Tracks = new List<Track>();
        }
    }



    public class Track
    {
        public List<Artist> Artists { get; set; }
        public int Duration_ms { get; set; }
        public External_Urls ExternalUrls { get; set; }
        public string Name { get; set; }
        public string Preview_url { get; set; }
        public string Type { get; set; }
    }

    public class External_Urls
    {
        public string Spotify { get; set; }
    }

    public class Artist
    {
        public External_Urls1 ExternalUrls { get; set; }
        
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Type { get; set; }
    }

    public class External_Urls1
    {
        public string Spotify { get; set; }
    }
}