using System;

namespace Avalara.Avatax.Rest.Data
{
#if !DNXCORE50
    [Serializable]
#endif
    public class CancelTaxResponse
    {
        public CancelTaxResult CancelTaxResult { get; set; }

        public SeverityLevel ResultCode { get; set; }

        public Message[] Messages { get; set; }
    }
}
