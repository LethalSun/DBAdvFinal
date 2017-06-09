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
    /// YearInputWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    ///     
    /// 

public partial class YearInputWindow : Window
    {
        private string m_infoName;

        public string InfoName
        {
            get { return this.m_infoName; }
            set { this.m_infoName = value; }
        }

        private string m_infoGender;

        public string InfoGender
        {
            get { return this.m_infoGender; }
            set { this.m_infoGender = value; }
        }



        public YearInputWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectRanking rankWindow = new SelectRanking();
            rankWindow.InfoName = m_infoName;
            rankWindow.InfoGender = m_infoGender;
            
            var yearNum = new int();

            MessageSpace.Text = "기다려 주세요...";

            if (Int32.TryParse(Year.Text,out yearNum))
            {
                rankWindow.InfoYear = yearNum;
                await rankWindow.GetFromMongo();
                App.Current.MainWindow = rankWindow;
                App.Current.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
                this.Close();
                rankWindow.Show();
            }
            else
            {
                MessageBox.Show("올바를 숫자를 입력해 주세요.");
                return;
            }

            
        }

        private void Year_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= Year_GotFocus;
        }
    }
}
