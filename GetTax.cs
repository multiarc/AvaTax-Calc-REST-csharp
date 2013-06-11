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
    //These classes are for the tax/get resource, verb POST
    [Serializable]
    public class GetTaxRequest
    {
        //Required for tax calculation
        public string DocDate {get; set;}
        public string CustomerCode  {get; set;}
        public Address[] Addresses { get; set; }
        public Line[] Lines { get; set; }
        //Best Practice for tax calculation
        public string Client { get; set; }
        public string DocCode { get; set; }
        public DocType DocType {get; set;}
        public string CompanyCode {get; set;}
        public bool Commit {get; set;}
        public DetailLevel DetailLevel {get; set;}
        //Use where appropriate to the situation
        public string CustomerUsageType { get; set; }
        public string ExemptionNo { get; set; }
        public decimal Discount { get; set; }
        public string BusinessIdentificationNo { get; set; }
        public TaxOverrideDef TaxOverride { get; set; }
        //Optional
        public string PurchaseOrderNo { get; set; }
        public string PaymentDate { get; set; }
        public string PosLaneCode { get; set; }
        public string ReferenceCode { get; set; }
            
    }

    [Serializable]
    public class Line
    {
        public string LineNo{get; set;} //Required
        public string DestinationCode{get; set;} //Required
        public string OriginCode{get; set;} //Required
        public string ItemCode{get;set;} //Required
        public decimal Qty{get; set;} //Required
        public decimal Amount{get; set;} //Required
        public string TaxCode { get; set; } //Best practice
        public string CustomerUsageType { get; set; }
        public string Description { get; set; } //Best Practice
        public bool Discounted { get; set; }
        public bool TaxIncluded { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }

    }
    [Serializable]
    public class TaxOverrideDef //Allows tax date, amount, or exempt status to be overridden.
    {
        public string TaxOverrideType { get; set; }
        public string TaxAmount { get; set; }
        public string TaxDate { get; set; }
        public string Reason { get; set; }
    
    }
    [Serializable]
    public class GetTaxResult //Result of tax/get verb POST
    {
        public string DocCode { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalExemption { get; set; }
        public decimal TotalTaxable { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalTaxCalculated { get; set; }
        public DateTime TaxDate { get; set; }
        public TaxLine[] TaxLines { get; set; }
        public TaxLine[] TaxSummary { get; set; }
        public TaxAddress[] TaxAddresses { get; set; }
        public SeverityLevel ResultCode { get; set; }
        public Message[] Messages { get; set; }

    
    }
    [Serializable]
    public class GeoTaxResult //Result of tax/get verb GET
    {
        public decimal Rate { get; set; }
        public decimal Tax {get; set;}
        public TaxDetail[] TaxDetails { get; set; }
        public SeverityLevel ResultCode { get; set; }
        public Message[] Messages { get; set; }
    }
    [Serializable]
    public class TaxLine //Result object
    {
        public string LineNo { get; set; }
        public string TaxCode { get; set; }
        public bool Taxability { get; set; }
        public decimal Taxable { get; set; }
        public decimal Rate { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal TaxCalculated { get; set; }
        public decimal Exemption { get; set; }
        public TaxDetail[] TaxDetails { get; set; }
    }
    [Serializable]
    public class TaxDetail //Result object
    {
        public decimal Rate { get; set; }
        public decimal Tax { get; set; }
        public decimal Taxable { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string JurisType { get; set; }
        public string JurisName { get; set; }
        public string TaxName { get; set; }
    }
    [Serializable]
    public class TaxAddress //Result object
    {
        public string Address{ get; set; }
        public string AddressCode{ get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string TaxRegionId { get; set; }
        public string JurisCode { get; set; }
    }


    public enum DocType { SalesOrder, SalesInvoice, ReturnOrder, ReturnInvoice, PurchaseOrder, PurchaseInvoice };
    public enum DetailLevel { Tax, Document, Line, Diagnostic };
    public enum SystemCustomerUsageType
    { 
        L,//"Other",
        A,//"Federal government",
        B,//"State government",
        C,//"Tribe / Status Indian / Indian Band",
        D,//"Foreign diplomat",
        E,//"Charitable or benevolent organization",
        F,//"Regligious or educational organization",
        G,//"Resale",
        H,//"Commercial agricultural production",
        I,// "Industrial production / manufacturer",
        J,// "Direct pay permit",
        K,// "Direct Mail",
        N,// "Local Government",
        P,// "Commercial Aquaculture",
        Q,// "Commercial Fishery",
        R// "Non-resident"
    }
    public class GetTax
    {
        //This actually calls the service to perform the tax calculation, and returns the calculation result.
        public static GetTaxResult Get(GetTaxRequest req, string acctNum, string licKey, string companyCode, string webAddr)
        {


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
            //doc.Save(@"get_tax_request.xml");

            //Call the service
            Uri address = new Uri(webAddr + "tax/get");
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(acctNum + ":" + licKey)));
            request.Method = "POST";
            request.ContentType = "text/xml";
            request.ContentLength = sb.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(ASCIIEncoding.ASCII.GetBytes(sb.ToString()), 0, sb.Length);
            GetTaxResult result = new GetTaxResult();
            try
            {
                WebResponse response = request.GetResponse();
                XmlSerializer r = new XmlSerializer(result.GetType());
                result = (GetTaxResult)r.Deserialize(response.GetResponseStream());
            }
            catch (WebException ex)
            {
                XmlSerializer r = new XmlSerializer(result.GetType());
                result = (GetTaxResult)r.Deserialize(((HttpWebResponse)ex.Response).GetResponseStream());
            }
            return result;
        }       
    }
}

