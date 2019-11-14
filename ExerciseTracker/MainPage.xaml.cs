using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using Xamarin.Essentials;
using System.Reflection;
using System.IO;

namespace ExerciseTracker
{
    [Table ("men")]
    public class Men
    {
        public string Race { get; set; }
        public string Time { get; set; }
        public string Age { get; set; }
    }

    [Table("women")]
    public class Women
    {
        public string Race { get; set; }
        public string Time { get; set; }
        public string Age { get; set; }
    }
    
    public partial class MainPage : TabbedPage
    {
        SQLiteConnection conn;
        Settings setting;
        public MainPage()
        {
            InitializeComponent();
            setting = new Settings();
            CalculateAgeGroup();

            //Create DB
            string libFolder = FileSystem.AppDataDirectory;
            string fname = System.IO.Path.Combine(libFolder, "Personnel.db");
            conn = new SQLiteConnection(fname);
            conn.CreateTable<Men>();
            conn.CreateTable<Women>();

            readFiles();
          var list = conn.Table<Men>().ToList();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            CalculateAgeGroup();
        }
        //Reads womens.txt and mens.txt and adds to the Men and Women tables in database
        public void readFiles()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("ExerciseTracker.mens.txt");
            string text = "";
            string[] age = { "Distance","Open", "40", "45", "50", "55", "60", "65", "70", "75", "80", "85", "90", "95", "100" };
            using (var menReader = new System.IO.StreamReader(stream))
            {
                while (!menReader.EndOfStream)
                {
                    Men newMan;
                    text = menReader.ReadLine();
                    string[] times = text.Split(',');
                    if (!text.Contains("Distance"))
                    {
                        for (int i = 2; i < times.Length; i++)
                        {
                            newMan = new Men { Race = times[0], Age = age[i], Time = times[i] };
                            conn.Insert(newMan);
                        }
                    }    
                }//end while  
            }
            Stream stream2 = assembly.GetManifestResourceStream("ExerciseTracker.womens.txt");
            string text2 = "";
            using (var womenReader = new System.IO.StreamReader(stream2))
            {
                while (!womenReader.EndOfStream)
                {
                    Women newWoman;
                    text2 = womenReader.ReadLine();
                    string[] times = text2.Split(',');
                    if (!text2.Contains("Distance"))
                    {
                        for (int i = 2; i < times.Length; i++)
                        {
                            newWoman = new Women { Race = times[0], Age = age[i], Time = times[i] };
                            conn.Insert(newWoman);
                        }
                    }
                }//end while
            }
        }
     
        public void RunPickerChanged(object sender, EventArgs args)
        {
            if (runPicker.SelectedItem != null)
            {
                warning.Text = "";
                CalculateAgeGroup();
                getAgeGrade();
            }
        }
        public void OnTimeChanged(object sender, EventArgs args)
        {
            CalculateAgeGroup();
            getAgeGrade();
           
        }
        public void getAgeGrade()
        {
            if (runPicker.SelectedItem != null && !ageGroup.Text.Equals("You are too young"))
            {
                double yourTime = getYourTimeFormat();
                double grade = 0;
                double record = 0;
                warning.Text = "";

                if (setting.getGender().Equals("Female"))
                {

                    var query = from w in conn.Table<Women>()
                                where w.Race.Equals(runPicker.SelectedItem) && w.Age.Equals(ageGroup.Text)
                                select w.Time;
                    var list = query.ToList();
                    string recordTime = list[0];
                    if (!recordTime.Equals("n/a"))
                    {
                        record = getRecordTimeFormat(recordTime);
                        grade = (record / yourTime) * 100;
                        ageGrade.Text = grade.ToString();
                    }
                    else
                    {
                        ageGrade.Text = "n/a";
                    }
                }
                else if (setting.getGender().Equals("Male"))
                {
                    var query = from m in conn.Table<Men>()
                                where m.Race.Equals(runPicker.SelectedItem) && m.Age.Equals(ageGroup.Text)
                                select m.Time;
                    var list = query.ToList();
                    string recordTime = list[0];
                    if (!recordTime.Equals("n/a"))
                    {
                        record = getRecordTimeFormat(recordTime);
                        grade = (record / yourTime) * 100;
                        ageGrade.Text = grade.ToString();
                    }
                    else
                    {
                        ageGrade.Text = "n/a";
                    }
                }
                else
                {
                    warning.Text = "Please select a gender in Settings";
                }


            }
            else
            {
                warning.Text = "Please select a run type";
            }
        }
        public double getRecordTimeFormat(string recordTime)
        {
            double record = 0;
           
            int colonIndex = recordTime.IndexOf(":");
            int lastcolon = recordTime.LastIndexOf(":");
            record = Double.Parse(recordTime.Substring(0, colonIndex))*60;
            if (colonIndex != lastcolon)
            {
                record = record * 60;
                string temp = recordTime.Substring(colonIndex + 1);
                colonIndex = temp.IndexOf(":");
                record = record + (Double.Parse(temp.Substring(0, colonIndex)) * 60);
            }
            else
            {
                double sec = Double.Parse(recordTime.Substring(colonIndex + 1, 2));
                record = record + sec;
            }
            return record;

        }
        public double getYourTimeFormat()
        {
            double yourTime=0;

            double h=0;
            double m=0;
            double s=0;
            if (!mins.Text.Equals(""))
            {
                m = Double.Parse(mins.Text);
            }
            if (!seconds.Text.Equals(""))
            {
                s = Double.Parse(seconds.Text);
            }
            if (hours.Text.Equals(""))
            {

               yourTime = (m*60) + s;
            }
            else
            {
                h = Double.Parse(hours.Text);
                yourTime = (h*36000) + (m*60)+ s;
            }
           
         
            return yourTime;
        }
        public void CalculateAgeGroup()
        {
            String currentYear = DateTime.Today.ToString("yyyy");
            int age = Int32.Parse(currentYear) - Int32.Parse(setting.getDOBYear());
            int div = age / 5;
            int groupNum = 5 * div;
            if (groupNum < 40)
            {
                ageGroup.Text = "You are too young";
            }
            else
            {
                ageGroup.Text = groupNum.ToString();
            }
        }
    }//end class
}//end namespace
