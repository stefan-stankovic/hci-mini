using System;
using System.Collections.Generic;

namespace mini_projekat {
    internal class TableDataSet {
        private StructuredAPIData apiData;

        private const int NUM_OF_ROWS= 20;

        private int dataSetLength;

        private int numOfPages;
        private int currentPage;

        public TableDataSet(StructuredAPIData apiData) {
            this.apiData = apiData;
            setPagingSettings();
        }

        public List<TableRowData> getTableRows(string marketCurr, string cryptoCurr, string valueType) {
            List<double> values = getValuesByType(valueType);

            List<TableRowData> rows = new();
            int start = (currentPage - 1) * NUM_OF_ROWS + 1;
            int end = currentPage == numOfPages ? dataSetLength % NUM_OF_ROWS : start + 20;
            for (int ind = start; ind < end; ind++)
                rows.Add(new TableRowData() {
                        ID = ind,
                        MarketCurrency = marketCurr,
                        CryptoCurrency = cryptoCurr,
                        MarketValue = values[ind],
                        DateAndTime = apiData.getDateTimeList()[ind]
                    }
                );
            return rows;
        }

        private List<double> getValuesByType(string valueType) {
            switch (valueType) {
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

        private void setPagingSettings() {
            dataSetLength = apiData.getDataSetLength();

            double pages = (dataSetLength / NUM_OF_ROWS);

            numOfPages = pages % 1 != 0 ? ((int)pages) + 1 : (int)pages;
            currentPage = 1;
        }

        public List<TableRowData> nextPage(string marketCurr, string cryptoCurr, string valueType) {
            if (currentPage == numOfPages)
                throw new InvalidOperationException("Cannot increment last page!");
            currentPage++;
            return getTableRows(marketCurr, cryptoCurr, valueType);
        }

        public List<TableRowData> previousPage(string marketCurr, string cryptoCurr, string valueType) {
            if (currentPage == 1)
                throw new InvalidOperationException("Cannot decrement first page!");
            currentPage--;
            return getTableRows(marketCurr, cryptoCurr, valueType);
        }
        
    }

    class TableRowData {
        public int ID { get; set; }
        public string? MarketCurrency { get; set; }
        public string? CryptoCurrency { get; set; }
        public double MarketValue { get; set; }
        public DateTime DateAndTime { get; set; }
    }
}
