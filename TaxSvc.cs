namespace AvaTaxCalcREST
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public enum DocType
    {
        SalesOrder,
        SalesInvoice,
        ReturnOrder,
        ReturnInvoice,
        PurchaseOrder,
        PurchaseInvoice
    }

    public enum DetailLevel
    {
        Tax,
        Document,
        Line,
        Diagnostic
    }

    public enum SystemCustomerUsageType
    {
        L, // "Other",
        A, // "Federal government",
        B, // "State government",
        C, // "Tribe / Status Indian / Indian Band",
        D, // "Foreign diplomat",
        E, // "Charitable or benevolent organization",
        F, // "Regligious or educational organization",
        G, // "Resale",
        H, // "Commercial agricultural production",
        I, // "Industrial production / manufacturer",
        J, // "Direct pay permit",
        K, // "Direct Mail",
        N, // "Local Government",
        P, // "Commercial Aquaculture",
        Q, // "Commercial Fishery",
        R // "Non-resident"
    }

    public enum CancelCode
    {
        Unspecified,
        PostFailed,
        DocDeleted,
        DocVoided,
        AdjustmentCancelled
    }

    [Serializable]
    public class GetTaxRequest
    {
        // Required for tax calculation
        public string DocDate { get; set; }

        public string CustomerCode { get; set; }

        public Address[] Addresses { get; set; }

        public Line[] Lines { get; set; }

        // Best Practice for tax calculation
        public string Client { get; set; }

        public string DocCode { get; set; }

        public DocType DocType { get; set; }

        public string CompanyCode { get; set; }

        public bool Commit { get; set; }

        public DetailLevel DetailLevel { get; set; }

        // Use where appropriate to the situation
        public string CustomerUsageType { get; set; }

        public string ExemptionNo { get; set; }

        public decimal Discount { get; set; }

        public string BusinessIdentificationNo { get; set; }

        public TaxOverrideDef TaxOverride { get; set; }

        public string CurrencyCode { get; set; }

        // Optional
        public string PurchaseOrderNo { get; set; }

        public string PaymentDate { get; set; }

        public string PosLaneCode { get; set; }

        public string ReferenceCode { get; set; }
    }

    [Serializable]
    public class Line
    {
        public string LineNo { get; set; } // Required

        public string DestinationCode { get; set; } // Required

        public string OriginCode { get; set; } // Required

        public string ItemCode { get; set; } // Required

        public decimal Qty { get; set; } // Required

        public decimal Amount { get; set; } // Required

        public string TaxCode { get; set; } // Best practice

        public string CustomerUsageType { get; set; }

        public TaxOverrideDef TaxOverride { get; set; }

        public string Description { get; set; } // Best Practice

        public bool Discounted { get; set; }

        public bool TaxIncluded { get; set; }

        public string Ref1 { get; set; }

        public string Ref2 { get; set; }
    }

    [Serializable]
    public class TaxOverrideDef // Allows tax date, amount, or exempt status to be overridden.
    {
        public string TaxOverrideType { get; set; }

        public string TaxAmount { get; set; }

        public string TaxDate { get; set; }

        public string Reason { get; set; }    
    }

    [Serializable]
    public class GetTaxResult // Result of tax/get verb POST
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
    public class GeoTaxResult // Result of tax/get verb GET
    {
        public decimal Rate { get; set; }

        public decimal Tax { get; set; }

        public TaxDetail[] TaxDetails { get; set; }

        public SeverityLevel ResultCode { get; set; }

        public Message[] Messages { get; set; }
    }

    [Serializable]
    public class TaxLine // Result object
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
    public class TaxDetail // Result object
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
    public class TaxAddress // Result object
    {
        public string Address { get; set; }

        public string AddressCode { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string TaxRegionId { get; set; }

        public string JurisCode { get; set; }
    }

    // These classes are for tax/cancel (verb POST)
    [Serializable]
    public class CancelTaxRequest
    {
        // Required for CancelTax operation
        public CancelCode CancelCode { get; set; }

        public DocType DocType { get; set; } // Note that the only *meaningful* values for this property here are SalesInvoice, ReturnInvoice, PurchaseInvoice.
        
        // The document needs to be identified by either DocCode/CompanyCode (recommended) OR DocId (not recommended).
        public string CompanyCode { get; set; }

        public string DocCode { get; set; }
    }

    [Serializable]
    public class CancelTaxResponse
    {
        public CancelTaxResult CancelTaxResult { get; set; }

        public SeverityLevel ResultCode { get; set; }

        public Message[] Messages { get; set; }
    }

    [Serializable]
    public class CancelTaxResult
    {
        public SeverityLevel ResultCode { get; set; }

        public string TransactionId { get; set; }

        public string DocId { get; set; }

        public Message[] Messages { get; set; }
    }

    public class TaxSvc
    {
        private static string accountNum;
        private static string license;
        private static string svcURL;

        public TaxSvc(string accountNumber, string licenseKey, string serviceURL)
        {
            accountNum = accountNumber;
            license = licenseKey;
            svcURL = serviceURL.TrimEnd('/') + "/1.0/";
        }

        // This actually calls the service to perform the tax calculation, and returns the calculation result.
        public GetTaxResult GetTax(GetTaxRequest req)
        {
            // Convert the request to XML
            XmlSerializerNamespaces namesp = new XmlSerializerNamespaces();
            namesp.Add(string.Empty, string.Empty);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            XmlSerializer x = new XmlSerializer(req.GetType());
            StringBuilder sb = new StringBuilder();
            x.Serialize(XmlTextWriter.Create(sb, settings), req, namesp);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());

            // Call the service
            Uri address = new Uri(svcURL + "tax/get");
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(accountNum + ":" + license)));
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

        public GeoTaxResult EstimateTax(decimal latitude, decimal longitude, decimal saleAmount)
        {
            // Call the service
            Uri address = new Uri(svcURL + "tax/" + latitude.ToString() + "," + longitude.ToString() + "/get.xml?saleamount=" + saleAmount);
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(accountNum + ":" + license)));
            request.Method = "GET";

            GeoTaxResult result = new GeoTaxResult();
            try
            {
                WebResponse response = request.GetResponse();
                XmlSerializer r = new XmlSerializer(result.GetType());
                result = (GeoTaxResult)r.Deserialize(response.GetResponseStream());
            }
            catch (WebException ex)
            {
                Stream responseStream = ((HttpWebResponse)ex.Response).GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string responseString = reader.ReadToEnd();

                // The service returns some error messages in JSON for authentication/unhandled errors.
                if (responseString.StartsWith("{") || responseString.StartsWith("[")) 
                {
                    result = new GeoTaxResult();
                    result.ResultCode = SeverityLevel.Error;
                    Message msg = new Message();
                    msg.Severity = result.ResultCode;
                    msg.Summary = "The request was unable to be successfully serviced, please try again or contact Customer Service.";
                    msg.Source = "Avalara.Web.REST";
                    if (!((HttpWebResponse)ex.Response).StatusCode.Equals(HttpStatusCode.InternalServerError))
                    {
                        msg.Summary = "The user or account could not be authenticated.";
                        msg.Source = "Avalara.Web.Authorization";
                    }

                    result.Messages = new Message[1] { msg };
                }
                else
                {
                    XmlSerializer r = new XmlSerializer(result.GetType());
                    byte[] temp = Encoding.ASCII.GetBytes(responseString);
                    MemoryStream stream = new MemoryStream(temp);
                    result = (GeoTaxResult)r.Deserialize(stream); // Inelegant, but the deserializer only takes streams, and we already read ours out.
                }
            }

            return result;        
        }

        public GeoTaxResult Ping()
        {
            return this.EstimateTax((decimal)47.627935, (decimal)-122.51702, (decimal)10);
        }

        // This calls CancelTax to void a transaction specified in taxreq
        public CancelTaxResult CancelTax(CancelTaxRequest cancelTaxRequest)
        {
            // Convert the request to XML
            XmlSerializerNamespaces namesp = new XmlSerializerNamespaces();
            namesp.Add(string.Empty, string.Empty);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            XmlSerializer x = new XmlSerializer(cancelTaxRequest.GetType());
            StringBuilder sb = new StringBuilder();
            x.Serialize(XmlTextWriter.Create(sb, settings), cancelTaxRequest, namesp);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());

            // Call the service
            Uri address = new Uri(svcURL + "tax/cancel");
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(accountNum + ":" + license)));
            request.Method = "POST";
            request.ContentType = "text/xml";
            request.ContentLength = sb.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(ASCIIEncoding.ASCII.GetBytes(sb.ToString()), 0, sb.Length);
            CancelTaxResponse cancelResponse = new CancelTaxResponse();
            try
            {
                WebResponse response = request.GetResponse();
                XmlSerializer r = new XmlSerializer(cancelResponse.GetType());
                cancelResponse = (CancelTaxResponse)r.Deserialize(response.GetResponseStream());
            }
            catch (WebException ex)
            {
                XmlSerializer r = new XmlSerializer(cancelResponse.GetType());
                cancelResponse = (CancelTaxResponse)r.Deserialize(((HttpWebResponse)ex.Response).GetResponseStream());

                // If the error is returned at the cancelResponse level, translate it to the cancelResult.
                if (cancelResponse.ResultCode.Equals(SeverityLevel.Error)) 
                {
                    cancelResponse.CancelTaxResult = new CancelTaxResult();
                    cancelResponse.CancelTaxResult.ResultCode = cancelResponse.ResultCode;
                    cancelResponse.CancelTaxResult.Messages = cancelResponse.Messages;
                }
            }

            return cancelResponse.CancelTaxResult;
        }
    }
}