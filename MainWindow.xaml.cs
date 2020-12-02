using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _filePath = $"{Directory.GetCurrentDirectory()}/data.xlsx";


        private ObservableCollection<Threat> _data = new ObservableCollection<Threat>();
        public ObservableCollection<Threat> displayData = new ObservableCollection<Threat>();

        private int _displayAmount = 15;
        private int _pageFirstIndex;
        private int _pageLastIndex;
        private string _currentPages = "nothing to display";


        private string _status = "Всё ок!";


        public MainWindow()
        {
            InitializeComponent();
            this.FontFamily = new FontFamily("Comic Sans MS");
            Data.ItemsSource = displayData;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (File.Exists(_filePath))
            {
                _status = "Найдена сохраненная копия.";
                _data.Clear();
                using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(_filePath)))
                {
                    var myWorksheet = xlPackage.Workbook.Worksheets.First();
                    var totalRows = myWorksheet.Dimension.End.Row;
                    var totalColumns = myWorksheet.Dimension.End.Column;

                    for (int rowNum = 3; rowNum <= totalRows; rowNum++)
                    {
                        _data.Add(new Threat(
                            "УБИ." + myWorksheet.Cells[rowNum, 1].Value.ToString(),
                            myWorksheet.Cells[rowNum, 2].Value.ToString(),
                            myWorksheet.Cells[rowNum, 3].Value.ToString(),
                            myWorksheet.Cells[rowNum, 4].Value.ToString(),
                            myWorksheet.Cells[rowNum, 5].Value.ToString(),
                            myWorksheet.Cells[rowNum, 6].Value.ToString() == "1" ? "✔" : "❌",
                            myWorksheet.Cells[rowNum, 7].Value.ToString() == "1" ? "✔" : "❌",
                            myWorksheet.Cells[rowNum, 8].Value.ToString() == "1" ? "✔" : "❌"
                            ));
                    }
                }
                if (_data.Count() == 0)
                {
                    _pageFirstIndex = 0;
                    _pageLastIndex = 0;
                }
                else if (_data.Count() < _displayAmount)
                {
                    _pageFirstIndex = 1;
                    _pageLastIndex = _data.Count();
                }
                else
                {
                    _pageFirstIndex = 1;
                    _pageLastIndex = _pageFirstIndex + _displayAmount;
                } 
                _currentPages = $"{_pageFirstIndex}..{_pageLastIndex} out of {_data.Count()}";              
                Update();
            }
            else
            {
                Download();
                _status = "Не найдено сохранённой копии. Повторная проверка доступна после окончания скачивания";
                Update();
            }
        }

        private void Download()
        {
            WebClient client = new WebClient();
            client.DownloadFileAsync(new Uri("https://bdu.fstec.ru/files/documents/thrlist.xlsx"), _filePath);
            client.Dispose();
        }

        private void Update()
        {
            StatusBar.Text = _status;
            CurrentPages.Text = _currentPages;
            displayData.Clear();
            if (_data.Count() != 0)
            {
                for (int i = _pageFirstIndex - 1; i < _pageLastIndex; i++)
                {
                    displayData.Add(_data[i]);
                } 
            }
        }

        private void TestClick(object sender, RoutedEventArgs e)
        {
            _status = "Test is fine!";
            MessageBox.Show(_data[0].Name);
            Update();
        }

        private void PreviousPages(object sender, RoutedEventArgs e)
        {
            if (_pageFirstIndex - _displayAmount < 1)
            {
                _pageFirstIndex = 1;
            }
            else
            {
                _pageFirstIndex -= _displayAmount;
            }
            _pageLastIndex = _pageFirstIndex + _displayAmount <= _data.Count() ? _pageFirstIndex + _displayAmount : _data.Count();
            _currentPages = $"{_pageFirstIndex}..{_pageLastIndex} out of {_data.Count()}";
            Update();
        }

        private void NextPages(object sender, RoutedEventArgs e)
        {
            if (_pageFirstIndex + _displayAmount < _data.Count())
            {
                _pageFirstIndex += _displayAmount;
            }
            _pageLastIndex = _pageFirstIndex + _displayAmount <= _data.Count() ? _pageFirstIndex + _displayAmount : _data.Count();
            _currentPages = $"{_pageFirstIndex}..{_pageLastIndex} out of {_data.Count()}";
            Update();
        }

        private void PagesSelected(object sender, RoutedEventArgs e)
        {
            switch (RecordsPerPage.SelectedIndex)
            {
                case 0:
                    _displayAmount = 15;
                    break;
                case 1:
                    _displayAmount = 25;
                    break;
                case 2:
                    _displayAmount = 50;
                    break;
            }
            _pageLastIndex = _pageFirstIndex + _displayAmount <= _data.Count() ? _pageFirstIndex + _displayAmount : _data.Count();
            _currentPages = $"{_pageFirstIndex}..{_pageLastIndex} out of {_data.Count()}";
            Update();
        }
    }
}
