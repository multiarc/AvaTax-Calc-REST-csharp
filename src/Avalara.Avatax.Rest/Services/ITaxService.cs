﻿using System.Threading.Tasks;
using Avalara.Avatax.Rest.Data;

namespace Avalara.Avatax.Rest.Services
{
    public interface ITaxService
    {
        Task<CancelTaxResult> CancelTax(CancelTaxRequest cancelTaxRequest);
        Task<GeoTaxResult> EstimateTax(decimal latitude, decimal longitude, decimal saleAmount);
        Task<GetTaxResult> GetTax(GetTaxRequest req);
        Task<GeoTaxResult> Ping();
    }
}