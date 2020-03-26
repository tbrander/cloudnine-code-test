﻿using Spotify.Api.Model.Json;
using Spotify.Api.Web.Models.RecommendationView;
using Spotify.Web.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotify.Api.Web.Extensions
{
    /// <summary>
    /// Mapping a recommendation to a view model
    /// </summary>
    public static class RecommendationExtension
    {
        public static RecommendationViewModel ToViewModel(this Recommendation recommendation)
        {
            return ModelMapper.Mapper.Map<RecommendationViewModel>(recommendation);
        }

        public static IEnumerable<RecommendationViewModel> ToViewModels(this IEnumerable<Recommendation> recommendations)
        {
            return recommendations.Select(i => i.ToViewModel());
        }

        public async static Task<RecommendationViewModel> ToViewModel(this Task<Recommendation> recommendation)
        {
            return await Task.FromResult(recommendation.Result.ToViewModel());
        }
    }
}