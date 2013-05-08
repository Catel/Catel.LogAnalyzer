using System;
using Catel.Data;
using Catel.Logging;

namespace Catel.LogAnalyzer
{
    public class LogEntry : ModelBase
    {
        // Compared to previous log message
        public TimeSpan Duration { get; set; }

        public TimeSpan Time { get; set; }

        public LogEvent LogEvent { get; set; }

        public string Message { get; set; }
    }
}