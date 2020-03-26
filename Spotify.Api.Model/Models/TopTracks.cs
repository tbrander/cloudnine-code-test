using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spotify.Api.Model.Json.TopTracks
{
    /// <summary>
    /// Top tracks model. Uncomment fields if necessary
    /// </summary>
    public class TopTracks
    {
        [JsonProperty("tracks")]
        public List<Track> Tracks { get; set; }
    }

    public class Track
    {
        [JsonProperty("album")]
        public Album Album { get; set; }

        //[JsonProperty("artists")]
        //public List<Artist> Artists { get; set; }

        //[JsonProperty("disc_number")]
        //public int disc_number { get; set; }

        [JsonProperty("duration_ms")]
        public int Duration_ms { get; set; }

        //[JsonProperty("_explicit")]
        //public bool _explicit { get; set; }

        //[JsonProperty("external_ids")]
        //public External_Ids external_ids { get; set; }

        [JsonProperty("external_urls")]
        public External_Urls2 External_urls { get; set; }

        //[JsonProperty("href")]
        //public string Href { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        //[JsonProperty("is_local")]
        //public bool Is_local { get; set; }

        //[JsonProperty("is_playable")]
        //public bool Is_playable { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        //[JsonProperty("popularity")]
        //public int popularity { get; set; }

        //[JsonProperty("preview_url")]
        //public string PreviewUrl { get; set; }

        [JsonProperty("track_number")]
        public int TrackNumber { get; set; }

        //[JsonProperty("type")]
        //public string type { get; set; }

        //[JsonProperty("uri")]
        //public string uri { get; set; }
    }


    public class Album
    {
        //[JsonProperty("album_type")]
        //public string Album_type { get; set; }

        [JsonProperty("artists")]
        public List<Artist> Artists { get; set; }

        [JsonProperty("external_urls")]
        public External_Urls ExternalUrls { get; set; }

        //[JsonProperty("href")]
        //public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("release_date")]
        public string Release_date { get; set; }

        //[JsonProperty("release_date_precision")]
        //public string Release_date_precision { get; set; }

        //[JsonProperty("total_tracks")]
        //public int Total_tracks { get; set; }

        //[JsonProperty("type")]
        //public string Type { get; set; }

        //[JsonProperty("uri")]
        //public string Uri { get; set; }
    }

    //public class External_Urls
    //{
    //    [JsonProperty("spotify")]
    //    public string Spotify { get; set; }
    //}

    public class Artist
    {
        //[JsonProperty("external_urls")]
        //public External_Urls1 External_urls { get; set; }

        //[JsonProperty("href")]
        //public string Href { get; set; }

        //[JsonProperty("id")]
        //public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        //[JsonProperty("type")]
        //public string Type { get; set; }

        //[JsonProperty("uri")]
        //public string Uri { get; set; }
    }

    //public class External_Urls1
    //{
    //    [JsonProperty("spotify")]
    //    public string Spotify { get; set; }
    //}

    public class Image
    {
        //[JsonProperty("height")]
        //public int Height { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        //[JsonProperty("width")]
        //public int Width { get; set; }
    }

    //public class External_Ids
    //{
    //    [JsonProperty("isrc")]
    //    public string Isrc { get; set; }
    //}

    public class External_Urls2
    {
        [JsonProperty("spotify")]
        public string Spotify { get; set; }
    }

    //public class Artist1
    //{
    //    [JsonProperty("external_urls")]
    //    public External_Urls3 External_urls { get; set; }

    //    [JsonProperty("href")]
    //    public string Href { get; set; }

    //    [JsonProperty("id")]
    //    public string Id { get; set; }

    //    [JsonProperty("name")]
    //    public string Name { get; set; }

    //    [JsonProperty("type")]
    //    public string Type { get; set; }

    //    [JsonProperty("uri")]
    //    public string Uri { get; set; }
    //}

    //public class External_Urls3
    //{
    //    [JsonProperty("spotify")]
    //    public string Spotify { get; set; }
    //}

}