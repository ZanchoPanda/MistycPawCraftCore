using System;
using System.Text;

namespace MistycPawCraftCore.Utils
{
    public static class Ppm
    {

        private const string PaypalLinkBase64 = "aHR0cHM6Ly93d3cucGF5cGFsLm1lL1pQYW5kYUFwcHM=";

        public static string PaypalDonateUrl => Decode(PaypalLinkBase64);

        private static string Decode(string base64)
        {
            var bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }


    }
}
