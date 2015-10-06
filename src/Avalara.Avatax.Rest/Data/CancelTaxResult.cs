using System;

namespace Avalara.Avatax.Rest.Data
{
#if !DNXCORE50
    [Serializable]
#endif
    public class CancelTaxResult
    {
        public SeverityLevel ResultCode { get; set; }

        public string TransactionId { get; set; }

        public string DocId { get; set; }

        public Message[] Messages { get; set; }
    }
}
