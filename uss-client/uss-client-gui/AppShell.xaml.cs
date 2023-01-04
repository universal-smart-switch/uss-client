using uss_client_gui.ViewModels;
using uss_client_gui.Views;
using uss_client_sandbox.Models;
using ussclientsandbox.Models;

namespace uss_client_gui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            CalcPossibleCharacteristics();
            InitializeComponent();



            Routing.RegisterRoute(nameof(SwitchDashboardView), typeof(SwitchDashboardView));
        }

        public void CalcPossibleCharacteristics()
        {
            
            LocalBridge.PossibleCharacteristics.Clear();
            foreach (var item in Enum.GetValues(typeof(CharacteristicType)))
            {
                LocalBridge.PossibleCharacteristics.Add(item.ToString());
            }

        }
    }
}