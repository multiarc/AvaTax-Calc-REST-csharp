﻿using System;

namespace Avalara.Avatax.Rest.Data
{
#if !DNXCORE50
    [Serializable]
#endif
    public class TaxDetail // Result object
    {
        public decimal Rate { get; set; }

        public decimal Tax { get; set; }

        public decimal Taxable { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string JurisType { get; set; }

        public string JurisName { get; set; }

        public string TaxName { get; set; }
    }
}
