using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvaTaxCalcREST
{
    class EstimateTaxTest
    {
        public static void Test()
        {
            string accountNumber = "1234567890";
            string licenseKey = "A1B2C3D4E5F6G7H8";
            string serviceURL = "https://development.avalara.net";

            //Required Request Parameters
            decimal latitude = (decimal) 47.627935;
            decimal longitude = (decimal)-122.51702;
            decimal saleAmount = (decimal)10;

            TaxSvc taxSvc = new TaxSvc(accountNumber, licenseKey, serviceURL);
            GeoTaxResult geoTaxResult = taxSvc.EstimateTax(latitude, longitude, saleAmount);

            //Print results
            Console.WriteLine("EstimateTaxTest Result: " + geoTaxResult.ResultCode.ToString());
            if (!geoTaxResult.ResultCode.Equals(SeverityLevel.Success))
            {
                foreach (Message message in geoTaxResult.Messages)
                {
                    Console.WriteLine(message.Summary);
                }
            }
            else
            {
                Console.WriteLine("Total Rate: " + geoTaxResult.Rate + " Total Tax: " + geoTaxResult.Tax);

                foreach (TaxDetail taxDetail in geoTaxResult.TaxDetails ?? Enumerable.Empty<TaxDetail>())
                {
                    Console.WriteLine("    " + "Jurisdiction: " + taxDetail.JurisName + " Tax: " + taxDetail.Tax.ToString());
                }
                
            }
            

        }
    }
}
