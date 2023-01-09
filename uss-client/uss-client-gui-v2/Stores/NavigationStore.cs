using uss_client_gui_v2.ViewModels;
using System;

namespace uss_client_gui_v2.Stores
{
    /// <summary>
    /// Stores the current viewmodel
    /// </summary>
    public static class NavigationStore
    {
        private static BaseViewModel _currentViewModel;

        public static BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        public static event Action CurrentViewModelChanged;

        private static void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
