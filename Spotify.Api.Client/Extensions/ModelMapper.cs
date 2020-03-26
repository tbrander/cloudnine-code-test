using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using Spotify.Api.Log.SpotifyLogger;
using Spotify.Api.Model.Json;
using Spotify.Api.Model.Json.TopTracks;
using Spotify.Api.Web.Models;
using Spotify.Api.Web.Models.RecommendationView;

namespace Spotify.Web.Extensions
{
    public static class ModelMapper
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<MappingProfile>();
            });
            return config.CreateMapper();
        });

        public static IMapper Mapper => Lazy.Value;
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Recommendation, RecommendationViewModel>().ConvertUsing<RecommendationToViewModel>();
            CreateMap<TopTracks, List<TopTrackViewModel>>().ConvertUsing<TopTracksToViewModel>();
        }
    }

    public class TopTracksToViewModel : ITypeConverter<TopTracks, List<TopTrackViewModel>>
    {
        public List<TopTrackViewModel> Convert(TopTracks source, List<TopTrackViewModel> destination, ResolutionContext context)
        {
            List<TopTrackViewModel> ttvm = new List<TopTrackViewModel>();
            List<string> albumIds = source.Tracks.Select(x => x.Album.Id).Distinct().ToList();

            try
            {
                foreach (string id in albumIds)
                {
                    var album = source.Tracks.FirstOrDefault(a => a.Album.Id == id)?.Album;
                    if (album == null) { continue; }
                    var albumTrackList = source.Tracks.Where(v => v.Album.Id == id).ToList();

                    ttvm.Add(new TopTrackViewModel()
                    {
                        AlbumId = album.Id,
                        AlbumName = album.Name,
                        SpotifyUrl = album.ExternalUrls.Spotify,
                        AlbumImageSource = album.Images != null && album.Images.Any() ? album.Images.First().Url : string.Empty,
                        ArtistName = album.Artists != null && album.Artists.Any() ? string.Join(", ", album.Artists.Select(n => n.Name).Distinct()) : string.Empty,
                        TrackList = albumTrackList.Select(c => new TrackViewModel()
                        {
                            TrackId = c.Id,
                            TrackName = c.Name,
                            TrackNumber = c.TrackNumber,
                            Duration = Math.Round((decimal)c.Duration_ms / 1000 / 60, 2)
                        }).OrderBy(i => i.TrackNumber).ToList()
                    });
                }
            }
            catch (Exception e)
            {
                SpotifyLogger.WriteExceptionToLog(this.GetType().Name, new StackTrace().GetFrame(0).GetMethod().Name, e);
                throw;
            }

            return ttvm;
        }
    }

    public class RecommendationToViewModel : ITypeConverter<Recommendation, RecommendationViewModel>
    {
        public RecommendationViewModel Convert(Recommendation source, RecommendationViewModel destination, ResolutionContext context)
        {
            return new RecommendationViewModel()
            {
                Genres = source.Genres,
                Type = source.SeedType,
                Message = source.Message,
                Tracks = source.Tracks?.Select(t => new Api.Web.Models.RecommendationView.Track()
                {
                    Artists = t.Artists.Select(a => new Api.Web.Models.RecommendationView.Artist()
                    {
                        ExternalUrls = new Api.Web.Models.RecommendationView.External_Urls1()
                        {
                            Spotify = a.ExternalUrls.Spotify
                        },
                        Id = a.Id,
                        Name = a.Name,
                        Type = a.Type
                    }).ToList(),
                    Duration_ms = t.Duration_ms,
                    ExternalUrls = new Api.Web.Models.RecommendationView.External_Urls()
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
