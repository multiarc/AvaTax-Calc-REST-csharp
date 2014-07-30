namespace AvaTaxCalcREST
{
    using System;
    using System.Configuration;
    using System.Linq;

    public class GetTaxTest
    {
        public static void Test()
        {
            // Header Level Elements
            // Required Header Level Elements
            string accountNumber = ConfigurationManager.AppSettings["AvaTax:AccountNumber"];
            string licenseKey = ConfigurationManager.AppSettings["AvaTax:LicenseKey"];
            string serviceURL = ConfigurationManager.AppSettings["AvaTax:ServiceUrl"];

            TaxSvc taxSvc = new TaxSvc(accountNumber, licenseKey, serviceURL);

            GetTaxRequest getTaxRequest = new GetTaxRequest();

            // Document Level Elements
            // Required Request Parameters
            getTaxRequest.CustomerCode = "ABC4335";
            getTaxRequest.DocDate = "2014-01-01";

            // Best Practice Request Parameters
            getTaxRequest.CompanyCode = "APITrialCompany";
            getTaxRequest.Client = "AvaTaxSample";
            getTaxRequest.DocCode = "INV001";
            getTaxRequest.DetailLevel = DetailLevel.Tax;
            getTaxRequest.Commit = false;
            getTaxRequest.DocType = DocType.SalesInvoice;

            // Situational Request Parameters
            // getTaxRequest.CustomerUsageType = "G";
            // getTaxRequest.ExemptionNo = "12345";
            // getTaxRequest.Discount = 50;
            // getTaxRequest.TaxOverride = new TaxOverrideDef();
            // getTaxRequest.TaxOverride.TaxOverrideType = "TaxDate";
            // getTaxRequest.TaxOverride.Reason = "Adjustment for return";
            // getTaxRequest.TaxOverride.TaxDate = "2013-07-01";
            // getTaxRequest.TaxOverride.TaxAmount = "0";

            // Optional Request Parameters
            getTaxRequest.PurchaseOrderNo = "PO123456";
            getTaxRequest.ReferenceCode = "ref123456";
            getTaxRequest.PosLaneCode = "09";
            getTaxRequest.CurrencyCode = "USD";

            // Address Data
            Address address1 = new Address();
            address1.AddressCode = "01";
            address1.Line1 = "45 Fremont Street";
            address1.City = "San Francisco";
            address1.Region = "CA";

            Address address2 = new Address();
            address2.AddressCode = "02";
            address2.Line1 = "118 N Clark St";
            address2.Line2 = "Suite 100";
            address2.Line3 = "ATTN Accounts Payable";
            address2.City = "Chicago";
            address2.Region = "IL";
            address2.Country = "US";
            address2.PostalCode = "60602";

            Address address3 = new Address();
            address3.AddressCode = "03";
            address3.Latitude = (decimal)47.627935;
            address3.Longitude = (decimal)-122.51702;
            Address[] addresses = { address1, address2, address3 };
            getTaxRequest.Addresses = addresses;

            // Line Data
            // Required Parameters
            Line line1 = new Line();
            line1.LineNo = "01";
            line1.ItemCode = "N543";
            line1.Qty = 1;
            line1.Amount = 10;
            line1.OriginCode = "01";
            line1.DestinationCode = "02";

            // Best Practice Request Parameters
            line1.Description = "Red Size 7 Widget";
            line1.TaxCode = "NT";

            // Situational Request Parameters
            // line1.CustomerUsageType = "L";
            // line1.Discounted = true;
            // line1.TaxIncluded = true;
            // line1.TaxOverride = new TaxOverrideDef();
            // line1.TaxOverride.TaxOverrideType = "TaxDate";
            // line1.TaxOverride.Reason = "Adjustment for return";
            // line1.TaxOverride.TaxDate = "2013-07-01";
            // line1.TaxOverride.TaxAmount = "0";

            // Optional Request Parameters
            line1.Ref1 = "ref123";
            line1.Ref2 = "ref456";

            Line line2 = new Line();
            line2.LineNo = "02";
            line2.ItemCode = "T345";
            line2.Qty = 3;
            line2.Amount = 150;
            line2.OriginCode = "01";
            line2.DestinationCode = "03";
            line2.Description = "Size 10 Green Running Shoe";
            line2.TaxCode = "PC030147";

            Line line3 = new Line();
            line3.LineNo = "02-FR";
            line3.ItemCode = "FREIGHT";
            line3.Qty = 1;
            line3.Amount = 15;
            line3.OriginCode = "01";
            line3.DestinationCode = "03";
            line3.Description = "Shipping Charge";
            line3.TaxCode = "FR";
            Line[] lines = { line1, line2, line3 };
            getTaxRequest.Lines = lines;

            GetTaxResult getTaxResult = taxSvc.GetTax(getTaxRequest);
            
            // Print results
            Console.WriteLine("GetTaxTest Result: " + getTaxResult.ResultCode.ToString());
            if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
            { 
                foreach (Message message in getTaxResult.Messages)
                {
                    Console.WriteLine(message.Summary);
                }
            }
            else
            {
                Console.WriteLine("Document Code: " + getTaxResult.DocCode + " Total Tax: " + getTaxResult.TotalTax);
                foreach (TaxLine taxLine in getTaxResult.TaxLines ?? Enumerable.Empty<TaxLine>())
                {
                    Console.WriteLine("    " + "Line Number: " + taxLine.LineNo + " Line Tax: " + taxLine.Tax.ToString());
                    foreach (TaxDetail taxDetail in taxLine.TaxDetails ?? Enumerable.Empty<TaxDetail>())
                    {
                        Console.WriteLine("        " + "Jurisdiction: " + taxDetail.JurisName + "Tax: " + taxDetail.Tax.ToString());
                    }
                }
            }
        }
    }
}
