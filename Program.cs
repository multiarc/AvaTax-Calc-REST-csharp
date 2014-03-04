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
        static void Main()
        {
            try
            {
                //Each test is managed in its own class 
                //Make sure you enter your valid credentials in that test class.
                GetTaxTest.Test();
                CancelTaxTest.Test();
                EstimateTaxTest.Test();
                PingTest.Test();
                ValidateAddressTest.Test();
            }
            catch (Exception ex)
            { Console.WriteLine("An Exception Occured: " + ex.Message); }
            finally
            {
                Console.WriteLine("Done");
                Console.ReadLine();
            }
        }        
    }
}
