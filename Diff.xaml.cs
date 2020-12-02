using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Diff.xaml
    /// </summary>
    public partial class Diff : Window
    {

        public ObservableCollection<ThreatDiff> diff = new ObservableCollection<ThreatDiff>();
        public Diff()
        {
            InitializeComponent();
            this.FontFamily = new FontFamily("Comic Sans MS");
            DiffDataGrid.ItemsSource = diff;
            foreach(var el in ((MainWindow)Application.Current.MainWindow).diff)
            {
                diff.Add(el);
            }
            Total.Text = $"Amount of updated records is - {diff.GroupBy(el => el.ID).Count()}";
        }

    }
}
