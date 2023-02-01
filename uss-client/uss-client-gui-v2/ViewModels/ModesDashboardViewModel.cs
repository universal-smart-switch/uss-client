using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ModesDashboardViewModel : BaseViewModel
    {
        #region fields
        private Mode selectedMode;
        private Characteristic _selectedCharacteristic;
        #endregion

        #region ctor
        public ModesDashboardViewModel()
        {
            this.TryAddCharacteristicCommand = new TryAddCharacteristicCommand(this);
            this.TrySendModesCommand = new SendModesCommand();
            this.TryAddModeCommand = new AddModeCommand(this);
            this.TryRemoveModeCommand = new RemoveModeCommand(this);
            this.TryRemoveCharacteristicCommand = new TryRemoveCharacteristicCommand(this);


            Thread.Sleep(2000);

            /*while (PossibleModes.Count == 0)
            {
                // try getting switch list 
                //NetworkManager.Send(new BCMessage(BCCommand.GetModes, "null", 0));
                //Thread.Sleep(2000);
                
            }*/

            if (PossibleModes.Count > 0) { SelectedMode = PossibleModes[0]; }
            
        }
        #endregion

        #region methods
        public override void UpdateEntireUI()
        {
            if (SelectedMode == null) { SelectedMode = PossibleModes[0]; }


            OnPropertyChanged(nameof(SelectedMode));
            OnPropertyChanged(nameof(CheckBoxOnSingle));
            OnPropertyChanged(nameof(CheckBoxTurnOff));
            OnPropertyChanged(nameof(PossibleModes));
            OnPropertyChanged(nameof(SelectedCharacteristic));
            OnPropertyChanged(nameof(RemoveCharacteristicbuttonVisible));
            OnPropertyChanged(nameof(RemoveModeButtonVisible));
            OnPropertyChanged(nameof(ModeCharacteristics));

        }
        #endregion

        #region commands
        public ICommand TryAddCharacteristicCommand { get; set; }
        public ICommand TrySendModesCommand { get; set; }
        public ICommand TryRemoveCharacteristicCommand { get; set; }
        public ICommand TryAddModeCommand { get; set; }
        public ICommand TryRemoveModeCommand { get; set; }
        #endregion

        #region properties

        public bool CheckBoxOnSingle 
        { 
            get
            {
                if (    (SelectedMode != null)  && SelectedMode.OnSingle != null) { return SelectedMode.OnSingle; }
                else return false;
            }
            set => SelectedMode.OnSingle = value; 
        }
        public bool CheckBoxTurnOff 
        {
            get
            {
                if (    (SelectedMode != null) &&SelectedMode.Invert != null) { return SelectedMode.Invert; }
                else return false;
            }
            set => SelectedMode.Invert = value;
        }
        public bool RemoveCharacteristicbuttonVisible
        {
            get
            {
                if (    (SelectedMode != null) && (SelectedMode.CharacteristicsToMet != null) && (SelectedMode != LocalBridge.ModeList[0] ))
                {
                    if (SelectedMode.CharacteristicsToMet.Count > 1)
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
   
            }
        }
        public bool RemoveModeButtonVisible
        {
            get
            {
                if ((SelectedMode != null) && !SelectedMode.Name.Contains("Default"))
                {
                    return true;
                }
                else return false;
            }
        }
        public ObservableCollection<Mode> PossibleModes { get { return LocalBridge.ModeList; } }

        public Mode SelectedMode 
        { 
            get => selectedMode;
            set 
            {
                selectedMode = value;
                OnPropertyChanged(nameof(SelectedMode));
                UpdateEntireUI();
            } 
        }

        public Characteristic SelectedCharacteristic 
        { 
            get => _selectedCharacteristic; 
            set
            {
                _selectedCharacteristic = value;
                UpdateEntireUI();
            } 
        }

        public ObservableCollection<Characteristic> ModeCharacteristics { get => SelectedMode.CharacteristicsToMet; }
        #endregion
    }
}
