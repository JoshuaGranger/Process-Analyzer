using Stylet;
using System;

namespace Collect.Pages
{
    public class DatalogDialogViewModel : Screen
    {
        private string _ipAddress;
        public string IPAddress
        {
            get { return _ipAddress; }
            set { SetAndNotify(ref _ipAddress, value); }
        }
        private string _progID;
        public string ProgID
        {
            get { return _progID; }
            set { SetAndNotify(ref _progID, value); }
        }

        public DatalogDialogViewModel()
        {
            this.DisplayName = "I'm Dialog 1";
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
