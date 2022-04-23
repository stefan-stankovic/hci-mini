using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace mini_projekat
{
    internal class APIController {
        private static string API_KEY = "CKT83JMVM613ZOBS";
        public static Dictionary<string, dynamic> GetCryptoData(string function, string currency, string market, string? interval) {
            Uri queryUri = new Uri(
                    "https://www.alphavantage.co/query" +
                    "?function=" + function + 
                    "&symbol=" + currency + 
                    "&market=" + market +
                    (function == "CRYPTO_INTRADAY" ? ("&interval=" + interval) : "") + 
                    "&apikey=" + API_KEY
                );

            using (WebClient client = new()) {
                return JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(queryUri));
            }
        }   
    }
}
