using RestSharp;
using Spotify.Api.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Api.Service.Utilities
{
    public static class RequestBuilder
    {
        public static RestRequest CreateSearchRequest(string query, string type, string token)
        {
            RestRequest request = new RestRequest();
            request.Resource = "search";
            request.AddQueryParameter("q", query);
            request.AddHeader("Authorization", $"Bearer {token}");

            switch (type)
            {
                case Constants.ARTIST:
                case Constants.GENRES:
                    request.AddQueryParameter("type", "artist"); break;
                case Constants.TRACK:
                    request.AddQueryParameter("type", "track"); break;
            }

            return request;
        }

        public static RestRequest CreateRecommendationRequest(string token, Model.Json.Search.Search model, List<string> genres, string type)
        {
            RestRequest request = new RestRequest("recommendations");
            Dictionary<string, string> seeds = Util.ExtractSeeds(model, type, genres);

            // adding seed parameter
            foreach (KeyValuePair<string, string> entry in seeds)
            {
                request.AddQueryParameter(entry.Key, entry.Value);
            }
            request.AddHeader("Authorization", $"Bearer {token}");

            return request;
        }

        public static RestRequest CreateTopTracksRequest(string spotifyId, string countryCode, string token)
        {
            RestRequest request = new RestRequest($"artists/{spotifyId}/top-tracks");
            request.AddQueryParameter("country", countryCode);
            request.AddHeader("Authorization", $"Bearer {token}");

            return request;
        }

        public static RestRequest CreateAuthTokenRequest(string clientId, string clientSecret)
        {
            RestRequest request = new RestRequest() { Method = Method.POST };
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("Authorization", $"Basic {Util.BuildAuthHeader(clientId, clientSecret)}", ParameterType.HttpHeader);

            return request;
        }
    }
}
