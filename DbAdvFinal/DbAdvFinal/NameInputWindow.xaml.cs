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
    /// NameInputWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NameInputWindow : Window
    {

        public NameInputWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           

            GenderSelectWindow GenderWindow = new GenderSelectWindow();
            GenderWindow.InfoName = FirstName.Text;
            App.Current.MainWindow = GenderWindow;
            App.Current.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Close();
            GenderWindow.Show();
        }

        private void FirstName_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= FirstName_GotFocus;

        }

        private void LastName_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= LastName_GotFocus;
        }
    }
}
