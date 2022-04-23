using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace mini_projekat {
    public partial class MainWindow : Window {
        private ChartDataSet? chartDataSet;
        private TableDataSet? tableDataSet;
        private StructuredAPIData? apiData;

        private string cryptCurr = "";
        private string marketCurr = "";
        private string functionType = "";
        private string dataType = "";
        private string? interval = null;


        public MainWindow() {
            InitializeComponent();
            ComboBoxSetup();
            initChartAxis();
            initChartTable();
        }
        private void initChartAxis() {
            WpfPlot1.plt.XLabel("Interval");
            WpfPlot1.plt.YLabel("Market Currency Value");
            WpfPlot1.plt.Style(ScottPlot.Style.Blue3);
            WpfPlot1.Plot.XAxis.DateTimeFormat(true);
            WpfPlot1.Refresh();
        }

        private void ComboBoxSetup() {
            SetUpMarketComboBox(FileReader.GetMarketComboBoxSelections());
            SetUpCurrencyComboBox(FileReader.GetCurrencyComboBoxSelections());
        }

        private void SetUpMarketComboBox(string[] fileLines) {
            marketCurrComboBox.ItemsSource = prepareComboData(fileLines);
            marketCurrComboBox.DisplayMemberPath = "Value";
            marketCurrComboBox.SelectedValuePath = "Id";
            marketCurrComboBox.SelectedIndex = 0;
        }

        private void SetUpCurrencyComboBox(string[] fileLines) {
            cryptoCurrComboBox.ItemsSource = prepareComboData(fileLines);
            cryptoCurrComboBox.DisplayMemberPath = "Value";
            cryptoCurrComboBox.SelectedValuePath = "Id";
            cryptoCurrComboBox.SelectedIndex = 0;
        }

        private List<ComboData> prepareComboData(string[] fileLines) {
            List<ComboData> listData = new();
            listData.Add(new ComboData() {
                Id = "",
                Value = "None Selected"
            });
            foreach (string line in fileLines)
                listData.Add(
                    new ComboData() {
                        Id = line.Split(" -> ")[0],
                        Value = line
                    }
                );
            return listData;
        }

        private void initChartTable() {
            table.Visibility = Visibility.Hidden;
            prevPageButton.Visibility = Visibility.Hidden;
            nextPageButton.Visibility = Visibility.Hidden;
            pageLabel.Visibility = Visibility.Hidden;
        }

        private void showChartBtn(object sender, RoutedEventArgs e) {
            table.Visibility = Visibility.Hidden;
            prevPageButton.Visibility = Visibility.Hidden;
            nextPageButton.Visibility = Visibility.Hidden;
            pageLabel.Visibility = Visibility.Hidden;

            WpfPlot1.Visibility = Visibility.Visible;
        }

        private void showTableBtn(object sender, RoutedEventArgs e) {
            WpfPlot1.Visibility = Visibility.Hidden;
         
            table.Visibility = Visibility.Visible;
            prevPageButton.Visibility = Visibility.Visible;
            nextPageButton.Visibility = Visibility.Visible;
            pageLabel.Visibility = Visibility.Visible;
        }

        private void nextPageBtn(object sender, RoutedEventArgs e) {
            try {
                table.ItemsSource = tableDataSet?.nextPage(marketCurr, cryptCurr, dataType);
                setPageLabelText();
            } catch (InvalidOperationException) { }
        }

        private void previousPageBtn(object sender, RoutedEventArgs e) {
            try {
                table.ItemsSource = tableDataSet?.previousPage(marketCurr, cryptCurr, dataType);
                setPageLabelText();
            } catch (InvalidOperationException) { }
        }

        private void setPageLabelText() {
            if (tableDataSet != null)
                pageLabel.Content = "Page: " + tableDataSet.currentPage + "/" + tableDataSet.numOfPages;
        }

        private void submitFormBtn(object sender, RoutedEventArgs e) {
            string cryptComBox = cryptoCurrComboBox.SelectedValue.ToString();
            string marketComBox = marketCurrComboBox.SelectedValue.ToString();
            string funcTypeComBox = intervalComboBox.SelectedValue.ToString().Split(" ")[1];
            string dataTypeComBox = valuTypeComboBox.SelectedValue.ToString().Split(" ")[1];

            if (!checkIfComBoxSameOrEmpty(cryptComBox, marketComBox, funcTypeComBox, dataTypeComBox))
                return;

            cryptCurr = cryptComBox;
            marketCurr = marketComBox;
            functionType = parseFunctionType(funcTypeComBox);
            dataType = dataTypeComBox;
            interval = getInterval(funcTypeComBox);

            try {
                apiData = new StructuredAPIData(callAPI());
            }
            catch (Exception) {
                apiErrorPopUp.IsOpen = true;
                return;
            }

            tableDataSet = new TableDataSet(apiData);
            chartDataSet = new ChartDataSet(apiData);

            WpfPlot1.Plot.Clear();
            WpfPlot1.Plot.AddScatter(
                    chartDataSet.getAxisX().Select(x => x.ToOADate()).ToArray(),
                    chartDataSet.getAxisY(dataType).ToArray()
                );
            WpfPlot1.Refresh();
             
            table.ItemsSource = tableDataSet.getTableRows(marketCurr, cryptCurr, dataType);
            setPageLabelText();
        }

        private bool checkIfComBoxSameOrEmpty(string cryptComBox, string marketComBox, string intervalComBox, string dataTypeComBox) {
            return !(cryptComBox == "" || marketComBox == "" ||
                        intervalComBox == "None" || dataTypeComBox == "None");
        }

        private string parseFunctionType(string comboBoxStr) {
            return comboBoxStr switch {
                "month" => "DIGITAL_CURRENCY_MONTHLY",
                "week" => "DIGITAL_CURRENCY_WEEKLY",
                "day" => "DIGITAL_CURRENCY_DAILY",
                _ => "CRYPTO_INTRADAY",
            };
        }

        private string getInterval(string comboBoxStr) {
            switch(comboBoxStr) {
                case "60":
                    return "60min";
                case "30":
                    return "30min";
                case "15":
                    return "15min";
                case "5":
                    return "5min";
                case "1":
                    return "1min";
                default:
                    return null;
            }
        }

        private void exitAppBtn(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private Dictionary<string, dynamic> callAPI() {
            return APIController.GetCryptoData(functionType, cryptCurr, marketCurr, interval);
        }

        private void closePopUp(object sender, RoutedEventArgs e) {
            apiErrorPopUp.IsOpen = false;
        }
    }

    public class ComboData {
        public string? Id { get; set; }
        public string? Value { get; set; }
    }

}
