using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uss_client_sandbox.Models;
using ussclientsandbox.Models;

namespace uss_client_gui.ViewModels
{
    public partial class ModesDashboardViewModel : BaseViewModel, INotifyPropertyChanged
    {

        private Mode _selectedMode;
        private Characteristic _selectedCharacteristic;
        private List<string> _enumVals;

        public ModesDashboardViewModel()
        {
            NetworkManager.Send(new BCMessage(BCCommand.GetModes, "null", 0));
            Thread.Sleep(1000);
            SelectedMode = LocalBridge.ModeList[0];
            SelectedCharacteristic = SelectedMode.CharacteristicsToMet[0];

            Title = "Mode Management";
            OnPropertyChanged("DisplayableType");

        }

        public ModeList ModeList 
        { 
            get 
            {
                NetworkManager.Send(new BCMessage(BCCommand.GetModes, "null", 0));
                Thread.Sleep(1000);
                return LocalBridge.ModeList;
            }
        }

        #region async
        [RelayCommand]
        async Task TrySendModesAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                if (!NetworkManager.ConnectionError)
                {
                    NetworkManager.Send(new BCMessage(BCCommand.GetModesRep, LocalBridge.ModeList.ToXML(), 0));
                }

            }
            catch (Exception) { }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task TryAddCharacteristicAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var chr = new Characteristic(CharacteristicType.Blank, 0, false);
                this.CharacteristicsToMet.Add(chr);
                OnPropertyChangedManual(nameof(CharacteristicsToMet));
                OnPropertyChangedManual(nameof(RemoveCharacteristicbuttonVisible));

            }
            catch (Exception) { }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task TryRemoveCharacteristicAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                if (this.CharacteristicsToMet.Count > 1)
                    this.CharacteristicsToMet.Remove(SelectedCharacteristic);


                OnPropertyChangedManual(nameof(CharacteristicsToMet));
                OnPropertyChangedManual(nameof(RemoveCharacteristicbuttonVisible));

            }
            catch (Exception) { }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task TryAddMode()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                this.PossibleModes.Add(new Mode((CharacteristicsToMet.Count + 1).ToString() + "mode"));

                OnPropertyChangedManual(nameof(PossibleModes));
            }
            catch (Exception) { }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task TryRemoveModeAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                if (this.PossibleModes.Count > 1)
                {
                    this.SelectedMode = PossibleModes[0];
                    this.PossibleModes.Remove(SelectedMode);
                    this.SelectedCharacteristic = CharacteristicsToMet[0];
                }
                    


                OnPropertyChangedManual(nameof(PossibleModes));
                OnPropertyChangedManual(nameof(SelectedMode));
                OnPropertyChangedManual(nameof(CharacteristicsToMet));
                OnPropertyChangedManual(nameof(SelectedCharacteristic));

            }
            catch (Exception) { }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion

        #region Properties
        public ModeList PossibleModes 
        { 
            get => LocalBridge.ModeList; 
            set 
            {
                LocalBridge.ModeList = value;
                OnPropertyChangedManual(nameof(PossibleModes));
            } 

        }
        public Mode SelectedMode 
        { 
            get => _selectedMode;
            set
            {
                _selectedMode = value;
                OnPropertyChangedManual(nameof(SelectedMode));
            }
        }
        public List<Characteristic> CharacteristicsToMet
        {
            get => SelectedMode.CharacteristicsToMet; 
            set
            {
                SelectedMode.CharacteristicsToMet = value;
                OnPropertyChangedManual(nameof(CharacteristicsToMet));
            }
        }

        public Characteristic SelectedCharacteristic
        { 
            get => _selectedCharacteristic; 
            set
            {
                _selectedCharacteristic = value;
                OnPropertyChangedManual(nameof(SelectedCharacteristic));
            } 
        }

        public bool RemoveCharacteristicbuttonVisible
        {
            get
            {
                if (CharacteristicsToMet.Count > 1) { return true; }
                else return false;
            }
        }
        #endregion

        #region event
        public event PropertyChangedEventHandler PropertyChangedManual;
        protected void OnPropertyChangedManual(string propertyName)
        {
            PropertyChangedManual?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool CheckBoxOnSingle { get => SelectedMode.OnSingle; set => SelectedMode.OnSingle = value; }
        #endregion
    }
}
