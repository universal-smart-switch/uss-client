﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using uss_client_gui_v2.Commands;
using uss_client_sandbox.Models;
using ussclientsandbox.Models;

namespace uss_client_gui_v2.ViewModels
{
    public class SwitchDashboardViewModel : BaseViewModel
    {
        #region fields
        private Switch _selectedSwitch;
        private bool _firstTimeLanding = true;
        private int oldSwitchListCount = 0;
        #endregion

        #region ctor
        public SwitchDashboardViewModel()
        {
            oldSwitchListCount = LocalBridge.SwitchList.Count;
            this.SaveModeSettingsCommand = new SendSwitchesCommand();
        }
        #endregion

        #region methods
        public void RequestUpdate()
        {
            NetworkManager.Send(new BCMessage(BCCommand.GetSwitches, "null", 0));
            Thread.Sleep(2000);
            _firstTimeLanding = false;
            UpdateEntireUI();
        }
        public override void UpdateEntireUI()
        {
            OnPropertyChanged(nameof(AvailableSwitches));
            OnPropertyChanged(nameof(SelectedSwitch));
            OnPropertyChanged("displayState");


            if (oldSwitchListCount != LocalBridge.SwitchList.Count)
            {
                foreach (var item in LocalBridge.SwitchList)
                {
                    item.SetStateCommand = new SetSwitchStateCommand(item,this);
                }
            }
        }
        #endregion

        #region commands
        public ICommand SaveModeSettingsCommand { get; set; }
        #endregion

        #region properties
        public SwitchList AvailableSwitches 
        { 
            get
            {
                if (_firstTimeLanding) 
                { 
                    //RequestUpdate();
                    _firstTimeLanding = false; 
                }
                if (oldSwitchListCount != LocalBridge.SwitchList.Count) 
                {
                    foreach (var item in LocalBridge.SwitchList)
                    {
                        item.SetStateCommand = new SetSwitchStateCommand(item,this);
                    }
                }
                return LocalBridge.SwitchList;
            }
        }
        public uss_client_sandbox.Models.Switch SelectedSwitch { get => _selectedSwitch; set => _selectedSwitch = value; }
        #endregion
    }
}
