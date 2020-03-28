using System;
using System.Collections.Generic;
using AutoMapper;
using Spotify.Api.Client.Mappers;
using Spotify.Api.Model.Json;
using Spotify.Api.Model.Json.TopTracks;
using Spotify.Api.Client.Models;
using Spotify.Api.Client.Models.RecommendationView;

namespace Spotify.Client.Mappers
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
}
