using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using static Spotify.Api.Log.LogModel.Enums;

namespace Spotify.Api.Log.SpotifyLogger
{
    public static class SpotifyLogger
    {
        public static void WriteMessageToLog(string className, string method, string logMessage, string extendedMessage = null)
        {
            LogModel.Log log = new LogModel.Log()
            {
                EventSubView = method,
                LogEventType = LogEvent.Info,
                MetaData = new Dictionary<string, string> { { $"{className}.{method}", logMessage } },
            };

            if (!string.IsNullOrEmpty(extendedMessage))
            {
                log.MetaData.Add("Extended message", extendedMessage);
            }

            NLogger.LogToFile(log);
        }

        public static void WriteExceptionToLog(string className, string method, Exception ex)
        {
            LogModel.Log log = new LogModel.Log()
            {
                EventSubView = method,
                LogEventType = LogEvent.Warn,
                MetaData = new Dictionary<string, string> { { $"{className}.{method}", ex.ToString() } },
            };

            if (ex.InnerException != null)
            {
                log.MetaData.Add("InnerException", ex.InnerException.ToString());
            }

            NLogger.LogToFile(log);
        }
    }

    public class NLogger
    {
        private static Logger Logger => LogManager.GetLogger("SpotifyApiLog");

        public static void LogToFile(LogModel.Log log)
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