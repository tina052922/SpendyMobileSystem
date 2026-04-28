using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class SignInPage : ContentPage
{
	public SignInPage()
	{
		InitializeComponent();
		BindingContext = Ioc.Services.GetRequiredService<SignInViewModel>();
		PasswordEyeImage.Source = "unhide.png";
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		// Intentionally do not auto-redirect here.
		// The app should only enter the main shell after an explicit successful login action.
	}

	void OnTogglePasswordEye(object? sender, TappedEventArgs e)
	{
		PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
		PasswordEyeImage.Source = PasswordEntry.IsPassword ? "unhide.png" : "hideicon.png";
	}

	async void OnForgot(object? sender, TappedEventArgs e)
	{
		if (AppNavigation.TryGetRootNavigationPage() is { } nav)
			await nav.PushAsync(new ForgotPasswordRequestPage());
	}

	async void OnSignUp(object? sender, TappedEventArgs e)
	{
		if (AppNavigation.TryGetRootNavigationPage() is { } nav)
			await nav.PushAsync(new SignUpPage());
	}
}
