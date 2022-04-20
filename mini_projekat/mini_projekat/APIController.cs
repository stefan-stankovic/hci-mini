using System;
using System.Net;

namespace mini_projekat
{
    internal class APIController {
        private static string API_KEY = "CKT83JMVM613ZOBS";
        public static string GetCryptoData(string period, string currency, string market, string interval)
        {
            Uri queryUri = new Uri(
                "https://www.alphavantage.co/query?function=" + period + 
                "&symbol=" + currency + 
                "&market=" + market + 
                "&interval=" + interval + 
                "&apikey=" + API_KEY
                );

            using (WebClient client = new()) {
                return client.DownloadString(queryUri);
            }
        }   
    }
}
