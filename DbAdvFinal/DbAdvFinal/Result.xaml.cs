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
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace DbAdvFinal
{
    /// <summary>
    /// Result.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 

    public partial class Result : Window
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

        List<BabyNameInfo> m_infos;

        double m_genderRatio;

        //OXYPLOT
        public PlotModel Model { get; set; }

        double m_xMin;
        double m_xMax;
        double m_yMin;
        double m_yMax;

        public async Task GetFromMongo()
        {
            var mongo = new MongoDBManager("211.249.60.69", 27017, "next", "next!!@@##$$", "baybName");

            mongo.SetCollection("nationalBabyName");

            mongo.ClearFilter();
            mongo.AddFIlterStringRegex("Name", InfoName);
            mongo.AddFilterStringEq("Gender", InfoGender);

            var part1 = await mongo.GetResult();

            part1.Sort(delegate (BabyNameInfo A, BabyNameInfo B)
            {
                if (A.Year > B.Year)
                {
                    return 1;
                }
                else if (A.Year < B.Year)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            });

            var tempList = new List<BabyNameInfo>();
            tempList.AddRange(part1);

            m_infos = new List<BabyNameInfo>();
            m_infos.AddRange(tempList.Distinct());

            mongo.ClearFilter();
            mongo.AddFIlterStringRegex("Name", InfoName);
            mongo.AddFilterEqInt("Year", InfoYear);

            var part2 = await mongo.GetResult();

            if(part2.Count == 2)
            {
                var MaxNum = part2[0].Count + part2[1].Count;
                if(part2[0].Gender != part2[1].Gender)
                {
                    if(part2[0].Gender == "F")
                    {
                        m_genderRatio = part2[0].Count / (double)MaxNum;
                    }
                    else
                    {
                        m_genderRatio = part2[1].Count / (double)MaxNum;
                    }

                    if(m_infoGender == "M")
                    {
                        m_genderRatio = 1 - m_genderRatio;
                    }
                    else
                    {
                        //아무것도 안함
                    }

                }
                else
                {
                    m_genderRatio = 1;
                }

            }
            else
            {
                m_genderRatio = 1;
            }
            
        }

        public Result()
        {
            

            InitializeComponent();
           
        }

        public void Plot()
        {
            SetData();
            string str;
            if(m_infoGender == "M")
            {
                str = string.Format("{0} 의 남성도 는 {1}/100 입니다.", m_infoName, m_genderRatio * 100);
            }
            else
            {
                str = string.Format("{0} 의 여성도 는 {1}/100 입니다.", m_infoName, m_genderRatio * 100);
            }
            this.Model = CreateNormalDistributionModel(m_xMin, m_xMax, m_yMin, m_yMax, m_infos ,m_infoName, str);
            this.DataContext = this;
        }

        private void SetData()
        {
            m_xMax = m_infos.Max(x => x.Year);
            m_xMin = m_infos.Min(x => x.Year);

            m_yMax = m_infos.Max(x => x.Count);
            m_yMin = m_infos.Min(x => x.Count);

        }
       

        private static PlotModel CreateNormalDistributionModel(double xmin, double xmax, double ymin, double ymax, List<BabyNameInfo> list, string name, string sub)
        {
            var plot = new PlotModel
            {
                Title = "인기도 변화량",
                Subtitle = sub
            };

            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = ymin,
                Maximum = ymax,
                MajorStep = 20,
                MinorStep = 100,
                TickStyle = TickStyle.Inside
            });
            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = xmin,
                Maximum = xmax,
                MajorStep = 10,
                MinorStep = 100,
                TickStyle = TickStyle.Inside
            });
            plot.Series.Add(CreateNormalDistributionSeries(list, name));
            return plot;
        }

        private static LineSeries CreateNormalDistributionSeries(List<BabyNameInfo> list , string name)
        {
            var ls = new LineSeries
            {
                Title = name
            };

            foreach (var info in list)
            {
                double x = info.Year;
                double f = info.Count;
                ls.Points.Add(new DataPoint(x, f));
            }

            return ls;
        }

    }
}
