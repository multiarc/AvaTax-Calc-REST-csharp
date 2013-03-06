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
        public const string AcctNum = ""; //TODO: this should be your Avatax Account Number e.g. 1100012345
        public const string LicKey = ""; //TODO: this should be the license key for the account above, e.g. 23CF4C53939C9725
        public const string CompanyCode = ""; //this should be the company code you set on your Admin Console, e.g. TEST
        public const string webaddr = "https://development.avalara.net/1.0/";

        static void Main()
        {
            string DocPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\INV0001.txt";
            GetTaxRequest calcreq = DocumentLoader.Load(DocPath); //Loads document from file to generate request

            //Run address validation test (address/validate)
            try{
            ValidateResult addressresult = ValidateAddress.Validate(calcreq.Addresses[0], AcctNum, LicKey, CompanyCode, webaddr); //Validates a given address.
            Console.WriteLine("ValidateAddress test result: "+addressresult.ResultCode.ToString()+", Address="
                + addressresult.Address.Line1 +" "+addressresult.Address.City+" "+addressresult.Address.Region+" "+addressresult.Address.PostalCode);//At this point, you would display the validated result to the user for approval, and write it to the customer record.
            }
            catch(Exception ex)
            {Console.WriteLine("ValidateAddress test: Exception "+ex.Message);}

            //Run tax calculation test (tax/get POST)
            try
            {
                GetTaxResult calcresult = GetTax.Get(calcreq, AcctNum, LicKey, CompanyCode, webaddr); //Calculates tax on document
                Console.WriteLine("GetTax test result: "+ calcresult.ResultCode.ToString()+ ", "+
                "TotalTax="+calcresult.TotalTax.ToString()); //At this point, you would write the tax calculated to your database and display to the user.
            }
            catch (Exception ex)
            { Console.WriteLine("GetTax test: Exception " + ex.Message); }

            //Run cancel tax test (tax/cancel)
            try
            {
                CancelTaxResult cancelresult = CancelTax.Cancel(calcreq, AcctNum, LicKey, CompanyCode, webaddr); //Let's void this document to demonstrate tax/cancel
                //You would normally initiate a tax/cancel call upon voiding or deleting the document in your system.
                Console.WriteLine("CancelTax test result: "+cancelresult.ResultCode.ToString()+", Document Voided");
                //Let's display the result of the cancellation. At this point, you would allow your system to complete the delete/void.
            }
            catch (Exception ex)
            { Console.WriteLine("CancelTax test: Exception " + ex.Message); }

            Console.WriteLine("Done");
            Console.ReadLine();
        }




        
    }
}
