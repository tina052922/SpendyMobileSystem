using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class ForgotPasswordRequestPage : ContentPage
{
	public ForgotPasswordRequestPage()
	{
		InitializeComponent();
		BindingContext = Ioc.Services.GetRequiredService<ForgotPasswordRequestViewModel>();
	}

	async void OnBack(object? sender, EventArgs e)
	{
		if (AppNavigation.TryGetRootNavigationPage() is { } nav)
			await nav.PopAsync();
	}
}

