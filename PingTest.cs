namespace AvaTaxCalcREST
{
    using System;

    public class PingTest
    {
        public static void Test()
        {
            //Header Level Elements
            //Required Header Level Elements
            string accountNumber = "1234567890";
            string licenseKey = "A1B2C3D4E5F6G7H8";
            string serviceURL = "https://development.avalara.net";

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
