namespace AvaTaxCalcREST
{
    using System;
    using System.Configuration;
    using System.Linq;

    public class EstimateTaxTest
    {
        public static void Test()
        {
            // Header Level Elements
            // Required Header Level Elements
            string accountNumber = ConfigurationManager.AppSettings["AvaTax:AccountNumber"];
            string licenseKey = ConfigurationManager.AppSettings["AvaTax:LicenseKey"];
            string serviceURL = ConfigurationManager.AppSettings["AvaTax:ServiceUrl"];

            TaxSvc taxSvc = new TaxSvc(accountNumber, licenseKey, serviceURL);

            // Required Request Parameters
            decimal latitude = (decimal)47.627935;
            decimal longitude = (decimal)-122.51702;
            decimal saleAmount = (decimal)10;

            GeoTaxResult geoTaxResult = taxSvc.EstimateTax(latitude, longitude, saleAmount);

            // Print results
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