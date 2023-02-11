using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using uss_client_gui_v2.Commands;
using uss_client_sandbox.Models;

namespace uss_client_gui_v2.ViewModels
{
    public class BridgeStatusViewModel : BaseViewModel
    {
        public BridgeStatusViewModel()
        {
            this.ConnectCommand = new ConnectCommand(this);
            //this.ConnectCommand.Execute(this);
        }

        #region commands
        public ICommand ConnectCommand { get; }
        #endregion


        public string ConnectInfo
        {
            get
            {
                if (NetworkManager.ConnectionError) { return "No bridge connected"; }
                else return "Bridge connected at " + NetworkManager.IpAdress;
            }
        }

        public bool IsConnected { get { return NetworkManager.ConnectionError; } }

        public Visibility BridgeConnectedVisibility
        {
            get
            {
                if (IsConnected)
                {
                    return Visibility.Hidden;
                }
                else return Visibility.Visible;
            }
        }
        public Visibility BridgeDisconnectedVisibility
        {
            get
            {
                if (IsConnected)
                {
                    return Visibility.Visible;
                }
                else return Visibility.Hidden;
            }
        }

        public bool NavigateButtonsEnabled
        {
            get
            {
                if (IsConnected)
                {
                    return true;
                }
                else return false;
            }
        }

        public override void UpdateEntireUI()
        {
            OnPropertyChanged(nameof(ConnectInfo));
            OnPropertyChanged(nameof(IsConnected));
            OnPropertyChanged(nameof(BridgeDisconnectedVisibility));
            OnPropertyChanged(nameof(BridgeConnectedVisibility));
            OnPropertyChanged(nameof(NavigateButtonsEnabled));
        }
    }
}
