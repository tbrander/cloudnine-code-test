﻿using LightInject;
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
            
            ServiceContainer serviceContainer = LightInjectConfig.RegisterDefaultService(baseUrl);
            serviceContainer.RegisterControllers();
            serviceContainer.EnableMvc();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
