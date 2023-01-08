using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uss_client_sandbox.Models;
using ussclientsandbox.Models;

namespace uss_client_gui.ViewModels
{
    public partial class SwitchDashboardViewModel : BaseViewModel
    {
        private uss_client_sandbox.Models.Switch _selectedSwitch;

        public SwitchDashboardViewModel()
        {
            _selectedSwitch = new uss_client_sandbox.Models.Switch();
            //NetworkManager.Send(new BCMessage(BCCommand.GetSwitches, "null", 0));
            //Thread.Sleep(1000);
            NetworkManager.Send(new BCMessage(BCCommand.GetModes, "null", 0));
            Thread.Sleep(1000);
            _selectedSwitch = LocalBridge.SwitchList[0];

            Title = "Devices Overview";
        }

        public void TrySetSwitchState()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;

                if (!NetworkManager.ConnectionError)
                {
                    NetworkManager.Send(new BCMessage(BCCommand.SetSwitchState, SelectedSwitch.StateOn.ToString(), byte.Parse(SelectedSwitch.Address)));
                }
            }
            catch (Exception) { }
            finally
            {
                IsBusy = false;
            }
        }

        #region async
        [RelayCommand]
        async Task TrySendSwitchesAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                if (!NetworkManager.ConnectionError)
                {
                    NetworkManager.Send(new BCMessage(BCCommand.GetSwitchesRep, LocalBridge.SwitchList.ToXML(), 0));
                }

            }
            catch (Exception) { }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task TrySetSwitchStateAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;

                if (!NetworkManager.ConnectionError)
                {
                    NetworkManager.Send(new BCMessage(BCCommand.SetSwitchState,SelectedSwitch.StateOn.ToString(),byte.Parse(SelectedSwitch.Address)));
                }
            }
            catch (Exception) { }
            finally
            {
                IsBusy = false;
                
            }
        }
        #endregion


        public SwitchList AvailableSwitches { get => LocalBridge.SwitchList; }
        public uss_client_sandbox.Models.Switch SelectedSwitch { get => _selectedSwitch; set => _selectedSwitch = value; }
    }
}
