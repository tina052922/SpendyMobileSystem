using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class ForgotPasswordPage : ContentPage
{
	public ForgotPasswordPage()
	{
		InitializeComponent();
		BindingContext = Ioc.Services.GetRequiredService<ForgotPasswordViewModel>();
	}

	async void OnBack(object? sender, EventArgs e)
	{
		if (AppNavigation.TryGetRootNavigationPage() is { } nav)
			await nav.PopAsync();
	}
}

