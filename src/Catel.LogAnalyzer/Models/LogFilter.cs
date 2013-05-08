using Catel.Data;

namespace Catel.LogAnalyzer.Models
{
    public class LogFilter : ModelBase
    {
        public bool EnableDebug { get; set; }

        public bool EnableInfo { get; set; }

        public bool EnableWarning { get; set; }

        public bool EnableError { get; set; }

        public string Filter { get; set; }
    }
}
