using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spotify.Api.Client.Models;
using Spotify.Api.Client.Models.RecommendationModel;
using Spotify.Api.Client.Models.TopTracks;

namespace Spotify.Api.Client.Helpers
{
    /// <summary>
    /// Helper to create model passed to the view
    /// </summary>
    public static class ViewModelHelper
    {
        /// <summary>
        /// Create ViewModel for the TopTracks View
        /// </summary>
        /// <param name="topTracks">Model representing the json-object response</param>
        /// <returns>List<TopTrackViewModel></returns>
        public static List<TopTrackViewModel> CreateTopTracksViewModel(TopTracks topTracks)
        {
            List<TopTrackViewModel> ttvm = new List<TopTrackViewModel>();
            List<string> albumIds = topTracks.Tracks.Select(x => x.Album.Id).Distinct().ToList();

            try
            {
                foreach (string id in albumIds)
                {
                    var album = topTracks.Tracks.FirstOrDefault(a => a.Album.Id == id)?.Album;
                    if (album == null) { continue; }
                    var albumTrackList = topTracks.Tracks.Where(v => v.Album.Id == id).ToList();

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
            catch (Exception)
            {
                throw;
            }

            return ttvm;
        }

        /// <summary>
        /// Create view model, used in the recommendation view, from the json-object
        /// </summary>
        /// <param name="recommendation">Json-object</param>
        /// <param name="query">Search query</param>
        /// <param name="genres">artist genres</param>
        /// <param name="type">Selected recommendation type</param>
        /// <returns>RecommendationViewModel</returns>
        public static RecommendationViewModel CreateRecommendationViewModel(RecommendationModel recommendation, string query, List<string> genres, string type)
        {
            return new RecommendationViewModel()
            {
                Genres = genres,
                Type = type,
                Message = recommendation.Tracks.Any() ? $"Search string '{query}'" : $"Found no recommendation for '{query}' ",
                Tracks = recommendation.Tracks
            };
        }
    }
}