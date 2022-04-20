using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using System.Text.Json;


namespace mini_projekat
{
    internal class APIController {
        private static string API_KEY = "CKT83JMVM613ZOBS";
        public static void GetCryptoData(string period, string currency, string market, string interval)
        {
            Uri queryUri = new Uri(
                "https://www.alphavantage.co/query?function=" + period + 
                "&symbol=" + currency + 
                "&market=" + market + 
                "&interval=" + interval + 
                "&apikey=" + API_KEY
                );

            using (WebClient client = new()) {
                using (StreamWriter writer = new StreamWriter(@"C:\Users\marko\Desktop\VI semestar\ICR\mini_projekat\hci-mini\mini_projekat\mini_projekat\api_data.json")) {
                    writer.Write(client.DownloadString(queryUri));
                }
            }
        }
    }
}
