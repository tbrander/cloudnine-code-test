using Spotify.Api.Log.SpotifyLogger;
using Spotify.Api.Model.Json.TopTracks;
using Spotify.Api.Web.Models;
using Spotify.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spotify.Api.Web.Extensions
{
    public static class TopTracksExtension
    {
        public static List<TopTrackViewModel> ToViewModel(this TopTracks topTracks)
        {
            List<TopTrackViewModel> mappedModel = null;
            try
            {
                mappedModel = ModelMapper.Mapper.Map<List<TopTrackViewModel>>(topTracks);
            }
            catch (Exception e)
            {
                SpotifyLogger.WriteExceptionToLog("TopTracksExtension", "ToViewModel", e);
                return null;
            }
            return mappedModel;
        }

        public async static Task<List<TopTrackViewModel>> ToViewModel(this Task<TopTracks> topTracks)
        {
            return await Task.FromResult(topTracks.Result.ToViewModel());
        }
    }
}