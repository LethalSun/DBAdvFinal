using System;
using System.Collections.Generic;
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

namespace DbAdvFinal
{
    /// <summary>
    /// GenderSelectWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GenderSelectWindow : Window
    {
        private string m_infoName;

        public string InfoName
        {
            get { return this.m_infoName; }
            set { this.m_infoName = value; }
        }

        public GenderSelectWindow()
        {
            InitializeComponent();
        }

        private void showStopsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            WomanButton.IsChecked = false;
        }

        private void WomanButton_Checked(object sender, RoutedEventArgs e)
        {
            ManButton.IsChecked = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            YearInputWindow yearWindow = new YearInputWindow();
            yearWindow.InfoName = m_infoName;

            if(ManButton.IsChecked == true)
            {
                yearWindow.InfoGender = "M";
            }
            else if(WomanButton.IsChecked == true)
            {
                yearWindow.InfoGender = "F";
            }
            else
            {
                MessageBox.Show("성별을 선택해 주세요.");
                return;
            }

            
            App.Current.MainWindow = yearWindow;
            App.Current.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Close();
            yearWindow.Show();
        }
    }
}
