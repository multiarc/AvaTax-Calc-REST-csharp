namespace AvaTaxCalcREST
{
    using System;

    public class Program
    {
        public static void Main()
        {
            try
            {
                // Each test is managed in its own class 
                // Make sure you enter your valid credentials in that test class.
                GetTaxTest.Test();
                CancelTaxTest.Test();
                EstimateTaxTest.Test();
                PingTest.Test();
                ValidateAddressTest.Test();
            }
            catch (Exception ex)
            { 
                Console.WriteLine("An Exception Occured: " + ex.Message); 
            }
            finally
            {
                Console.WriteLine("Done");
                Console.ReadLine();
            }
        }        
    }
}
