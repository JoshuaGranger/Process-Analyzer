using Stylet;
using System;
using System.Windows;
using TitaniumAS.Opc.Client.Common;
using TitaniumAS.Opc.Client.Da;

namespace Collect.Views
{
    public class ServerDialogViewModel : Screen
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
        private OpcDaServer _server;
        public OpcDaServer Server
        {
            get { return _server; }
            set { SetAndNotify(ref _server, value); }
        }

        public ServerDialogViewModel()
        {
            IPAddress = "localhost";
            ProgID = "";
        }

        #region Actions
        public async System.Threading.Tasks.Task Connect()
        {
            IPAddress = (IPAddress == "") ? "localhost" : IPAddress.TrimStart('\\');

            Uri url = UrlBuilder.Build(ProgID, IPAddress);
            try
            {
                Server = new OpcDaServer(url);
                Server.Connect();
                Save();
            }
            catch (Exception e)
            {
                Server = null;
                MessageBox.Show(e.Message);
            }
        }
        #endregion

        public void Save()
        {
            this.RequestClose(Server != null);
        }

        public void Close()
        {
            this.RequestClose(null);
        }
    }
}
