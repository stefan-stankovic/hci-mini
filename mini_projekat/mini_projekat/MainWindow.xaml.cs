using System.Collections.Generic;
using System.Windows;

namespace mini_projekat {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            ComboBoxSetup();

            double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            WpfPlot1.Plot.AddScatter(dataX, dataY);
            WpfPlot1.Refresh();
        }

        private void ComboBoxSetup() {
            SetUpMarketComboBox(FileReader.GetMarketComboBoxSelections());
            SetUpCurrencyComboBox(FileReader.GetCurrencyComboBoxSelections());
        }

        private void SetUpMarketComboBox(string[] fileLines) {
            prepareComboData(fileLines);
/*
            marketComboBox.ItemsSource = ListData;
            marketComboBox.DisplayMemberPath = "Value";
            marketComboBox.SelectedValuePath = "Id";
*/
        }

        private void SetUpCurrencyComboBox(string[] fileLines) {
            prepareComboData(fileLines);
/*
            currencyComboBox.ItemsSource = ListData;
            currencyComboBox.DisplayMemberPath = "Value";
            currencyComboBox.SelectedValuePath = "Id";
*/
        }

        private List<ComboData> prepareComboData(string[] fileLines) {
            List<ComboData> listData = new();
            foreach (string line in fileLines)
                listData.Add(
                    new ComboData() {
                        Id = line.Split(" -> ")[0],
                        Value = line
                    }
                );
            return listData;
        }

        private void batn(object sender, RoutedEventArgs e)
        {
            APIController.GetCryptoData("CRYPTO_INTRADAY", "ETH", "USD", "5min");
        }
    }
    public class ComboData {
        public string? Id { get; set; }
        public string? Value { get; set; }
    }
}
