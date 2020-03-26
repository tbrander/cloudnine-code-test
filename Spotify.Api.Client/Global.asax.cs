using LightInject;
using Spotify.Api.Web;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Spotify.Api.Client
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            string baseUrl = $"{ConfigurationManager.AppSettings["SpotifyApiBaseUrl"]}/";
            string clientId = ConfigurationManager.AppSettings["SpotifyClientId"];
            string clientSecret = ConfigurationManager.AppSettings["SpotifyClientSecret"];
            string authTokenUrl = ConfigurationManager.AppSettings["SpotifyAuthTokenUrl"];

            ServiceContainer serviceContainer = LightInjectConfig.RegisterDefaultService(baseUrl, clientSecret, clientId, authTokenUrl);
            serviceContainer.RegisterControllers();
            serviceContainer.EnableMvc();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
