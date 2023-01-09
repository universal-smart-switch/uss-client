using System;
using System.ComponentModel;

namespace uss_client_gui_v2.ViewModels
{
    /// <summary>
    /// Basis which other viewmodels inherit from -> Easier to implement INotifyPropertyChanged 
    /// </summary>

    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract void UpdateEntireUI();
    }
}
