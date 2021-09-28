using ScottPlot;
using Stylet;
using System;

namespace Collect.Pages
{
    public class ShellViewModel : Screen
    {
        private IWindowManager windowManager;
        private IDialogFactory dialogFactory;

        private WpfPlot _wpfPlot;
        public WpfPlot WpfPlot
        {
            get { return _wpfPlot; }
            set { SetAndNotify(ref _wpfPlot, value); }
        }

        public ShellViewModel(IWindowManager windowManager, IDialogFactory dialogFactory)
        {
            this.DisplayName = "Hello Dialog";
            WpfPlot = new WpfPlot();
            WpfPlot.Plot.AddScatter(new double[] { 0, 1, 2, 3 }, new double[] { 0, 10, 2, 3 });
            WpfPlot.Refresh();
            this.windowManager = windowManager;
            this.dialogFactory = dialogFactory;
        }

        public async System.Threading.Tasks.Task ShowDialog()
        {
            var dialogVm = this.dialogFactory.CreateDatalogDialog();
            var result = this.windowManager.ShowDialog(dialogVm);
        }
    }

    public interface IDialogFactory
    {
        ServerDialogViewModel CreateServerDialog();
        DatalogDialogViewModel CreateDatalogDialog();
    }
}
