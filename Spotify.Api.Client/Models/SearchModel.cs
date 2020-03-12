using Newtonsoft.Json;
using System.Collections.Generic;

namespace Spotify.Api.Client.Models
{
    /// <summary>
    /// Model representing the json-object returned from the search endpoint 
    /// </summary>
    public class SearchModel
    {
        [JsonProperty("albums")]
        public Albums Albums { get; set; }

        [JsonProperty("artists")]
        public Artists Artists { get; set; }

        [JsonProperty("tracks")]
        public Tracks Tracks { get; set; }

        [JsonProperty("playlists")]
        public Playlists Playlists { get; set; }
    }

    public class Albums
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("next")]
        public object Next { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("previous")]
        public object Previous { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }

    public class Item
    {
        [JsonProperty("album_type")]
        public string Album_type { get; set; }

        [JsonProperty("artists")]
        public List<Artist> Artists { get; set; }

        [JsonProperty("external_urls")]
        public External_Urls ExternalUrls { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        [JsonProperty("release_date_precision")]
        public string ReleaseDatePrecision { get; set; }

        [JsonProperty("total_tracks")]
        public int TotalTracks { get; set; }

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

    public class Image
    {
        [JsonProperty("height")]
        public int? Height { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int? Width { get; set; }
    }

    public class Artists
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("items")]
        public List<Item1> Items { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }

        [JsonProperty("next")]
        public object Next { get; set; }

        [JsonProperty("offset")]
        public int? Offset { get; set; }

        [JsonProperty("previous")]
        public object Previous { get; set; }

        [JsonProperty("total")]
        public int? Total { get; set; }
    }

    public class Item1
    {
        [JsonProperty("external_urls")]
        public External_Urls2 ExternalUrls { get; set; }

        [JsonProperty("followers")]
        public Followers Followers { get; set; }

        [JsonProperty("genres")]
        public List<object> Genres { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public List<Image1> Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("popularity")]
        public int? Popularity { get; set; }

        [JsonProperty("totypetal")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

    public class External_Urls2
    {
        [JsonProperty("spotify")]
        public string Spotify { get; set; }
    }

    public class Followers
    {
        public int? total { get; set; }
    }

    public class Image1
    {
        [JsonProperty("height")]
        public int? Height { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int? Width { get; set; }
    }

    public class Tracks
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("items")]
        public List<Item2> Items { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("offset")]
        public int? Offset { get; set; }

        [JsonProperty("previous")]
        public object Previous { get; set; }

        [JsonProperty("total")]
        public int? Total { get; set; }
    }

    public class Item2
    {
        [JsonProperty("album")]
        public Album Album { get; set; }

        [JsonProperty("artists")]
        public List<Artist2> Artists { get; set; }

        [JsonProperty("disc_number")]
        public int? Disc_number { get; set; }

        [JsonProperty("duration_ms")]
        public int? Duration_ms { get; set; }

        [JsonProperty("_explicit")]
        public bool Explicit { get; set; }

        [JsonProperty("external_ids")]
        public External_Ids External_ids { get; set; }

        [JsonProperty("external_urls")]
        public External_Urls5 External_urls { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_local")]
        public bool Is_local { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("popularity")]
        public int? Popularity { get; set; }

        [JsonProperty("preview_url")]
        public string Preview_url { get; set; }

        [JsonProperty("track_number")]
        public int? Track_number { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

    public class Album
    {
        public string album_type { get; set; }
        public List<Artist1> artists { get; set; }
        public External_Urls3 external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<Image2> images { get; set; }
        public string name { get; set; }
        public string release_date { get; set; }
        public string release_date_precision { get; set; }
        public int? total_tracks { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class External_Urls3
    {
        public string spotify { get; set; }
    }

    public class Artist1
    {
        public External_Urls4 external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class External_Urls4
    {
        public string spotify { get; set; }
    }

    public class Image2
    {
        public int? height { get; set; }
        public string url { get; set; }
        public int? width { get; set; }
    }

    public class External_Ids
    {
        public string isrc { get; set; }
    }

    public class External_Urls5
    {
        public string spotify { get; set; }
    }

    public class Artist2
    {
        public External_Urls6 external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class External_Urls6
    {
        public string spotify { get; set; }
    }

    public class Playlists
    {
        public string href { get; set; }
        public List<Item3> items { get; set; }
        public int? limit { get; set; }
        public object next { get; set; }
        public int? offset { get; set; }
        public object previous { get; set; }
        public int? total { get; set; }
    }

    public class Item3
    {
        public bool collaborative { get; set; }
        public string description { get; set; }
        public External_Urls7 external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<Image3> images { get; set; }
        public string name { get; set; }
        public Owner owner { get; set; }
        public object primary_color { get; set; }
        public object _public { get; set; }
        public string snapshot_id { get; set; }
        public Tracks1 tracks { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class External_Urls7
    {
        public string spotify { get; set; }
    }

    public class Owner
    {
        public string display_name { get; set; }
        public External_Urls8 external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class External_Urls8
    {
        public string spotify { get; set; }
    }

    public class Tracks1
    {
        public string href { get; set; }
        public int? total { get; set; }
    }

    public class Image3
    {
        public int? height { get; set; }
        public string url { get; set; }
        public int? width { get; set; }
    }


}