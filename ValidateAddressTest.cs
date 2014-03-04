namespace AvaTaxCalcREST
{
    using System;

    public class ValidateAddressTest
    {
        public static void Test()
        {
            //Header Level Elements
            //Required Header Level Elements
            string accountNumber = "1234567890";
            string licenseKey = "A1B2C3D4E5F6G7H8";
            string serviceURL = "https://development.avalara.net";

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
