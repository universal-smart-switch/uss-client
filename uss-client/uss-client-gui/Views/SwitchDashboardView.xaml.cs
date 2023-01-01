using uss_client_gui.ViewModels;
using uss_client_sandbox.Models;

namespace uss_client_gui.Views;

public partial class SwitchDashboardView : ContentPage
{
	SwitchDashboardViewModel vm;
	public SwitchDashboardView(SwitchDashboardViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		this.vm = vm;
		switchListView.SelectedItem = LocalBridge.SwitchList[0];
	}

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
		if (switchListView.IsLoaded)
		{
			vm.TrySetSwitchState();
		}
    }
}