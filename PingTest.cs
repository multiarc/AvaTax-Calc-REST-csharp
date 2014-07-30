namespace AvaTaxCalcREST
{
    using System;
    using System.Configuration;

    public class PingTest
    {
        public static void Test()
        {
            // Header Level Elements
            // Required Header Level Elements
            string accountNumber = ConfigurationManager.AppSettings["AvaTax:AccountNumber"];
            string licenseKey = ConfigurationManager.AppSettings["AvaTax:LicenseKey"];
            string serviceURL = ConfigurationManager.AppSettings["AvaTax:ServiceUrl"];

            TaxSvc taxSvc = new TaxSvc(accountNumber, licenseKey, serviceURL);

            GeoTaxResult geoTaxResult = taxSvc.Ping();

            Console.WriteLine("PingTest Result: " + geoTaxResult.ResultCode.ToString());
            if (!geoTaxResult.ResultCode.Equals(SeverityLevel.Success))
            {
                foreach (Message message in geoTaxResult.Messages)
                {
                    Console.WriteLine(message.Summary);
                }
            }
        }
    }
}
