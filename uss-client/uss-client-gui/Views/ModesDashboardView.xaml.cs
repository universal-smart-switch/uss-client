using uss_client_gui.ViewModels;

namespace uss_client_gui.Views;

public partial class ModesDashboardView : ContentPage
{
	public ModesDashboardView(ModesDashboardViewModel vm)
	{
		InitializeComponent();
		BindingContext= vm;
	}
}