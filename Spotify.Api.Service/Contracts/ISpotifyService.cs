using Spotify.Api.Model.Json;
using Spotify.Api.Model.Json.Search;
using Spotify.Api.Model.Json.TopTracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Api.Service.Contracts
{
    public interface ISpotifyService
    {
        Task<TopTracks> TopTracksAsync(string spotifyId, string countryCode);
        Task<Recommendation> SearchAsync(string query, string type);
    }
}
