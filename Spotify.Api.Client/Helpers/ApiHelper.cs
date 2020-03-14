using Newtonsoft.Json;
using RestSharp;
using Spotify.Api.Client.Models;
using Spotify.Api.Client.Models.RecommendationModel;
using Spotify.Api.Client.Models.TopTracks;
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
        private readonly string AuthenticationEndpoint = ConfigurationManager.AppSettings["SpotifyAuthTokenUrl"];

        /// <summary>
        /// Returns list of top tracks for the specified artist-/spotify id in the given country
        /// </summary>
        /// <param name="spotifyId">id of the artist</param>
        /// <param name="countryCode">top tracks from this country</param>
        /// <returns></returns>
        // https://developer.spotify.com/documentation/web-api/reference/artists/get-artists-top-tracks/
        public async Task<List<TopTrackViewModel>> TopTracksAsync(string spotifyId, string countryCode)
        {
            IRestClient restclient = new RestClient(BaseUrl);

            RestRequest request = new RestRequest($"artists/{spotifyId}/top-tracks");
            request.AddQueryParameter("country", countryCode);

            string token = string.Empty;
            try
            {
                token = await GetAuthenticationTokenAsync();
            }
            catch (Exception e)
            {
                LogHelper.WriteExceptionToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, e);

            }

            request.AddHeader("Authorization", $"Bearer {token}");

            // execute the request and retrieve the response
            IRestResponse response = await restclient.ExecuteAsync<TopTracks>(request);

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
                    var topTracks = JsonConvert.DeserializeObject<TopTracks>(response.Content);

                    List<TopTrackViewModel> ttvm = ViewModelHelper.CreateTopTracksViewModel(topTracks);
                    
                    return ttvm;
                }
                catch (Exception e)
                {
                    LogHelper.WriteExceptionToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, e);
                }
            }

            return null;
        }
        
        /// <summary>
        /// Request data from the Spotify search Api by query and type (artist, track or genre)
        /// </summary>
        /// <param name="query">search query</param>
        /// <param name="type">artist, genre, tracks</param>
        /// <returns>Task</returns>
        // https://developer.spotify.com/documentation/web-api/reference/search/search/
        public async Task<RecommendationViewModel> SearchAsync(string query, string type)
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

            string token = string.Empty;
            try
            {
                token = await GetAuthenticationTokenAsync();
            }
            catch (Exception e)
            {
                LogHelper.WriteExceptionToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, e);
                return new RecommendationViewModel()
                {
                    Message = $"An error occured, please try again later",
                };
            }

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

                    // PickRandomGenres: An artist is often divided into several genres but since the api only takes in 5 genres these 5 are randomly picked.
                    List<string> genres = type == Constants.GENRES ? PickRandomGenres(searchModel.Artists.Items.SelectMany(g => g.Genres).Select(x => x.ToString()).ToList()) : null;
                    RecommendationModel recommendation = await GetRecommendationsAsync(searchModel, type, genres);

                    if (recommendation != null && recommendation.Tracks != null)
                    {
                        return ViewModelHelper.CreateRecommendationViewModel(recommendation, query, genres, type);
                    }
                }
                catch (Exception e)
                {
                    LogHelper.WriteExceptionToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, e);
                }
            }

            return new RecommendationViewModel()
            {
                Message = $"An error occured, please try again later",
            };
        }

        /// <summary>
        /// Extract spotify ids from the SearchModel (artist ids, genres, track ids)
        /// Saved as keyValuePair(ParameterName, ParameterValue) used later in the recommendation request
        /// </summary>
        /// <param name="model">SearchModel obtained in the previus search request</param>
        /// <returns>Dictionary(string, string)</returns>
        private Dictionary<string, string> ExtractSeeds(SearchModel model, string type, List<string> genres = null)
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
        /// Pick random genres from the search result (the api only allows 5 to be passed in when fetching recommendations)
        /// </summary>
        /// <param name="genres">list of genres for the artist</param>
        /// <returns></returns>
        private List<string> PickRandomGenres(List<string> genres)
        {
            // no need to pick random when we got max values or less
            if (genres.Count() <= Constants.MAX_SEED_VALUES)
            {
                return genres;
            }

            List<string> randomGenres = new List<string>();
            Random rnd = new Random();

            while (randomGenres.Count() < Constants.MAX_SEED_VALUES)
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
        /// <param name="type">the type of recommendation requested</param>
        /// <param name="genres">list of genres for the artist</param>
        /// <returns>Task</returns>
        // https://developer.spotify.com/documentation/web-api/reference/browse/get-recommendations/
        private async Task<RecommendationModel> GetRecommendationsAsync(SearchModel model, string type, List<string> genres = null)
        {
            Dictionary<string, string> seeds = ExtractSeeds(model, type, genres);

            IRestClient restclient = new RestClient(BaseUrl);

            RestRequest request = new RestRequest("recommendations");

            // adding seed parameter
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

                var response = await GetAuthenticationTokenResponseAsync();
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
        // https://developer.spotify.com/documentation/general/guides/authorization-guide/
        private async Task<AuthenticationResponse> GetAuthenticationTokenResponseAsync()
        {
            IRestClient restclient = new RestClient(AuthenticationEndpoint);
            RestRequest request = new RestRequest() { Method = Method.POST };
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("Authorization", $"Basic {BuildAuthHeader()}", ParameterType.HttpHeader);

            IRestResponse response = await restclient.ExecuteAsync<AuthenticationResponse>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            else if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new System.Web.HttpException($"{this.GetType().Name}.{new StackTrace().GetFrame(0).GetMethod().Name}: Status code: {(int)response.StatusCode}, description: {response.StatusDescription}");
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
                    throw;
                }
            }
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