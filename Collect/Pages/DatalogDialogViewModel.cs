using Stylet;
using System;

namespace Collect.Pages
{
    public class DatalogDialogViewModel : Screen
    {
        public string Name { get; set; }

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
