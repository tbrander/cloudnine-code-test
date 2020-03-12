using NLog;
using System.Collections.Generic;
using System.Text;

namespace Spotify.Api.Client.Models.LogModels
{
    public class NLogger
    {
        private static Logger Logger => LogManager.GetLogger("SpotifyApiLogger");

        public static void LogToFile(Log log)
        {
            LogEventInfo logEvent = new LogEventInfo(LogLevel.FromString(log.LogEventType.ToString()), log.Name, "");
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> entry in log.MetaData)
            {
                sb.Append($"{entry.Key}:{entry.Value}");
            }

            logEvent.Message = sb.ToString();
            Logger.Log(logEvent);
        }
    }
}