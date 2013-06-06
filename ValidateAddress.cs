using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Web;
using System.Net;
using System.IO;

namespace AvaTaxCalcREST
{
    //Request for address/validate is parsed into the URI query parameters.
    [Serializable]
    public class ValidateResult
    {
        public Address Address { get; set; }
        public SeverityLevel ResultCode { get; set; }
        public Message[] Messages { get; set; }
    }

    [Serializable]
    public class Address
    {
        //Address can be determined for tax calculation by Line1, City, Region, PostalCode, Country OR Latitude/Longitude OR TaxRegionId
        public string AddressCode { get; set; } //Input for GetTax only, not by address validation
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string County { get; set; } //Output for ValidateAddress only
        public string FipsCode { get; set; } //Output for ValidateAddress only
        public string CarrierRoute { get; set; } //Output for ValidateAddress only
        public string PostNet { get; set; }//Output for ValidateAddress only
        public AddressType? AddressType { get; set; } //Output for ValidateAddress only
        public decimal? Latitude { get; set; } //Input for GetTax only
        public decimal? Longitude { get; set; } //Input for GetTax only
        public string TaxRegionId { get; set; } //Input for GetTax only

    }
    public enum AddressType
    {
        F,//Firm or company address
        G,//General Delivery address
        H,//High-rise or business complex
        P,//PO box address
        R,//Rural route address
        S//Street or residential address
    };
    public class ValidateAddress
    {
        public static ValidateResult Validate(Address addr, string AcctNum, string LicKey, string CompanyCode, string webaddr)
        {

            //Convert input address data to query string
            string querystring = "";
            if (addr.Line1 != null) { querystring = querystring + "Line1=" + addr.Line1.Replace(" ", "+"); }
            if (addr.Line2 != null) { querystring = querystring + "&Line2=" + addr.Line2.Replace(" ", "+"); }
            if (addr.Line3 != null) { querystring = querystring + "&Line3=" + addr.Line3.Replace(" ", "+"); }
            if (addr.City != null) { querystring = querystring + "&City=" + addr.City.Replace(" ", "+"); }
            if (addr.Region != null) { querystring = querystring + "&Region=" + addr.Region.Replace(" ", "+"); }
            if (addr.PostalCode != null) { querystring = querystring + "&PostalCode=" + addr.PostalCode.Replace(" ", "+"); }
            if (addr.Country != null) { querystring = querystring + "&Country=" + addr.Country.Replace(" ", "+"); }


            //Call the service
            Uri address = new Uri(webaddr + "address/validate.xml?" + querystring);
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(AcctNum + ":" + LicKey)));
            request.Method = "GET";

            ValidateResult result = new ValidateResult();
            try
            {
                WebResponse response = request.GetResponse();
                XmlSerializer r = new XmlSerializer(result.GetType());
                result = (ValidateResult)r.Deserialize(response.GetResponseStream());
                addr = result.Address; //If the address was validated, take the validated address.
            }
            catch (WebException ex)
            {
                XmlSerializer r = new XmlSerializer(result.GetType());
                result = (ValidateResult)r.Deserialize(((HttpWebResponse)ex.Response).GetResponseStream());
            }
            return result;

        }
    }
}

