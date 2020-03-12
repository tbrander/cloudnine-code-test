using Spotify.Api.Client.Models.LogModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using static Spotify.Api.Client.Models.LogModels.Enums;

namespace Spotify.Api.Client.Helpers
{
    public static class LogHelper
    {
        public static void WriteMessageToLog(string className, string method, string logMessage, string extendedMessage = null)
        {
            Log log = new Log()
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
            Log log = new Log()
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
}