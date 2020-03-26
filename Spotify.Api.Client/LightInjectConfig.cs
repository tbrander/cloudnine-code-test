using LightInject;
using RestSharp;
using Spotify.Api.Service.Contracts;
using Spotify.Api.Service.Service;
using System.Configuration;

namespace Spotify.Api.Client
{
    public static class LightInjectConfig
    {
        public static ServiceContainer RegisterDefaultService(string baseUrl, string clientSecret, string clientId, string authTokenUrl)
        {
            ServiceContainer container = new ServiceContainer();

            // Services
            container.Register<ISpotifyService, SpotifyService>();

            // http
            string BaseUrl = $"{ConfigurationManager.AppSettings["SpotifyApiBaseUrl"]}/";
            container.Register(factory => new RestClient(BaseUrl), new PerScopeLifetime());
            
            return container;
        }
    }
}