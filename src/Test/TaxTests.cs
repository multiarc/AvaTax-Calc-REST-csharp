using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Avalara.Avatax.Rest.Data;
using Avalara.Avatax.Rest.Services;
using Xunit;

namespace Avalara.Avatax.Rest.Test
{
    public class TaxTests
    {
        [Fact]
        public async Task CancelTaxTest()
        {
            // Header Level Elements
            // Required Header Level Elements
            var configSection = ConfigurationHelper.GetConfiguration();
            string accountNumber = configSection["accountNumber"];
            string licenseKey = configSection["licenseKey"];
            string serviceUrl = configSection["serviceUrl"];

            ITaxService taxSvc = new TaxService(accountNumber, licenseKey, serviceUrl);

            CancelTaxRequest cancelTaxRequest = new CancelTaxRequest
            {
                CompanyCode = "APITrialCompany",
                DocType = DocType.SalesInvoice,
                DocCode = "INV001",
                CancelCode = CancelCode.DocVoided
            };

            // Required Request Parameters

            CancelTaxResult cancelTaxResult = await taxSvc.CancelTax(cancelTaxRequest);

            // Print results
            Console.WriteLine("CancelTaxTest Result: {0}", cancelTaxResult.ResultCode);
            if (!cancelTaxResult.ResultCode.Equals(SeverityLevel.Success))
            {
                foreach (Message message in cancelTaxResult.Messages)
                {
                    Console.WriteLine(message.Summary);
                }
            }
        }

        [Fact]
        public async Task EstimateTaxTest()
        {
            // Header Level Elements
            // Required Header Level Elements
            var configSection = ConfigurationHelper.GetConfiguration();
            string accountNumber = configSection["accountNumber"];
            string licenseKey = configSection["licenseKey"];
            string serviceUrl = configSection["serviceUrl"];

            ITaxService taxSvc = new TaxService(accountNumber, licenseKey, serviceUrl);

            // Required Request Parameters
            decimal latitude = (decimal) 47.627935;
            decimal longitude = (decimal) -122.51702;
            decimal saleAmount = 10;

            GeoTaxResult geoTaxResult = await taxSvc.EstimateTax(latitude, longitude, saleAmount);

            // Print results
            Console.WriteLine("EstimateTaxTest Result: {0}", geoTaxResult.ResultCode);
            if (!geoTaxResult.ResultCode.Equals(SeverityLevel.Success))
            {
                foreach (Message message in geoTaxResult.Messages)
                {
                    Console.WriteLine(message.Summary);
                }
            }
            else
            {
                Console.WriteLine("Total Rate: {0} Total Tax: {1}", geoTaxResult.Rate, geoTaxResult.Tax);

                foreach (TaxDetail taxDetail in geoTaxResult.TaxDetails ?? Enumerable.Empty<TaxDetail>())
                {
                    Console.WriteLine("    Jurisdiction: {0} Tax: {1}", taxDetail.JurisName,
                        taxDetail.Tax.ToString(CultureInfo.CurrentCulture));
                }
            }
        }

