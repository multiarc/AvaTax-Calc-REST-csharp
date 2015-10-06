using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Avalara.Avatax.Rest.Data;
using Avalara.Avatax.Rest.Utility;

namespace Avalara.Avatax.Rest.Services
{
    public class AddressService : IAddressService
    {
        private static string _accountNumber;
        private static string _license;
        private static string _svcUrl;

        public AddressService(string accountNumber, string licenseKey, string serviceUrl)
        {
            _accountNumber = accountNumber;
            _license = licenseKey;
            _svcUrl = serviceUrl.TrimEnd('/') + "/1.0/";
        }

        public async Task<ValidateResult> Validate(Address address)
        {
            // Convert input address data to query string
            string querystring = string.Empty;
            querystring += (address.Line1 == null) ? string.Empty : "Line1=" + address.Line1.Replace(" ", "+");
            querystring += (address.Line2 == null) ? string.Empty : "&Line2=" + address.Line2.Replace(" ", "+");
            querystring += (address.Line3 == null) ? string.Empty : "&Line3=" + address.Line3.Replace(" ", "+");
            querystring += (address.City == null) ? string.Empty : "&City=" + address.City.Replace(" ", "+");
            querystring += (address.Region == null) ? string.Empty : "&Region=" + address.Region.Replace(" ", "+");
            querystring += (address.PostalCode == null) ? string.Empty : "&PostalCode=" + address.PostalCode.Replace(" ", "+");
            querystring += (address.Country == null) ? string.Empty : "&Country=" + address.Country.Replace(" ", "+");

            // Call the service
            Uri webAddress = new Uri(_svcUrl + "address/validate.xml?" + querystring);
            var request = HttpHelper.CreateRequest(webAddress, _accountNumber, _license);
            request.Method = "GET";

            ValidateResult result = new ValidateResult();
            try
            {
                WebResponse response = await request.GetResponseAsync();
                XmlSerializer r = new XmlSerializer(result.GetType());
                result = (ValidateResult)r.Deserialize(response.GetResponseStream());
                address = result.Address; // If the address was validated, take the validated address.
            }
            catch (WebException ex)
            {
                Stream responseStream = ((HttpWebResponse)ex.Response).GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string responseString = reader.ReadToEnd();

                // The service returns some error messages in JSON for authentication/unhandled errors.
                if (responseString.StartsWith("{")  || responseString.StartsWith("["))
                {
                    result = new ValidateResult();
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
                    result = (ValidateResult)r.Deserialize(stream); // Inelegant, but the deserializer only takes streams, and we already read ours out.
                }
            }

            return result;
        }
    }
}