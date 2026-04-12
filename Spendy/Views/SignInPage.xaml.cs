using Spendy.Services;

namespace Spendy.Views;

public partial class SignInPage : ContentPage
{
	public SignInPage()
	{
		InitializeComponent();
	}

	void OnTogglePassword(object? sender, EventArgs e) =>
		PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

	async void OnForgot(object? sender, TappedEventArgs e) =>
		await DisplayAlert("Spendy", "Forgot password flow would open here.", "OK");

	void OnSignIn(object? sender, EventArgs e) => AppNavigation.GoToMainShell();

	async void OnGoogle(object? sender, EventArgs e) =>
		await DisplayAlert("Spendy", "Google sign-in would continue here.", "OK");

	async void OnSignUp(object? sender, TappedEventArgs e)
	{
		if (AppNavigation.TryGetRootNavigationPage() is { } nav)
			await nav.PushAsync(new SignUpPage());
	}
}
