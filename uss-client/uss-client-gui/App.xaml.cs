using System.ComponentModel;
using System.Net.Sockets;
using uss_client_sandbox.Models;
using ussclientsandbox.Models;

namespace uss_client_gui
{
    public partial class App : Application, INotifyPropertyChanged
    {
        private bool _otherTabsVisible;

        public bool OtherTabsVisible
        {
            get { return _otherTabsVisible; }
            set 
            { 
                _otherTabsVisible = value; 
                OnPropertyChanged(nameof(OtherTabsVisible));
            }
        }

        public App()
        {
            var token = new CancellationTokenSource();
            NetworkManager.ConnectFirstTime();
            OtherTabsVisible = !NetworkManager.ConnectionError;

            /*
             
            SwitchList swiTest = new SwitchList();

            uss_client_sandbox.Models.Switch sw = new uss_client_sandbox.Models.Switch();
            sw.Address = "addr";
            sw.Name = "switchy";
            swiTest.Add(sw);
            var swLR = new BCMessage(BCCommand.GetSwitchesRep, swiTest.ToXML(), 0);
            NetworkManager.Send(swLR);
            */

            InitializeComponent();
            MainPage = new AppShell();


        }

        #region event
        public event PropertyChangedEventHandler PropertyChangedManual;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedManual?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}