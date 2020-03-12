using System.Collections.Generic;
using static Spotify.Api.Client.Models.LogModels.Enums;

namespace Spotify.Api.Client.Models.LogModels
{ 
    public class Log
    {
        public string EventSubView { get; set; }
        public Dictionary<string, string> MetaData { get; set; }
        public LogEvent LogEventType { get; set; }

        public string Name => "SpotifyApiLogger";

        public Log()
        {
            MetaData = new Dictionary<string, string>();
        }
    }
}