        [Fact]
        public async Task GeoTaxTest()
        {
            // Header Level Elements
            // Required Header Level Elements
            var configSection = ConfigurationHelper.GetConfiguration();
            string accountNumber = configSection["accountNumber"];
            string licenseKey = configSection["licenseKey"];
            string serviceUrl = configSection["serviceUrl"];

            ITaxService taxSvc = new TaxService(accountNumber, licenseKey, serviceUrl);

            GetTaxRequest getTaxRequest = new GetTaxRequest
            {
                CustomerCode = "ABC4335",
                DocDate = "2014-01-01",
                CompanyCode = "APITrialCompany",
                Client = "AvaTaxSample",
                DocCode = "INV001",
                DetailLevel = DetailLevel.Tax,
                Commit = false,
                DocType = DocType.SalesInvoice,
                PurchaseOrderNo = "PO123456",
                ReferenceCode = "ref123456",
                PosLaneCode = "09",
                CurrencyCode = "USD"
            };

            // Document Level Elements
            // Required Request Parameters

            // Best Practice Request Parameters

            // Situational Request Parameters
            // getTaxRequest.CustomerUsageType = "G";
            // getTaxRequest.ExemptionNo = "12345";
            // getTaxRequest.BusinessIdentificationNo = "234243";
            // getTaxRequest.Discount = 50;
            // getTaxRequest.TaxOverride = new TaxOverrideDef();
            // getTaxRequest.TaxOverride.TaxOverrideType = "TaxDate";
            // getTaxRequest.TaxOverride.Reason = "Adjustment for return";
            // getTaxRequest.TaxOverride.TaxDate = "2013-07-01";
            // getTaxRequest.TaxOverride.TaxAmount = "0";

            // Optional Request Parameters

            // Address Data
            Address address1 = new Address
            {
                AddressCode = "01",
                Line1 = "45 Fremont Street",
                City = "San Francisco",
                Region = "CA"
            };

            Address address2 = new Address
            {
                AddressCode = "02",
                Line1 = "118 N Clark St",
                Line2 = "Suite 100",
                Line3 = "ATTN Accounts Payable",
                City = "Chicago",
                Region = "IL",
                Country = "US",
                PostalCode = "60602"
            };

            Address address3 = new Address
            {
                AddressCode = "03",
                Latitude = (decimal) 47.627935,
                Longitude = (decimal) -122.51702
            };
            Address[] addresses = {address1, address2, address3};
            getTaxRequest.Addresses = addresses;

            // Line Data
            // Required Parameters
            Line line1 = new Line
            {
                LineNo = "01",
                ItemCode = "N543",
                Qty = 1,
                Amount = 10,
                OriginCode = "01",
                DestinationCode = "02",
                Description = "Red Size 7 Widget",
                TaxCode = "NT",
                Ref1 = "ref123",
                Ref2 = "ref456"
            };

            // Best Practice Request Parameters

            // Situational Request Parameters
            // line1.CustomerUsageType = "L";
            // line1.Discounted = true;
            // line1.TaxIncluded = true;
            // line1.BusinessIdentificationNo = "234243";
            // line1.TaxOverride = new TaxOverrideDef();
            // line1.TaxOverride.TaxOverrideType = "TaxDate";
            // line1.TaxOverride.Reason = "Adjustment for return";
            // line1.TaxOverride.TaxDate = "2013-07-01";
            // line1.TaxOverride.TaxAmount = "0";

            // Optional Request Parameters

            Line line2 = new Line
            {
                LineNo = "02",
                ItemCode = "T345",
                Qty = 3,
                Amount = 150,
                OriginCode = "01",
                DestinationCode = "03",
                Description = "Size 10 Green Running Shoe",
                TaxCode = "PC030147"
            };

            Line line3 = new Line
            {
                LineNo = "02-FR",
                ItemCode = "FREIGHT",
                Qty = 1,
                Amount = 15,
                OriginCode = "01",
                DestinationCode = "03",
                Description = "Shipping Charge",
                TaxCode = "FR"
            };
            Line[] lines = {line1, line2, line3};
            getTaxRequest.Lines = lines;

            GetTaxResult getTaxResult = await taxSvc.GetTax(getTaxRequest);

            // Print results
            Console.WriteLine("GetTaxTest Result: {0}", getTaxResult.ResultCode);
            if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
            {
                foreach (Message message in getTaxResult.Messages)
                {
                    Console.WriteLine(message.Summary);
                }
            }
            else
            {
                Console.WriteLine("Document Code: {0} Total Tax: {1}", getTaxResult.DocCode, getTaxResult.TotalTax);
                foreach (TaxLine taxLine in getTaxResult.TaxLines ?? Enumerable.Empty<TaxLine>())
                {
                    Console.WriteLine("    Line Number: {0} Line Tax: {1}", taxLine.LineNo,
                        taxLine.Tax.ToString(CultureInfo.CurrentCulture));
                    foreach (TaxDetail taxDetail in taxLine.TaxDetails ?? Enumerable.Empty<TaxDetail>())
                    {
                        Console.WriteLine("        Jurisdiction: {0}Tax: {1}", taxDetail.JurisName,
                            taxDetail.Tax.ToString(CultureInfo.CurrentCulture));
                    }
                }
            }
        }

        [Fact]
        public async Task AddressValidateTest()
        {
            // Header Level Elements
            // Required Header Level Elements
            var configSection = ConfigurationHelper.GetConfiguration();
            string accountNumber = configSection["accountNumber"];
            string licenseKey = configSection["licenseKey"];
            string serviceUrl = configSection["serviceUrl"];

            IAddressService addressSvc = new AddressService(accountNumber, licenseKey, serviceUrl);

            Address address = new Address
            {
                Line1 = "118 N Clark St",
                City = "Chicago",
                Region = "IL",
                Line2 = "Suite 100",
                Line3 = "ATTN Accounts Payable",
                Country = "US",
                PostalCode = "60602"
            };

            // Required Request Parameters

            // Optional Request Parameters

            ValidateResult validateResult = await addressSvc.Validate(address);

            // Print results
            Console.WriteLine("ValidateAddressTest Result: {0}", validateResult.ResultCode);
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

        [Fact]
        public async Task PingTest()
        {
            // Header Level Elements
            // Required Header Level Elements
            var configSection = ConfigurationHelper.GetConfiguration();
            string accountNumber = configSection["accountNumber"];
            string licenseKey = configSection["licenseKey"];
            string serviceUrl = configSection["serviceUrl"];

            ITaxService taxSvc = new TaxService(accountNumber, licenseKey, serviceUrl);

            GeoTaxResult geoTaxResult = await taxSvc.Ping();

            Console.WriteLine("PingTest Result: {0}", geoTaxResult.ResultCode);
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