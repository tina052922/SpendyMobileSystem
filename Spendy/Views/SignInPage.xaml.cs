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

	void OnTogglePasswordEye(object? sender, TappedEventArgs e)
	{
		PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
		PasswordEyeImage.Source = PasswordEntry.IsPassword ? "unhide.png" : "hideicon.png";
	}

	async void OnForgot(object? sender, TappedEventArgs e) =>
		await DisplayAlert("Spendy", "Forgot password flow would open here.", "OK");

	async void OnGoogle(object? sender, EventArgs e) =>
		await DisplayAlert("Spendy", "Google sign-in would continue here.", "OK");

	async void OnSignUp(object? sender, TappedEventArgs e)
	{
		if (AppNavigation.TryGetRootNavigationPage() is { } nav)
			await nav.PushAsync(new SignUpPage());
	}
}
