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

		// Hide Google button when not configured (keeps app usable offline).
		var google = Ioc.Services.GetRequiredService<IGoogleAuthService>();
		GoogleButton.IsVisible = google.IsConfigured;
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

	async void OnGoogle(object? sender, EventArgs e) =>
		await TryGoogleAsync();

	async Task TryGoogleAsync()
	{
		var svc = Ioc.Services.GetRequiredService<IGoogleAuthService>();
		var err = await svc.SignInAsync();
		if (err is not null)
			await DisplayAlert("Spendy", err, "OK");
		// When a real Google auth service is added, it should complete login + session, then navigate.
		// Keep this page functional even without Google configured.
	}

	async void OnSignUp(object? sender, TappedEventArgs e)
	{
		if (AppNavigation.TryGetRootNavigationPage() is { } nav)
			await nav.PushAsync(new SignUpPage());
	}
}
