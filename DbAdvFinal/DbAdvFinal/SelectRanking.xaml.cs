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
    /// SelectRanking.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SelectRanking : Window
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

        private int m_infoYear;

        public int InfoYear
        {
            get { return this.m_infoYear; }
            set { this.m_infoYear = value; }
        }

        

        public async Task GetFromMongo()
        {
            var mongo = new MongoDBManager("211.249.60.69", 27017, "next", "next!!@@##$$", "baybName");

            mongo.SetCollection("nationalBabyName");

            mongo.ClearFilter();
            mongo.AddFIlterStringRegex("Name", InfoName.Substring(0,3));
            mongo.AddFilterStringEq("Gender", InfoGender);
            mongo.AddFilterEqInt("Year", InfoYear);

            var part1 = await mongo.GetResult();

            part1.Sort(delegate (BabyNameInfo A, BabyNameInfo B)
            {
                if (A.Count > B.Count)
                {
                    return -1;
                }
                else if (A.Count < B.Count)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            });

            mongo.ClearFilter();
            mongo.AddFIlterStringRegex("Name", InfoName.Substring(0, 2));
            mongo.AddFilterStringEq("Gender", InfoGender);
            mongo.AddFilterEqInt("Year", InfoYear);

            var part2 = await mongo.GetResult();

            part2.Sort(delegate (BabyNameInfo A, BabyNameInfo B)
            {
                if (A.Count > B.Count)
                {
                    return -1;
                }
                else if (A.Count < B.Count)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            });
            mongo.ClearFilter();
            mongo.AddFIlterStringRegex("Name", InfoName.Substring(0, 1));
            mongo.AddFilterStringEq("Gender", InfoGender);
            mongo.AddFilterEqInt("Year", InfoYear);

            var part3 = await mongo.GetResult();

            part3.Sort(delegate (BabyNameInfo A, BabyNameInfo B)
            {
                if (A.Count > B.Count)
                {
                    return -1;
                }
                else if (A.Count < B.Count)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            });
            var tempList = new List<BabyNameInfo>();

            tempList.AddRange(part1);

            foreach (var info in part2)
            {
                if (!tempList.Exists(x => x.Name == info.Name))
                {
                    tempList.Add(info);
                }
            }

            foreach (var info in part3)
            {
                if (!tempList.Exists(x =>x.Name == info.Name))
                {
                    tempList.Add(info);
                }
            }

            var item = new List<BabyNameInfo>();
            item.AddRange(tempList.Distinct());

            ResultView.ItemsSource = item;
        }

        public SelectRanking()
        {
            InitializeComponent();
            
        }

        private async void Double_Click(object sender, MouseButtonEventArgs e)
        {
            var row = (BabyNameInfo)ResultView.SelectedItems[0];
            var ResultWindow = new Result();
            ResultWindow.InfoName = row.Name;
            ResultWindow.InfoGender = row.Gender;
            ResultWindow.InfoYear = row.Year;
            output.Text = "기다려 주세요...";
            await ResultWindow.GetFromMongo();
            ResultWindow.Plot();
            App.Current.MainWindow = ResultWindow;
            App.Current.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Close();
            ResultWindow.Show();

        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    if(m_startIndex == 0)
        //    {
        //        return;
        //    }
        //
        //    var item = new List<BabyNameInfo>();
        //    item.AddRange(m_item.GetRange(m_startIndex, 10).ToList());
        //    Result.ItemsSource = item;
        //}
        //
        //public void Button_Click_1()
        //{
        //    var item = new List<BabyNameInfo>();
        //    int i = 0;
        //    var ff = new BabyNameInfo();
        //    ff.Name = InfoName;
        //    ff.Count = 100;
        //    ff.Gender = InfoGender;
        //    ff.Year = InfoYear;
        //
        //    while(i<100)
        //    {
        //        item.Add(ff);
        //        ++i;
        //    }
        //   
        //    Result.ItemsSource = item;
        //}
    }
}
