using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uss_client_gui_v2.Stores;

namespace uss_client_gui_v2.ViewModels
{
    public static class CreateViewModels
    {

        public static void CreateBridgeStatusViewModel()
        {
            NavigationStore.CurrentViewModel = new BridgeStatusViewModel();
        }

        public static void CreateModesDashboardViewModel()
        {
            NavigationStore.CurrentViewModel = new ModesDashboardViewModel();
        }

        public static void CreateSwitchDashboardViewModel()
        {
            NavigationStore.CurrentViewModel = new SwitchDashboardViewModel();
        }
    }
}
