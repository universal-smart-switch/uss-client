using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
            base.OnPropertyChanged("DisplayableType");

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
                

            }
            catch (Exception) { }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(CharacteristicsToMet));
                OnPropertyChanged(nameof(RemoveCharacteristicbuttonVisible));
                updateUI = true;
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


                

            }
            catch (Exception) { }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(CharacteristicsToMet));
                OnPropertyChanged(nameof(RemoveCharacteristicbuttonVisible));
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

            }
            catch (Exception) { }
            finally
            {
                IsBusy = false;

                OnPropertyChanged(nameof(PossibleModes));
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
                    


                OnPropertyChanged(nameof(PossibleModes));
                OnPropertyChanged(nameof(SelectedMode));
                OnPropertyChanged(nameof(CharacteristicsToMet));
                OnPropertyChanged(nameof(SelectedCharacteristic));

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
                OnPropertyChanged(nameof(PossibleModes));
            } 

        }
        public Mode SelectedMode 
        { 
            get => _selectedMode;
            set
            {
                _selectedMode = value;
                OnPropertyChanged(nameof(SelectedMode));
            }
        }
        public List<Characteristic> CharacteristicsToMet
        {
            get => SelectedMode.CharacteristicsToMet; 
            set
            {
                SelectedMode.CharacteristicsToMet = value;
                OnPropertyChanged(nameof(CharacteristicsToMet));
            }
        }

        public Characteristic SelectedCharacteristic
        { 
            get => _selectedCharacteristic; 
            set
            {
                _selectedCharacteristic = value;
                OnPropertyChanged(nameof(SelectedCharacteristic));
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

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(RemoveCharacteristicbuttonVisible))]
        [NotifyPropertyChangedFor(nameof(SelectedCharacteristic))]
        [NotifyPropertyChangedFor(nameof(CharacteristicsToMet))]
        [NotifyPropertyChangedFor(nameof(SelectedMode))]
        [NotifyPropertyChangedFor(nameof(PossibleModes))]
        private bool updateUI;
        #endregion

        #region event
        /*public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var tempHandler = PropertyChanged; //für den unwahrscheinlichen - aber möglichen Fall, dass der
                                               //Eventhandler genau jetzt entfernt wird, wird der Funktionszeiger
                                               //behalten, damit nicht ein ungültiger Codebereich aufgerufen wird

            if (PropertyChanged != null) //Abfrage: Wurde dieses Ereignis abonniert bzw. beim Eventhandler hinterlegt?
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        }*/
        public bool CheckBoxOnSingle { get => SelectedMode.OnSingle; set => SelectedMode.OnSingle = value; }
        #endregion
    }
}
