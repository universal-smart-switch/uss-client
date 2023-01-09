using uss_client_gui_v2.Commands;
using System.Windows.Input;
using uss_client_gui_v2.Stores;

namespace uss_client_gui_v2.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        #region fields
        
        #endregion

        #region ctor
        public MainViewModel()
        {
            NavigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }
        #endregion

        #region events
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public override void UpdateEntireUI()
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region command
        public ICommand CloseCommand { get; }
        #endregion
        #region properties
        public BaseViewModel CurrentViewModel { get => NavigationStore.CurrentViewModel; } //currently used viewmodel -> Mainwindow chooses view depending on VM
        #endregion

    }
}
