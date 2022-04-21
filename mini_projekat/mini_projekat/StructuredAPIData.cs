using System;
using System.Collections.Generic;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace mini_projekat {
    internal class StructuredAPIData {
        private List<DateTime> dateTimeList;
        private List<double> openValues;
        private List<double> closeValues;
        private List<double> highValues;
        private List<double> lowValues;

        public StructuredAPIData(Dictionary<string, dynamic> apiData) {
            dateTimeList = new List<DateTime>();
            openValues = new List<double>();
            closeValues = new List<double>();
            highValues = new List<double>();
            lowValues = new List<double>();
            parseDataValues(extractDataValues(apiData));

        }

        private dynamic extractDataValues(Dictionary<string, dynamic> data) {
            foreach (KeyValuePair<string, dynamic> pair in data) 
                if (pair.Key != "Meta Data")
                    return pair.Value;
            return null;
        }

        private void parseDataValues(dynamic dataValues) {
            foreach (KeyValuePair<string, dynamic> pair in JsonSerializer.Deserialize<Dictionary<string, dynamic>>(dataValues)) {
                dateTimeList.Add(DateTime.Parse(pair.Key));
                parseDataValuesTypes(pair.Value);
            }  
        }

        private void parseDataValuesTypes(dynamic dataValuesTypes) {
            foreach (KeyValuePair<string, dynamic> pair in JsonSerializer.Deserialize<Dictionary<string, dynamic>>(dataValuesTypes)) {
                string key = pair.Key;
                double value = Double.Parse(pair.Value.ToString());
                if (key.Contains("1. open") || key.Contains("1a. open"))
                    openValues.Add(value);
                else if (key.Contains("2. high") || key.Contains("2a. high"))
                    highValues.Add(value);
                else if (key.Contains("3. low") || key.Contains("3a. low"))
                    lowValues.Add(value);
                else if (key.Contains("4. close") || key.Contains("4a. close")) {
                    closeValues.Add(value);
                    break;  // Zato sto je ovo poslednji value type koji treba da obradi pa da ne bi bezveze iterirao nasilno zavrsava
                }
            }
        }
    }

}
