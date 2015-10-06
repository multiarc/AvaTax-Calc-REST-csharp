using System;

namespace Avalara.Avatax.Rest.Data
{
    public enum SeverityLevel
    {
        Success,
        Warning,
        Error,
        Exception
    }
#if !DNXCORE50
    [Serializable]
#endif
    public class Message // Result object for Common Response Format
    {
        public string Summary { get; set; }

        public string Details { get; set; }

        public string RefersTo { get; set; }

        public SeverityLevel Severity { get; set; }

        public string Source { get; set; }
    }
}
