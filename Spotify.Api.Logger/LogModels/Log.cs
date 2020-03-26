using System.Collections.Generic;
using static Spotify.Api.Log.LogModel.Enums;

namespace Spotify.Api.Log.LogModel
{ 
    public class Log
    {
        public string EventSubView { get; set; }
        public Dictionary<string, string> MetaData { get; set; }
        public LogEvent LogEventType { get; set; }

        public string Name => "SpotifyApiLog";

        public Log()
        {
            MetaData = new Dictionary<string, string>();
        }
    }
}