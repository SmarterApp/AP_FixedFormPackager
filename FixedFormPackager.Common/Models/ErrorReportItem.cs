using NLog;

namespace FixedFormPackager.Common.Models
{
    public class ErrorReportItem
    {
        public LogLevel Severity { get; set; }
        public string Location { get; set; }
    }
}