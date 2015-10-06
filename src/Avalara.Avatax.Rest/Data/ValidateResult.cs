using System;

namespace Avalara.Avatax.Rest.Data
{
    // Request for address/validate is parsed into the URI query parameters.
#if !DNXCORE50
    [Serializable]
#endif
    public class ValidateResult
    {
        public Address Address { get; set; }

        public SeverityLevel ResultCode { get; set; }

        public Message[] Messages { get; set; }
    }
}
