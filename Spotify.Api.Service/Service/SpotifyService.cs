using Newtonsoft.Json;
using RestSharp;
using Spotify.Api.Log.SpotifyLogger;
using Spotify.Api.Model.Json;
using Spotify.Api.Model.Json.Search;
using Spotify.Api.Model.Json.TopTracks;
using Spotify.Api.Models.Constants;
using Spotify.Api.Service.Contracts;
using Spotify.Api.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.Caching;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Web;

namespace Spotify.Api.Service.Service
{
    public class SpotifyService : ISpotifyService
    {
        private RestClient _restClient;
        private readonly string AuthenticationEndpoint = ConfigurationManager.AppSettings["SpotifyAuthTokenUrl"];
        private readonly string ClientId = ConfigurationManager.AppSettings["SpotifyClientId"];
        private readonly string ClientSecret = ConfigurationManager.AppSettings["SpotifyClientSecret"];

        public SpotifyService(RestClient restClient)
        {
            this._restClient = restClient;
        }

        /// <summary>
        /// Request data from the Spotify search Api by query and type (artist, track or genre)
        /// </summary>
        /// <param name="query">search query</param>
        /// <param name="type">artist, genre, tracks</param>
        /// <returns>Task</returns>
        public async Task<Recommendation> SearchAsync(string query, string type)
        {
            string token = await GetAuthenticationTokenAsync().ConfigureAwait(false);
            RestRequest request = RequestBuilder.CreateSearchRequest(query, type, token);
            
            IRestResponse response = await ExecuteRequestAsync(request).ConfigureAwait(false);
            Search search = DeserializeResponse(response.Content, new Search());
            
            Recommendation recommendation = null;

            bool foundResult = Util.FoundResult(type, search);

            if (!foundResult)
            {
                return new Recommendation(type, query);
            }
            
            if (type == Constants.GENRES)
            {
                List<string> genres = Util.PickRandomGenres(search);
                recommendation = await GetRecommendationsAsync(search, type, genres).ConfigureAwait(false);
                recommendation.Genres = genres;
            }
            else
            {
                recommendation = await GetRecommendationsAsync(search, type).ConfigureAwait(false);
            }

            recommendation.SeedType = type;
            recommendation.Message = query;

            return recommendation;

        }

        /// <summary>
        /// Request recommendation data based on search result and user selected type
        /// </summary>
        /// <param name="model">Search model</param>
        /// <param name="type">the type of recommendation requested</param>
        /// <param name="genres">list of genres for the artist</param>
        /// <returns>Task</returns>
        public async Task<Recommendation> GetRecommendationsAsync(Search model, string type, List<string> genres = null)
        {
            string token = await GetAuthenticationTokenAsync().ConfigureAwait(false);
            RestRequest request = RequestBuilder.CreateRecommendationRequest(token, model, genres, type);
            
            IRestResponse response = await ExecuteRequestAsync(request).ConfigureAwait(false);

            return DeserializeResponse(response.Content, new Recommendation());
        }

        /// <summary>
        /// Executes a request and returns the response
        /// </summary>
        /// <param name="request">Request to be executed</param>
        /// <returns>Task<IRestResponse></returns>
        private async Task<IRestResponse> ExecuteRequestAsync(RestRequest request)
        {
            IRestResponse response = null;
            try
            {
                response = await _restClient.ExecuteAsync(request).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                SpotifyLogger.WriteExceptionToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, e);
                throw new HttpException(500, e.Message);
            }

            if (response.ErrorException != null)
            {
                SpotifyLogger.WriteExceptionToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, response.ErrorException);
                throw new HttpException((int)response.StatusCode, response.ErrorException.Message);
            }
            else if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                SpotifyLogger.WriteMessageToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, $"Status code: {(int)response.StatusCode}, description: {response.StatusDescription}");
                throw new HttpException((int)response.StatusCode, response.StatusDescription);
            }

            return response;
        }


        /// <summary>
        /// Returns list of top tracks for the specified artist-/spotify id in the given country
        /// </summary>
        /// <param name="spotifyId">id of the artist</param>
        /// <param name="countryCode">top tracks from this country</param>
        /// <returns></returns>
        public async Task<TopTracks> TopTracksAsync(string spotifyId, string countryCode)
        {
            string token = await GetAuthenticationTokenAsync().ConfigureAwait(false);
            RestRequest request = RequestBuilder.CreateTopTracksRequest(spotifyId, countryCode, token);

            IRestResponse response = await ExecuteRequestAsync(request).ConfigureAwait(false);
            TopTracks topTracks = DeserializeResponse(response.Content, new TopTracks());

            return topTracks;
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
                var response = await GetAuthenticationTokenResponseAsync().ConfigureAwait(false);
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
            RestRequest request = RequestBuilder.CreateAuthTokenRequest(ClientId, ClientSecret);

            try
            {
                IRestResponse response = await restclient.ExecuteAsync<AuthenticationResponse>(request).ConfigureAwait(false);
                var authenticationResponse = DeserializeResponse(response.Content, new AuthenticationResponse());
                return authenticationResponse;
            }
            catch (Exception e)
            {
                SpotifyLogger.WriteExceptionToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, e);
                throw;
            }
        }

        /// <summary>
        /// Deserialize json string and return an instance of objectType
        /// </summary>
        /// <param name="json">json string</param>
        /// <param name="objectType">deserialize json to an instance of this type </param>
        /// <returns></returns>
        private T DeserializeResponse<T>(string json, T objectType)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
