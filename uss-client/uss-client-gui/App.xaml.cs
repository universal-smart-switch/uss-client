using uss_client_sandbox.Models;

namespace uss_client_gui
{
    public partial class App : Application
    {
        public App()
        {
            var token = new CancellationTokenSource();
            NetworkManager.Connect(token);
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}