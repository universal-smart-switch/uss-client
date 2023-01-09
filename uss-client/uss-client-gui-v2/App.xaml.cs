using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using uss_client_gui_v2.Stores;
using uss_client_gui_v2.ViewModels;
using uss_client_gui_v2.ViewModels.Commands;

namespace uss_client_gui_v2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region field
        
        #endregion 

        #region ctor


        protected override void OnStartup(StartupEventArgs e)
        {
            var nVM = new BridgeStatusViewModel();
            NavigationStore.CurrentViewModel = nVM;
            nVM.ConnectCommand.Execute(nVM);



            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel()
            };

            MainWindow.Show();

            base.OnStartup(e);
        }
        #endregion



    }
}
