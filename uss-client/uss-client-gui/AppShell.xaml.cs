using uss_client_gui.ViewModels;
using uss_client_gui.Views;

namespace uss_client_gui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(SwitchDashboardView), typeof(SwitchDashboardView));
        }
    }
}