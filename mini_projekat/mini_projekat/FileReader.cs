using System.IO;

namespace mini_projekat {
    internal class FileReader {
        private const string EvilPopS_FILEPATH = @"D:\Projects\c#\hci-mini\mini_projekat\mini_projekat\";
        private const string Stanke_FILEPATH = @"";

        private const string CURRENT_FILEPATH = EvilPopS_FILEPATH;
        private const string MARKET_VALS_FILEPATH = CURRENT_FILEPATH + "market-currencies.txt"; 
        private const string CURRENCY_VALS_FILEPATH = CURRENT_FILEPATH + "crypto-currencies.txt";


        private static string[] ReadFileLines(string filePath) {
            using (StreamReader reader = new(filePath)) {
                return reader.ReadToEnd().Split("\n");
            }
        }
        public static string[] GetMarketComboBoxSelections() {
            return ReadFileLines(MARKET_VALS_FILEPATH);
        }

        public static string[] GetCurrencyComboBoxSelections() {
            return ReadFileLines(CURRENCY_VALS_FILEPATH);
        }

    }
}
