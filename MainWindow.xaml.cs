using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
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
using System.Xml.Serialization;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _xlsxFilePath = $"{Directory.GetCurrentDirectory()}/data.xlsx";
        private string _rawDataFilePath = $"{Directory.GetCurrentDirectory()}/data.binary";


        private ObservableCollection<Threat> _data = new ObservableCollection<Threat>();
        private ObservableCollection<Threat> _dataNew = new ObservableCollection<Threat>();
        public ObservableCollection<Threat> displayData = new ObservableCollection<Threat>();
        public ObservableCollection<ThreatDiff> diff = new ObservableCollection<ThreatDiff>();

        private int _displayAmount = 15;
        private int _pageFirstIndex = 1;
        private int _pageLastIndex = 1;
        private string _currentPages = "nothing to display";


        private string _status = "Всё ок!";


        public MainWindow()
        {
            InitializeComponent();
            this.FontFamily = new FontFamily("Comic Sans MS");
            Data.ItemsSource = displayData;
            CheckSaved();
        }

        private void CheckSaved()
        {
            if (File.Exists(_rawDataFilePath))
            {
                MessageBox.Show("Saved data founded!", "Saved data", MessageBoxButton.OK, MessageBoxImage.Information);
                GetSavedData();
            }
            else
            {
                var result = MessageBox.Show("There is no saved data! Download new data?", "No saved data", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Download();
                    getDataFromXLSX();
                    _data = _dataNew;
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
                    MessageBox.Show("File was downloaded and analyzed!");
                    
                }
            }
        }

        private void GetSavedData()
        {
            using (FileStream fs = new FileStream(_rawDataFilePath, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                _data = (ObservableCollection<Threat>)formatter.Deserialize(fs);
            }
            MessageBox.Show("Data successfully readed from local file!");
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

        private void getDataFromXLSX()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _dataNew.Clear();
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(_xlsxFilePath)))
            {
                var myWorksheet = xlPackage.Workbook.Worksheets.First();
                var totalRows = myWorksheet.Dimension.End.Row;
                var totalColumns = myWorksheet.Dimension.End.Column;

                for (int rowNum = 3; rowNum <= totalRows; rowNum++)
                {
                    _dataNew.Add(new Threat(
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
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(_rawDataFilePath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, _dataNew);
            }
            File.Delete(_xlsxFilePath);       
            
        }

        private void ForceUpdate(object sender, RoutedEventArgs e)
        {
            try
            {
                Download();
                getDataFromXLSX();
                diff.Clear();
                foreach (Threat currentThreat in _dataNew)
                {
                    var target = _data.Where(el => el.ID == currentThreat.ID).ToList();
                    
                    if (target.Count() != 0)
                    {    
                        Threat old = target[0];
                        if (old.Name != currentThreat.Name)
                        {
                            diff.Add(new ThreatDiff(old.ID, "Name", old.Name, currentThreat.Name));
                        }
                        if (old.Description != currentThreat.Description)
                        {
                            diff.Add(new ThreatDiff(old.ID, "Description", old.Description, currentThreat.Description));
                        }
                        if (old.Source != currentThreat.Source)
                        {
                            diff.Add(new ThreatDiff(old.ID, "Source", old.Source, currentThreat.Source));
                        }
                        if (old.Obj != currentThreat.Obj)
                        {
                            diff.Add(new ThreatDiff(old.ID, "Object", old.Obj, currentThreat.Obj));
                        }
                        if (old.Confidentiality != currentThreat.Confidentiality)
                        {
                            diff.Add(new ThreatDiff(old.ID, "Confidentiality", old.Confidentiality, currentThreat.Confidentiality));
                        }
                        if (old.Integrity != currentThreat.Integrity)
                        {
                            diff.Add(new ThreatDiff(old.ID, "Integrity", old.Integrity, currentThreat.Integrity));
                        }
                        if (old.Availability != currentThreat.Availability)
                        {
                            diff.Add(new ThreatDiff(old.ID, "Availability", old.Availability, currentThreat.Availability));
                        }
                        
                    }
                }
                new Diff().ShowDialog();
                _data = _dataNew;
                Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error during update!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Download()
        {
            WebClient client = new WebClient();
            client.DownloadFile(new Uri("https://bdu.fstec.ru/files/documents/thrlist.xlsx"), _xlsxFilePath);
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
