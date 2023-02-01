using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using uss_client_gui_v2.Commands;
using uss_client_gui_v2.ViewModels;
using uss_client_sandbox.Models;

namespace uss_client_gui_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        public NavigateToCommand NavigateToBridge = new NavigateToCommand(new BridgeStatusViewModel());
        public NavigateToCommand NavigateToSwitches = new NavigateToCommand(new SwitchDashboardViewModel());
        public NavigateToCommand NavigateToModes = new NavigateToCommand(new ModesDashboardViewModel());

        private void btnBridgeV_Click(object sender, RoutedEventArgs e)
        {
            NavigateToBridge.Execute(this);
        }

        private void btnSwitchV_Click(object sender, RoutedEventArgs e)
        {
            NavigateToSwitches.Execute(this);
        }

        private void btnModesV_Click(object sender, RoutedEventArgs e)
        {
            NavigateToModes.Execute(this);
        }

        public bool NavigateButtonsEnabled { get => !NetworkManager.ConnectionError; }
    }
}
