


using uss_client_gui.ViewModels;

namespace uss_client_gui
{
    public partial class MainPage : ContentPage
    {
        public MainPage(BridgeStatusViewModel viewModel)
        {
            InitializeComponent();
            BindingContext= viewModel;
        }
    }
}