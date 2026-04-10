using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class DashboardView : ContentView
{
	public DashboardView()
	{
		InitializeComponent();
		BindingContext = Ioc.Services.GetRequiredService<DashboardViewModel>();
	}
}
