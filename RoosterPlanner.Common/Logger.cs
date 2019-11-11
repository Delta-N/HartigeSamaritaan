using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace RoosterPlanner.Common
{
    public interface ILogger
    {
        void SetSessionId(string sessionId);

        void Error(Exception ex, string message);

        void LogException(Exception ex, IDictionary<string, string> properties = null);

        void LogEvent(string eventName, string messageKey, string message);

        void LogEvent(string eventName, IDictionary<string, string> properties = null);

        void LogRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success);
    }

    public class Logger : ILogger
    {
        private static TelemetryClient telemetryClient;

        //Constructor => Use Create factory method the get intance
        private Logger(string instrumentationKey)
        {
            if (telemetryClient == null)
            {
                telemetryClient = new TelemetryClient(new TelemetryConfiguration { InstrumentationKey = instrumentationKey });
                //ServerTelemetryChannel.Initialize();
            }
        }

        public static Logger Create(string instrumentationKey)
        {
            if (String.IsNullOrEmpty(instrumentationKey))
                throw new ArgumentNullException("instrumentationKey");

            return new Logger(instrumentationKey);
        }

        public void SetSessionId(string sessionId)
        {
            if (!String.IsNullOrEmpty(sessionId))
                telemetryClient.Context.Session.Id = sessionId;
        }

        #region Logging Methods
        public void Error(Exception ex, string message)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("Message", message);
            properties.Add("Source", ex.Source);
            properties.Add("StackTrace", ex.StackTrace);
            properties.Add("BaseError", ex.GetBaseException().Message ?? String.Empty);
            if (ex.Data != null)
            {
                foreach (KeyValuePair<object, object> item in ex.Data)
                    properties.Add(item.Key.ToString(), item.Value.ToString());
            }

            if (ex == null)
                telemetryClient.TrackTrace(message, SeverityLevel.Error, properties);
            else
                telemetryClient.TrackException(ex, properties);
        }

        public void Information(string message, IDictionary<string, string> properties = null)
        {
            telemetryClient.TrackTrace(message, SeverityLevel.Information, properties);
        }

        public void LogException(Exception ex, IDictionary<string, string> properties = null)
        {
            telemetryClient.TrackException(ex, properties);
        }

        public void LogEvent(string eventName, string messageKey, string message)
        {
            LogEvent(eventName, new Dictionary<string, string> { { messageKey, message } });
        }

        public void LogEvent(string eventName, IDictionary<string, string> properties = null)
        {
            telemetryClient.TrackEvent(eventName, properties);
        }

        public void LogRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success)
        {
            telemetryClient.TrackRequest(name, startTime, duration, responseCode, success);
        }
        #endregion
    }
}
