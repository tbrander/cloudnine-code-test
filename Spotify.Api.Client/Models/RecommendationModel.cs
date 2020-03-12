using Newtonsoft.Json;
using System.Collections.Generic;

namespace Spotify.Api.Client.Models.RecommendationModel
{
    public class RecommendationViewModel
    {
        public List<string> Genres { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public List<Track> Tracks { get; set; }
    }

    public class RecommendationModel
    {
        [JsonProperty("tracks")]
        public List<Track> Tracks { get; set; }

        [JsonProperty("seeds")]
        public List<Seed> Seeds { get; set; }
    }

    public class Track
    {
        [JsonProperty("artists")]
        public List<Artist> Artists { get; set; }

        [JsonProperty("disc_number")]
        public int DiscNumber { get; set; }

        [JsonProperty("duration_ms")]
        public int Duration_ms { get; set; }

        [JsonProperty("_explicit")]
        public bool Explicit { get; set; }

        [JsonProperty("external_urls")]
        public External_Urls ExternalUrls { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_playable")]
        public bool IsPlayable { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("preview_url")]
        public string Preview_url { get; set; }

        [JsonProperty("track_number")]
        public int TrackNumber { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

    public class External_Urls
    {
        [JsonProperty("spotify")]
        public string Spotify { get; set; }
    }

    public class Artist
    {
        [JsonProperty("external_urls")]
        public External_Urls1 ExternalUrls { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

    public class External_Urls1
    {
        [JsonProperty("spotify")]
        public string Spotify { get; set; }
    }

    public class Seed
    {
        [JsonProperty("initialPoolSize")]
        public int InitialPoolSize { get; set; }

        [JsonProperty("afterFilteringSize")]
        public int AfterFilteringSize { get; set; }

        [JsonProperty("afterRelinkingSize")]
        public int AfterRelinkingSize { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}