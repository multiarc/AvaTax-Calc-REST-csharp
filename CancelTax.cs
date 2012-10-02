using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.Net;
using System.IO;

namespace AvaTaxCalcREST
{
    //These classes are for tax/cancel (verb POST)
    [Serializable]
    public class CancelTaxRequest
    {
        //Required for CancelTax operation
        public CancelCode CancelCode { get; set; }
        public DocType DocType { get; set; } //Note that the only *meaningful* values for this property here are SalesInvoice, ReturnInvoice, PurchaseInvoice.
        //The document needs to be identified by either DocCode/CompanyCode (recommended) OR DocId (not recommended).
        public string CompanyCode { get; set; }
        public string DocCode { get; set; }

    }
    [Serializable]
    public class CancelTaxResponse
    {
        public CancelTaxResult CancelTaxResult { get; set; }
    }

    [Serializable]
    public class CancelTaxResult
    {
        public SeverityLevel ResultCode { get; set; }
        public String TransactionId { get; set; }
        public String DocId { get; set; }
        public Message[] Messages { get; set; }

    }
    public enum CancelCode { Unspecified, PostFailed, DocDeleted, DocVoided, AdjustmentCancelled };
    public class CancelTax
    {

        //This calls CancelTax to void a transaction specified in taxreq
        public static CancelTaxResult Cancel(GetTaxRequest taxreq, string AcctNum, string LicKey, string CompanyCode, string webaddr)
        {
            CancelTaxRequest req = new CancelTaxRequest();
            req.CompanyCode = taxreq.CompanyCode;
            req.DocCode = taxreq.DocCode;
            req.DocType = taxreq.DocType;
            req.CancelCode = CancelCode.DocVoided;

            //Convert the request to XML
            XmlSerializerNamespaces namesp = new XmlSerializerNamespaces();
            namesp.Add("", "");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            XmlSerializer x = new XmlSerializer(req.GetType());
            StringBuilder sb = new StringBuilder();
            x.Serialize(XmlTextWriter.Create(sb, settings), req, namesp);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
            //doc.Save(@"cancel_tax_request.xml");

            //Call the service
            Uri address = new Uri(webaddr + "tax/cancel");
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(AcctNum + ":" + LicKey)));
            request.Method = "POST";
            request.ContentType = "text/xml";
            request.ContentLength = sb.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(ASCIIEncoding.ASCII.GetBytes(sb.ToString()), 0, sb.Length);
            CancelTaxResponse result = new CancelTaxResponse();
            try
            {
                WebResponse response = request.GetResponse();
                XmlSerializer r = new XmlSerializer(result.GetType());
                result = (CancelTaxResponse)r.Deserialize(response.GetResponseStream());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " on canceltax object");
            }
            return result.CancelTaxResult;
        }
    
    }

}
