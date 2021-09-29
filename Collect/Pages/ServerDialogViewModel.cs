using Stylet;
using System;

namespace Collect.Pages
{
    public class ServerDialogViewModel : Screen
    {
        public string Name { get; set; }

        public ServerDialogViewModel()
        {
            this.DisplayName = "I'm Dialog 1";
        }

        #region Actions
        public async System.Threading.Tasks.Task Connect()
        {
            
        }
        #endregion

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
