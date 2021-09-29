using Stylet;
using System;
using System.Reflection;

namespace Collect.Pages
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

        public AboutDialogViewModel()
        {
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //Version = Assembly.GetExecutingAssembly().GetName().Bui
        }

        public void Close()
        {
            this.RequestClose(null);
        }

        public void Save()
        {
            this.RequestClose(true);
        }
    }
}
