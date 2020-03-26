using Spotify.Api.Model.Json.Search;
using Spotify.Api.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Api.Service.Utilities
{
    public class Util
    {
        public static Dictionary<string, string> ExtractSeeds(Search model, string type, List<string> genres = null)
        {
            Dictionary<string, string> seeds = new Dictionary<string, string>();

            switch (type)
            {
                case Constants.ARTIST:
                    if (model.Artists != null && model.Artists.Items != null)
                    {
                        seeds.Add("seed_artists", string.Join(",", model.Artists.Items.Take(Constants.MAX_SEED_VALUES).Select(i => i.Id)));
                    }
                    break;
                case Constants.GENRES:
                    if (genres != null)
                    {
                        seeds.Add("seed_genres", string.Join(",", genres));
                    }
                    break;
                case Constants.TRACK:
                    if (model.Tracks != null && model.Tracks.Items != null)
                    {
                        seeds.Add("seed_tracks", string.Join(",", model.Tracks.Items.Take(Constants.MAX_SEED_VALUES).Select(i => i.Id)));
                    }
                    break;
            }
            return seeds;
        }

        /// <summary>
        /// Shuffels genres from the search result and returns 5 or less (the api only allows 5 to be passed in when fetching recommendations)
        /// </summary>
        /// <param name="genres">list of genres for the artist</param>
        /// <returns></returns>
        public static List<string> PickRandomGenres(Search search)
        {
            List<string> genres = search.Artists.Items.SelectMany(g => g.Genres).Select(x => x.ToString()).ToList();
            
            if (genres.Count() <= Constants.MAX_SEED_VALUES)
            {
                return genres;
            }

            genres.Shuffle();
            
            return genres.Take(Constants.MAX_SEED_VALUES).ToList();
        }

        /// <summary>
        /// Create a base64 encoded string of ClientId:ClientSecret
        /// </summary>
        /// <returns>string</returns>
        public static string BuildAuthHeader(string clientId, string clientSecret)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
            return Convert.ToBase64String(plainTextBytes);
        }

        public static bool FoundResult(string type, Search search)
        {
            switch (type)
            {
                case Constants.ARTIST:
                case Constants.GENRES:
                    return search.Artists.Items.Any();
                case Constants.TRACK:
                    return search.Tracks.Items.Any();
                default: return false;
            }
        }
    }

    public static class ListExtension
    {
        /// <summary>
        /// Shuffle list items
        /// </summary>
        /// <param name="list"></param>
        public static void Shuffle<T>(this List<T> list)
        {
            Random rng = new Random(Guid.NewGuid().GetHashCode());

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
