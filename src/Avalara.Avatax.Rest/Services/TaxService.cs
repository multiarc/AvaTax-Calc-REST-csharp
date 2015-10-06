using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Avalara.Avatax.Rest.Data;
using Avalara.Avatax.Rest.Utility;

namespace Avalara.Avatax.Rest.Services
{
    public class TaxService: ITaxService
    {
        private static string _accountNumber;
        private static string _license;
        private static string _svcUrl;

        public TaxService(string accountNumber, string licenseKey, string serviceUrl)
        {
            _accountNumber = accountNumber;
            _license = licenseKey;
            _svcUrl = serviceUrl.TrimEnd('/') + "/1.0/";
        }

        // This actually calls the service to perform the tax calculation, and returns the calculation result.
        public async Task<GetTaxResult> GetTax(GetTaxRequest req)
        {
            // Convert the request to XML
            XmlSerializerNamespaces namesp = new XmlSerializerNamespaces();
            namesp.Add(string.Empty, string.Empty);
            XmlWriterSettings settings = new XmlWriterSettings {OmitXmlDeclaration = true};
            XmlSerializer x = new XmlSerializer(req.GetType());
            StringBuilder sb = new StringBuilder();
            x.Serialize(XmlWriter.Create(sb, settings), req, namesp);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());

            // Call the service
            Uri address = new Uri(_svcUrl + "tax/get");
            var request = HttpHelper.CreateRequest(address, _accountNumber, _license);
            request.Method = "POST";
            request.ContentType = "text/xml";
            //request.ContentLength = sb.Length;
            Stream newStream = await request.GetRequestStreamAsync();
            newStream.Write(Encoding.ASCII.GetBytes(sb.ToString()), 0, sb.Length);
            GetTaxResult result = new GetTaxResult();
            try
            {
                WebResponse response = await request.GetResponseAsync();
                XmlSerializer r = new XmlSerializer(result.GetType());
                result = (GetTaxResult) r.Deserialize(response.GetResponseStream());
            }
            catch (WebException ex)
            {
                XmlSerializer r = new XmlSerializer(result.GetType());
                result = (GetTaxResult) r.Deserialize(((HttpWebResponse) ex.Response).GetResponseStream());
            }

            return result;
        }

        public async Task<GeoTaxResult> EstimateTax(decimal latitude, decimal longitude, decimal saleAmount)
        {
            // Call the service
            Uri address = new Uri($"{_svcUrl}tax/{latitude},{longitude}/get.xml?saleamount={saleAmount}");
            var request = HttpHelper.CreateRequest(address, _accountNumber, _license);
            request.Method = "GET";

            GeoTaxResult result = new GeoTaxResult();
            try
            {
                WebResponse response = await request.GetResponseAsync();
                XmlSerializer r = new XmlSerializer(result.GetType());
                result = (GeoTaxResult) r.Deserialize(response.GetResponseStream());
            }
            catch (WebException ex)
            {
                Stream responseStream = ((HttpWebResponse) ex.Response).GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string responseString = reader.ReadToEnd();

                // The service returns some error messages in JSON for authentication/unhandled errors.
                if (responseString.StartsWith("{") || responseString.StartsWith("["))
                {
                    result = new GeoTaxResult {ResultCode = SeverityLevel.Error};
                    Message msg = new Message
                    {
                        Severity = result.ResultCode,
                        Summary =
                            "The request was unable to be successfully serviced, please try again or contact Customer Service.",
                        Source = "Avalara.Web.REST"
                    };
                    if (!((HttpWebResponse) ex.Response).StatusCode.Equals(HttpStatusCode.InternalServerError))
                    {
                        msg.Summary = "The user or account could not be authenticated.";
                        msg.Source = "Avalara.Web.Authorization";
                    }

                    result.Messages = new[] {msg};
                }
                else
                {
                    XmlSerializer r = new XmlSerializer(result.GetType());
                    byte[] temp = Encoding.ASCII.GetBytes(responseString);
                    MemoryStream stream = new MemoryStream(temp);
                    result = (GeoTaxResult) r.Deserialize(stream);
                        // Inelegant, but the deserializer only takes streams, and we already read ours out.
                }
            }

            return result;
        }

        public async Task<GeoTaxResult> Ping()
        {
            return await EstimateTax((decimal) 47.627935, (decimal) -122.51702, 10);
        }

        // This calls CancelTax to void a transaction specified in taxreq
        public async Task<CancelTaxResult> CancelTax(CancelTaxRequest cancelTaxRequest)
        {
            // Convert the request to XML
            XmlSerializerNamespaces namesp = new XmlSerializerNamespaces();
            namesp.Add(string.Empty, string.Empty);
            XmlWriterSettings settings = new XmlWriterSettings {OmitXmlDeclaration = true};
            XmlSerializer x = new XmlSerializer(cancelTaxRequest.GetType());
            StringBuilder sb = new StringBuilder();
            x.Serialize(XmlWriter.Create(sb, settings), cancelTaxRequest, namesp);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());

            // Call the service
            Uri address = new Uri(_svcUrl + "tax/cancel");
            var request = HttpHelper.CreateRequest(address, _accountNumber, _license);
            request.Method = "POST";
            request.ContentType = "text/xml";
            //request.ContentLength = sb.Length;
            Stream newStream = await request.GetRequestStreamAsync();
            newStream.Write(Encoding.ASCII.GetBytes(sb.ToString()), 0, sb.Length);
            CancelTaxResponse cancelResponse = new CancelTaxResponse();
            try
            {
                WebResponse response = await request.GetResponseAsync();
                XmlSerializer r = new XmlSerializer(cancelResponse.GetType());
                cancelResponse = (CancelTaxResponse) r.Deserialize(response.GetResponseStream());
            }
            catch (WebException ex)
            {
                XmlSerializer r = new XmlSerializer(cancelResponse.GetType());
                cancelResponse = (CancelTaxResponse) r.Deserialize(((HttpWebResponse) ex.Response).GetResponseStream());

                // If the error is returned at the cancelResponse level, translate it to the cancelResult.
                if (cancelResponse.ResultCode.Equals(SeverityLevel.Error))
                {
                    cancelResponse.CancelTaxResult = new CancelTaxResult
                    {
                        ResultCode = cancelResponse.ResultCode,
                        Messages = cancelResponse.Messages
                    };
                }
            }

            return cancelResponse.CancelTaxResult;
        }

        
    }
}