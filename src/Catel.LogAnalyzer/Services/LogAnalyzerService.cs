// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogAnalyzerService.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Catel.LogAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Logging;
    using Models;

    public class LogAnalyzerService : ILogAnalyzerService
    {
        #region ILogAnalyzerService Members
        public IEnumerable<LogEntry> Parse(LogFilter logFilter, string text)
        {
            Argument.IsNotNull(() => logFilter);
            Argument.IsNotNullOrWhitespace(() => text);

            var stringReader = new StringReader(text);

            var entries = new List<LogEntry>();

            string line;

            while ((line = stringReader.ReadLine()) != null)
            {
                try
                {
                    var timeString = line.Substring(0, 12);

                    var logEventType = line.Contains("DEBUG") ? "DEBUG" : line.Contains("INFO") ? "INFO" : line.Contains("ERROR") ? "ERROR" : line.Contains("WARNING") ? "WARNING" : string.Empty;

                    var message = line.Substring((line.IndexOf(logEventType, StringComparison.Ordinal) + (logEventType.Length + 1))).Trim();

                    var exactDate = DateTime.ParseExact(timeString, new[] {"hh:mm:ss:fff"}, new CultureInfo("en-US"), DateTimeStyles.None);

                    var time = new TimeSpan(0, exactDate.Hour, exactDate.Minute, exactDate.Second, exactDate.Millisecond);

                    var logEventValue = TagHelper.AreTagsEqual(logEventType, "DEBUG") ? LogEvent.Debug : TagHelper.AreTagsEqual(logEventType, "INFO") ? LogEvent.Info : TagHelper.AreTagsEqual(logEventType, "WARNING") ? LogEvent.Warning : TagHelper.AreTagsEqual(logEventType, "ERROR") ? LogEvent.Error : 0;

                    var logEntry = new LogEntry {Time = time, LogEvent = logEventValue, Message = message};

                    if ((logEntry.LogEvent != LogEvent.Debug || !logFilter.EnableDebug) && (logEntry.LogEvent != LogEvent.Error || !logFilter.EnableError) && (logEntry.LogEvent != LogEvent.Info || !logFilter.EnableInfo) && (logEntry.LogEvent != LogEvent.Warning || !logFilter.EnableWarning))
                    {
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(logFilter.Filter) && !logEntry.Message.ToLower().Contains(logFilter.Filter.ToLower()))
                    {
                        continue;
                    }

                    entries.Add(logEntry);
                }
                catch (Exception exception)
                {
                    // TODO: Swallow, or prepend to previous one?
                }
            }

            return entries;
        }

        public IEnumerable<LogEntry> GetSlowestMethods(IEnumerable<LogEntry> entries, int maximumItemsToReturn)
        {
            return Filter(entries, maximumItemsToReturn, orderByDescending: x => x.Duration);
        }

        public IEnumerable<LogEntry> Filter(IEnumerable<LogEntry> entries, int maximumItemsToReturn = -1, Func<LogEntry, bool> filter = null, Func<LogEntry, object> orderBy = null, Func<LogEntry, object> orderByDescending = null)
        {
            Argument.IsNotNull("entries", entries);

            var query = new List<LogEntry>(entries);

            if (filter != null)
            {
                query = query.Where(filter).ToList();
            }

            if (orderBy != null)
            {
                query = query.OrderBy(orderBy).ToList();
            }

            if (orderByDescending != null)
            {
                query = query.OrderByDescending(orderByDescending).ToList();
            }

            if (maximumItemsToReturn > 0)
            {
                query = query.Take(maximumItemsToReturn).ToList();
            }

            return query;
        }
        #endregion
    }
}