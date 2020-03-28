using AutoMapper;
using Spotify.Api.Log.SpotifyLogger;
using Spotify.Api.Model.Json.TopTracks;
using Spotify.Api.Client.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Spotify.Api.Client.Mappers
{
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
}