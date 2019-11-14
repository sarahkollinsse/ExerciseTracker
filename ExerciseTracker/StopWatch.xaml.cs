using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace ExerciseTracker
{
    public partial class StopWatch : ContentPage
    {
        Stopwatch myStopWatch = new Stopwatch();
        TimeSpan timeSpan = new TimeSpan();
        public StopWatch()
        {
            InitializeComponent();
            timerLabel.Text = Preferences.Get("time", myStopWatch.Elapsed.ToString());
            if (!timerLabel.Text.Equals("00:00:00"))
            {
                resetButton.IsVisible = true;
            }
           // timerLabel.SetBinding(Label.TextProperty, new Binding {Source=myStopWatch, Path=myStopWatch.Elapsed.ToString()});
            BindingContext = this;
           
        }
        public void OnStartClicked(object sender, EventArgs args)
        {
            Device.StartTimer(new TimeSpan(0,0,0,0,100), OnTimerExpired);
            Binding binding = new Binding();
            binding.Mode = BindingMode.TwoWay;
            binding.Source = myStopWatch;
            binding.Path = myStopWatch.Elapsed.ToString();
            timerLabel.SetBinding(Label.TextProperty, binding);
            resetButton.IsVisible = false;
            startButton.IsVisible = false;
            stopButton.IsVisible = true;
            myStopWatch.Start();
         
          
        }
        public bool OnTimerExpired()
        {
            timeSpan = myStopWatch.Elapsed;
            timerLabel.Text = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds,
            timeSpan.Milliseconds / 10);
            return true;

        }
        public void OnStopClicked(object sender, EventArgs args)
        {
            myStopWatch.Stop();
            
            resetButton.IsVisible = true;
            startButton.IsVisible = true;
            stopButton.IsVisible = false;
            timeSpan = myStopWatch.Elapsed;
            timerLabel.Text = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds,
            timeSpan.Milliseconds / 10);
            Preferences.Set("time", timerLabel.Text);

        }
        public void OnResetClicked(object sender, EventArgs args)
        {
            myStopWatch.Reset();
            stopButton.IsVisible = false;
            resetButton.IsVisible = false;
            timerLabel.Text = myStopWatch.Elapsed.ToString();
            Preferences.Set("time", timerLabel.Text);


        }
    }
}
