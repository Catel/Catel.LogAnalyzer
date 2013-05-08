using System;
using System.Collections.Generic;
using Catel.Logging;

namespace Catel.LogAnalyzer
{
    public interface ILogAnalyzerService
    {
        IEnumerable<LogEntry> Parse(LogEvent logEvent, string text);
        IEnumerable<LogEntry> GetSlowestMethods(IEnumerable<LogEntry> entries, int maximumItemsToReturn);

        IEnumerable<LogEntry> Filter(IEnumerable<LogEntry> entries, int maximumItemsToReturn = -1,
                                                     Func<LogEntry, bool> filter = null, Func<LogEntry, object> orderBy = null,
                                                     Func<LogEntry, object> orderByDescending = null);
    }
}
