namespace AvaTaxCalcREST
{
    using System;
    using System.Configuration;

    public class ValidateAddressTest
    {
        public static void Test()
        {
            // Header Level Elements
            // Required Header Level Elements
            string accountNumber = ConfigurationManager.AppSettings["AvaTax:AccountNumber"];
            string licenseKey = ConfigurationManager.AppSettings["AvaTax:LicenseKey"];
            string serviceURL = ConfigurationManager.AppSettings["AvaTax:ServiceUrl"];

            AddressSvc addressSvc = new AddressSvc(accountNumber, licenseKey, serviceURL);

            Address address = new Address();

            // Required Request Parameters
            address.Line1 = "118 N Clark St";
            address.City = "Chicago";
            address.Region = "IL";

            // Optional Request Parameters
            address.Line2 = "Suite 100";
            address.Line3 = "ATTN Accounts Payable";
            address.Country = "US";
            address.PostalCode = "60602";

            ValidateResult validateResult = addressSvc.Validate(address);

            // Print results
            Console.WriteLine("ValidateAddressTest Result: " + validateResult.ResultCode.ToString());
            if (!validateResult.ResultCode.Equals(SeverityLevel.Success))
            {
                foreach (Message message in validateResult.Messages)
                {
                    Console.WriteLine(message.Summary);
                }
            }
            else
            {
                Console.WriteLine(validateResult.Address.Line1 
                    + " " 
                    + validateResult.Address.City 
                    + ", "
                    + validateResult.Address.Region 
                    + " " 
                    + validateResult.Address.PostalCode);
            }
        }
    }
}
