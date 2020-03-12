using Newtonsoft.Json;
using RestSharp;
using Spotify.Api.Client.Models;
using Spotify.Api.Client.Models.RecommendationModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Api.Client.Helpers
{
    /// <summary>
    /// Helper class for making requests to the Spotify API
    /// </summary>
    public class ApiHelper
    {
        private readonly string BaseUrl = $"{ConfigurationManager.AppSettings["SpotifyApiBaseUrl"]}/{ConfigurationManager.AppSettings["SpotifyApiVersion"]}/";
        private readonly string ClientId = ConfigurationManager.AppSettings["SpotifyClientId"];
        private readonly string ClientSecret = ConfigurationManager.AppSettings["SpotifyClientSecret"];

        /// <summary>
        /// Request data from the search Api by query and type (artist and tracks)
        /// </summary>
        /// <param name="query">search query</param>
        /// <param name="type">artist, genre, tracks</param>
        /// <returns>Task</returns>
        public async Task<RecommendationViewModel> Search(string query, string type)
        {
            IRestClient restclient = new RestClient(BaseUrl);

            RestRequest request = new RestRequest("search");
            request.AddQueryParameter("q", query);

            switch (type)
            {
                case Constants.ARTIST:
                case Constants.GENRES:
                    request.AddQueryParameter("type", "artist"); break;
                case Constants.TRACK:
                    request.AddQueryParameter("type", "track"); break;
            }

            string token = await GetAuthenticationTokenAsync();

            request.AddHeader("Authorization", $"Bearer {token}");

            // execute the request and retrieve the response
            IRestResponse response = await restclient.ExecuteAsync<SearchModel>(request);

            if (response.ErrorException != null)
            {
                LogHelper.WriteExceptionToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, response.ErrorException);
            }
            else if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LogHelper.WriteMessageToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, $"Status code: {(int)response.StatusCode}, description: {response.StatusDescription}");
            }
            else
            {
                try
                {
                    var searchModel = JsonConvert.DeserializeObject<SearchModel>(response.Content);
                    List<string> genres = new List<string>();
                    RecommendationModel result = GetRecommendations(searchModel, type, genres);
                    
                    return new RecommendationViewModel()
                    {
                        Genres = genres,
                        Type = type,
                        Message = result.Tracks.Any() ? $"Search string '{query}'" : $"Found no results for '{query}' ",
                        Tracks = result.Tracks
                    };
                }
                catch (Exception e)
                {
                    LogHelper.WriteExceptionToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, e);
                }
            }
            return new RecommendationViewModel()
            {
                Type = type,
                Message = $"Found no results for '{query}'",
                Tracks = null
            };
        }

        /// <summary>
        /// Prepare request and execute task for retrieving recommendation data based on search result and user selected type
        /// </summary>
        /// <param name="model">Search model</param>
        /// <param name="type">Selected type (artist, genres, tracks)</param>
        /// <returns>RecommendationModel</returns>
        private RecommendationModel GetRecommendations(SearchModel model, string type, List<string> genres)
        {
            try
            {
                Dictionary<string, string> seeds = ExtractSeeds(model, type);
                
                if(type == Constants.GENRES)
                {
                    genres.AddRange(seeds.Values?.ToList());
                }
                
                Task<RecommendationModel> recommendationTask = Task.Run(async () => await Recommendation(model, seeds));
                recommendationTask.Wait();

                RecommendationModel result = recommendationTask.Result;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Extract spotify ids from the SearchModel (artist ids, genres, track ids)
        /// Saved as keyValuePair(ParameterName, ParameterValue) used later in the recommendation request
        /// </summary>
        /// <param name="model">SearchModel obtained in the previus search request</param>
        /// <returns>Dictionary(string, string)</returns>
        private Dictionary<string, string> ExtractSeeds(SearchModel model, string type)
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
                    if (model.Artists != null && model.Artists.Items != null)
                    {
                        var genres = PickRandomGenres(model.Artists.Items.SelectMany(x => x.Genres).Select(z => z.ToString()).Distinct().ToList());

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
        /// Pick random genres from the search result (the api only allows 5 to be passed in when fetching recommendations)
        /// </summary>
        /// <param name="genres"></param>
        /// <returns></returns>
        private List<string> PickRandomGenres(List<string> genres)
        {
            if(genres.Count() <= Constants.MAX_SEED_VALUES)
            {
                return genres;
            }

            List<string> randomGenres = new List<string>();
            Random rnd = new Random();

            while(randomGenres.Count() < Constants.MAX_SEED_VALUES)
            {
                string genre = genres.ElementAt(rnd.Next(genres.Count));
                if (!randomGenres.Contains(genre))
                {
                    randomGenres.Add(genre);
                }
            }
            
            return randomGenres;
        }

        /// <summary>
        /// Request recommendation data based on search result and user selected type
        /// </summary>
        /// <param name="model">Search model</param>
        /// <param name="seeds">Dictionary with seed ids</param>
        /// <returns>Task</returns>
        private async Task<RecommendationModel> Recommendation(SearchModel model, Dictionary<string, string> seeds)
        {
            IRestClient restclient = new RestClient(BaseUrl);

            RestRequest request = new RestRequest("recommendations");

            foreach (KeyValuePair<string, string> entry in seeds)
            {
                request.AddQueryParameter(entry.Key, entry.Value);
            }

            request.AddHeader("Authorization", $"Bearer {await GetAuthenticationTokenAsync()}");

            // execute the request and retrieve the response
            IRestResponse response = await restclient.ExecuteAsync<RecommendationModel>(request);

            if (response.ErrorException != null)
            {
                LogHelper.WriteExceptionToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, response.ErrorException);
            }
            else if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LogHelper.WriteMessageToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, $"Status code: {(int)response.StatusCode}, description: {response.StatusDescription}");
            }
            else
            {
                try
                {
                    var recommendationModel = JsonConvert.DeserializeObject<RecommendationModel>(response.Content);
                    return recommendationModel;
                }
                catch (Exception)
                {
                    LogHelper.WriteExceptionToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, response.ErrorException);
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Get token from MemoryCache or fetch a new token from Spotify Api
        /// </summary>
        /// <returns>Task</returns>
        private async Task<string> GetAuthenticationTokenAsync()
        {
            var cacheKey = "SpotifyWebApiSession-Token" + ClientId;

            var token = MemoryCache.Default.Get(cacheKey) as string;

            if (token == null)
            {
                var timeBeforeRequest = DateTime.Now;

                var response = await GetAuthenticationTokenResponse();
                token = response?.AccessToken;

                if (token == null)
                {
                    throw new AuthenticationException("Spotify authentication failed");
                }

                var expireTime = timeBeforeRequest.AddSeconds(response.ExpiresIn);

                MemoryCache.Default.Set(cacheKey, token, new DateTimeOffset(expireTime));
            }
            return token;
        }

        /// <summary>
        /// Fetch new token from the Spotify Api
        /// </summary>
        /// <returns>Task</returns>
        private async Task<AuthenticationResponse> GetAuthenticationTokenResponse()
        {
            const string AuthenticationEndpoint = "https://accounts.spotify.com/api/token";

            IRestClient restclient = new RestClient(AuthenticationEndpoint);
            RestRequest request = new RestRequest() { Method = Method.POST };
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("Authorization", $"Basic {BuildAuthHeader()}", ParameterType.HttpHeader);

            IRestResponse response = await restclient.ExecuteAsync<AuthenticationResponse>(request);

            if (response.ErrorException != null)
            {
                LogHelper.WriteExceptionToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, response.ErrorException);
            }
            else if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LogHelper.WriteMessageToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, $"Status code: {(int)response.StatusCode}, description: {response.StatusDescription}");
            }
            else
            {
                try
                {
                    var authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(response.Content);
                    return authenticationResponse;
                }
                catch (Exception)
                {
                    LogHelper.WriteExceptionToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, response.ErrorException);
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Create a base64 encoded string of ClientId:ClientSecret
        /// </summary>
        /// <returns>string</returns>
        private string BuildAuthHeader()
        {
            var plainTextBytes = Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}");
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}