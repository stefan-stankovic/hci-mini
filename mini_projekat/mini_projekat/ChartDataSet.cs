using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mini_projekat {
    internal class ChartDataSet {
        private StructuredAPIData apiData;
        public ChartDataSet(StructuredAPIData apiData) {
            this.apiData = apiData;
        }

        public List<DateTime> getAxisX() {
            return apiData.getDateTimeList();
        }

        public List<double> getAxisY(string type) {
            switch (type) {
                case "open":
                    return apiData.getOpenValues();
                case "close":
                    return apiData.getCloseValues();
                case "high":
                    return apiData.getHighValues();
                default:
                    return apiData.getLowValues();
            }
        }
    }
}
