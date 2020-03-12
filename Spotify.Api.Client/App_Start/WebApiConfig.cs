using Microsoft.Web.Http.Routing;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Spotify.Api.Client
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Web API routes
            var constraintResolver = new DefaultInlineConstraintResolver()
            {
                ConstraintMap =
                {
                    ["apiVersion"] = typeof( ApiVersionRouteConstraint )
                }
            };

            config.MapHttpAttributeRoutes(constraintResolver);
            config.AddApiVersioning();
        
            // - Add additional default supported media types
            config.Formatters.Clear();

            // JSON
            JsonMediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
            // Default to JSON in browser
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // Support default XML serializer
            config.Formatters.Add(jsonFormatter);

            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None;

        }

    }
}
