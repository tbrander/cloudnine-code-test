using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spotify.Api.Models.Constants
{
    public static class Constants
    {
        public const string ARTIST = "Artist";
        public const string TRACK = "Track";
        public const string GENRES = "Genres";
        /// <summary>
        /// Maximum number of seed values to be passed to the recommendation endpoint
        /// </summary>
        public const int MAX_SEED_VALUES = 5;
    }
}