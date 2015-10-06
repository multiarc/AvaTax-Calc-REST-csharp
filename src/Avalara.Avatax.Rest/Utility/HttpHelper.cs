using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Avalara.Avatax.Rest.Utility
{
    public class HttpHelper
    {
        internal static HttpWebRequest CreateRequest(Uri address, string accountNumber, string license)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(address);
            request.Headers = new WebHeaderCollection
            {
                [HttpRequestHeader.Authorization] =
                    "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(accountNumber + ":" + license))
            };
            return request;
        }
    }
}