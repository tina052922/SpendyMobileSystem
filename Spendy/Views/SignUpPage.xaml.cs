using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
		BindingContext = Ioc.Services.GetRequiredService<SignUpViewModel>();
		PasswordEyeImage.Source = "unhide.png";
	}

	void OnTogglePasswordEye(object? sender, TappedEventArgs e)
	{
		PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
		PasswordEyeImage.Source = PasswordEntry.IsPassword ? "unhide.png" : "hideicon.png";
	}

	async void OnTerms(object? sender, TappedEventArgs e) =>
		await DisplayAlert("Spendy", "Terms & Conditions placeholder.", "OK");

	async void OnGoogle(object? sender, EventArgs e) =>
		await DisplayAlert("Spendy", "Google sign-up would continue here.", "OK");

	async void OnSignIn(object? sender, TappedEventArgs e)
	{
		if (AppNavigation.TryGetRootNavigationPage() is { } nav)
			await nav.PopAsync();
	}
}
