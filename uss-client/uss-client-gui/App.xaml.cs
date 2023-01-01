using uss_client_sandbox.Models;
using ussclientsandbox.Models;

namespace uss_client_gui
{
    public partial class App : Application
    {
        public App()
        {
            var token = new CancellationTokenSource();
            NetworkManager.ConnectFirstTime();

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
    }
}