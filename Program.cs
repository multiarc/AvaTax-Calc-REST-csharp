using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Web;
using System.Net;

namespace AvaTaxCalcREST
{
    class Program
    {
        public const string ACCTNUM = ""; //TODO: this should be your Avatax Account Number e.g. 1100012345
        public const string KEY = ""; //TODO: this should be the license key for the account above, e.g. 23CF4C53939C9725
        public const string COMPANYCODE = ""; //this should be the company code you set on your Admin Console, e.g. TEST
        public const string WEBADDR = "https://development.avalara.net/1.0/";

        static void Main()
        {
            GetTaxRequest calcReq = DocumentLoader.Load(); //Loads document from file to generate request

            //Run address validation test (address/validate)
            try{
            ValidateResult addressResult = ValidateAddress.Validate(calcReq.Addresses[0], ACCTNUM, KEY, COMPANYCODE, WEBADDR); //Validates a given address.
            Console.WriteLine("ValidateAddress test result: "+addressResult.ResultCode.ToString()+", \nAddress="
                + addressResult.Address.Line1 +" "+addressResult.Address.City+" "+addressResult.Address.Region+" "+addressResult.Address.PostalCode);//At this point, you would display the validated result to the user for approval, and write it to the customer record.
            }
            catch(Exception ex)
            {Console.WriteLine("ValidateAddress test: Exception "+ex.Message);}

            //Run tax calculation test (tax/get POST)
            try
            {
                GetTaxResult calcresult = GetTax.Get(calcReq, ACCTNUM, KEY, COMPANYCODE, WEBADDR); //Calculates tax on document
                Console.WriteLine("GetTax test result: "+ calcresult.ResultCode.ToString()+ ", "+
                "TotalTax="+calcresult.TotalTax.ToString()); //At this point, you would write the tax calculated to your database and display to the user.
            }
            catch (Exception ex)
            { Console.WriteLine("GetTax test: Exception " + ex.Message); }

            //Run cancel tax test (tax/cancel)
            try
            {
                CancelTaxResult cancelresult = CancelTax.Cancel(calcReq, ACCTNUM, KEY, COMPANYCODE, WEBADDR); //Let's void this document to demonstrate tax/cancel
                //You would normally initiate a tax/cancel call upon voiding or deleting the document in your system.
                Console.WriteLine("CancelTax test result: "+cancelresult.ResultCode.ToString()+", Document Voided");
                //Let's display the result of the cancellation. At this point, you would allow your system to complete the delete/void.
            }
            catch (Exception ex)
            { Console.WriteLine("CancelTax test: Exception " + ex.Message); }

            Console.WriteLine("Done");
        }        
    }
}
