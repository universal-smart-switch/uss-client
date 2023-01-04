using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using uss_client_sandbox.Models;
using ussclientsandbox.Models;

namespace uss_client_gui.ViewModels
{
    public partial class BridgeStatusViewModel : BaseViewModel
    {
        #region ctor
        public BridgeStatusViewModel()
        {
            Title = "Bridge Status";
        }
        #endregion

        #region async
        [RelayCommand]
        async Task TryConnectAsync()
        {
            if (IsBusy) return;

            try 
            { 
                IsBusy = true;
                NetworkManager.Connect();

                if (!NetworkManager.ConnectionError)
                {
                    NetworkManager.Send(new BCMessage(BCCommand.GetSwitches, "null", 0));
                    NetworkManager.Send(new BCMessage(BCCommand.GetModes, "null", 0));
                }

            }
            catch (Exception) {  }
            finally 
            { 
                IsBusy = false;
                OnPropertyChanged(nameof(NoConnectionVisible));
                OnPropertyChanged(nameof(ConnectionVisible));
            }
        }
        #endregion
            

        #region properties
        public bool NoConnectionVisible { get => NetworkManager.ConnectionError;}
        public bool ConnectionVisible { get => !NoConnectionVisible; }
        public string BridgeIp { get => NetworkManager.IpAdress; }
        #endregion
    }
}
