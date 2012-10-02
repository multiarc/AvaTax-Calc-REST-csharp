using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AvaTaxCalcREST
{
    class DocumentLoader
    {
        //This loads the invoice (or return) located at the specified path and returns a GetTaxRequest object for tax calculation.
        public static GetTaxRequest Load(string DocPath)
        {
            GetTaxRequest req = new GetTaxRequest();
            //loads the invoice file
            string[] txtInv = File.ReadAllLines(@DocPath);

            //Parses header-level data from the invoice file
            req.DocCode = txtInv[0].Split(':')[1] + DateTime.Now.ToString();
            req.CustomerCode = txtInv[1].Split(':')[1];
            req.DocDate = "2012-07-07";//txtInv[3].Split(':')[1];
            req.DocType = DocType.SalesInvoice;

            string[] shipto = txtInv[10].Split(':')[1].Split(',');

            req.Addresses = new Address[2]; //We will need to pass in two addresses, our origin and destination.
            //Parse our destiniation address
            Address dest = new Address();
            dest.AddressCode = "01";
            dest.Line1 = shipto[0];
            dest.City = shipto[1];
            dest.Region = shipto[2];
            dest.PostalCode = shipto[3];


            //Add the address to our request object.
            req.Addresses[0] = new Address();
            req.Addresses[0] = dest;
            //Hardcodes the origin address for the GetTaxRequest. This should be your warehouse or company address, and should not be hardcoded.
            req.Addresses[1] = new Address();
            req.Addresses[1].AddressCode = "02";
            req.Addresses[1].Line1 = "PO Box 123";
            req.Addresses[1].City = "Bainbridge Island";
            req.Addresses[1].Region = "WA";
            req.Addresses[1].PostalCode = "98110";

            //create array of line items
            req.Lines = new Line[txtInv.Length - 12];

            //Iterate through line items on transaction and add them to the request
            for (int i = 1; txtInv.Length > 12 + i; i++)
            {
                string[] item = txtInv[12 + i].Split(',');
                req.Lines[i] = new Line();
                req.Lines[i].LineNo = item[0];
                req.Lines[i].ItemCode = item[1];
                req.Lines[i].Qty = Convert.ToDecimal(item[3]);
                req.Lines[i].Amount = Convert.ToDecimal(item[4]) * req.Lines[i].Qty;
                req.Lines[i].OriginCode = "02";
                req.Lines[i].DestinationCode = "01";
            }

            //Pull the freight line from the header information and add to the request as an additional line item
            req.Lines[0] = new Line();
            req.Lines[0].ItemCode = "Shipping";
            req.Lines[0].Qty = 1;
            req.Lines[0].LineNo = "FR";
            req.Lines[0].Amount = Convert.ToDecimal(txtInv[7].Split(':')[1]);
            req.Lines[0].OriginCode = "02";
            req.Lines[0].DestinationCode = "01";
            return req;
        }
    }
}
