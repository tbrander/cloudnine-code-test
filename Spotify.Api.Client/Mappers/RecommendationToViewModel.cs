using AutoMapper;
using Spotify.Api.Model.Json;
using Spotify.Api.Client.Models.RecommendationView;
using System.Linq;

namespace Spotify.Api.Client.Mappers
{
    public class RecommendationToViewModel : ITypeConverter<Recommendation, RecommendationViewModel>
    {
        public RecommendationViewModel Convert(Recommendation source, RecommendationViewModel destination, ResolutionContext context)
        {
            return new RecommendationViewModel()
            {
                Genres = source.Genres,
                Type = source.SeedType,
                Message = source.Message,
                Tracks = source.Tracks?.Select(t => new Models.RecommendationView.Track()
                {
                    Artists = t.Artists.Select(a => new Models.RecommendationView.Artist()
                    {
                        ExternalUrls = new Models.RecommendationView.External_Urls1()
                        {
                            Spotify = a.ExternalUrls.Spotify
                        },
                        Id = a.Id,
                        Name = a.Name,
                        Type = a.Type
                    }).ToList(),
                    Duration_ms = t.Duration_ms,
                    ExternalUrls = new Models.RecommendationView.External_Urls()
                    {
                        Spotify = t.ExternalUrls.Spotify
                    },
                    Name = t.Name,
                    Type = t.Type,
                    Preview_url = t.Preview_url
                }).ToList()
            };
        }
    }
}