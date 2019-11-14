using System;
using System.Collections.Generic;
using Xamarin.Essentials;

using Xamarin.Forms;

namespace ExerciseTracker
{
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            
            dobPicker.Date = Preferences.Get("dob",DateTime.Today);
            genderPicker.SelectedItem = Preferences.Get("gender", "");
        }
        //When credit button is clicked it will open Miami University's webpage
        public void OnCreditClicked(object sender, EventArgs args)
        {
            Device.OpenUri(new Uri("https://www.miamioh.edu"));
        }
        //When the gender picker is changed it will permantely save the value
        public void GenderPickerChanged(object sender, EventArgs args)
        {
            if(genderPicker.SelectedItem!= null)
            {
                Preferences.Set("gender", genderPicker.SelectedItem.ToString());
            }            
        }
        //When the DOB picker is changed it will permanetly save the value
        public void DOBPickerChanged(object sender, EventArgs args)
        {
            Preferences.Set("dob", dobPicker.Date);
        }
        public String getDOBYear()
        {
            dobPicker.Date = Preferences.Get("dob", DateTime.Today);
            String year = dobPicker.Date.ToString("yyyy");
            return year;
        }
        public String getGender()
        {
            genderPicker.SelectedItem = Preferences.Get("gender", "");
            return genderPicker.SelectedItem.ToString();
        }
    }
}
