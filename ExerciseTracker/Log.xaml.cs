using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ExerciseTracker
{
    public partial class Log : ContentPage
    {
        List<string> log = new List<string>();
        List<string> reverseList = new List<string>();
        string filename;
        public Log()
        {
            InitializeComponent();
            filename = GetFullFileName("log.txt");
           // System.IO.File.WriteAllText(filename, string.Empty);
           // Preferences.Clear();
            totalMiles.Text = Preferences.Get("miles", "0");
            readFile();
            logList.ItemsSource = log;
        }
        public void AddRunClicked(object sender, EventArgs args)
        {
            if (!distanceEntry.Text.Equals(""))
            {
                log.Clear();
                logList.ItemsSource = null;
                int miles = Int32.Parse(totalMiles.Text) + Int32.Parse(distanceEntry.Text);
                totalMiles.Text = miles.ToString();
                Preferences.Set("miles", totalMiles.Text);

                StreamWriter writer = new StreamWriter(filename, append: true);
                writer.WriteLine(DateTime.Today.ToString("MM/dd/yyyy") + " " + distanceEntry.Text + " miles");
                writer.Close();

                //log.Add(DateTime.Today.ToString("MM/dd/yyyy") + " " + distanceEntry.Text + " miles");

                readFile();
                logList.ItemsSource = log;
            }
           
        }

        public static string GetFullFileName(string basename)
        {
            string libFolder = FileSystem.AppDataDirectory;
            string fname = System.IO.Path.Combine(libFolder, basename);
            return fname;
        }
        public void readFile()
        {
            StreamReader reader = new StreamReader(filename);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                log.Add(line);
            }
            reader.Close();
        }
    }//end class
}//end namespace
