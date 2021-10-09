using Stylet;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Collect.Views.Dialogs
{
    public class AboutDialogViewModel : Screen
    {
        private string _version;
        public string Version
        {
            get { return _version; }
            set { SetAndNotify(ref _version, value); }
        }
        private string _buildDate;
        public string BuildDate
        {
            get { return _buildDate; }
            set { SetAndNotify(ref _buildDate, value); }
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetAndNotify(ref _description, value); }
        }

        public AboutDialogViewModel()
        {
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            
            var days = Assembly.GetExecutingAssembly().GetName().Version.Build;
            DateTime startDate = new DateTime(2000, 1, 1);
            BuildDate = startDate.AddDays(days).ToShortDateString();
            
            Description =
                "Collect is a real-time OPC data collection and trending tool. Features include exporting/importing tag sets, adjustable " +
                "data resolution (update rate), writing recorded data to file, and an interactive plot.\n\nFor questions, bugs, or feature requests, " +
                "please use the 'Issues' section on the project Github page.";
        }

        public void Navigate()
        {
            Process.Start(new ProcessStartInfo(@"https://github.com/JoshuaGranger/Process-Analyzer"));
        }

        public void Close()
        {
            this.RequestClose(null);
        }
    }
}
