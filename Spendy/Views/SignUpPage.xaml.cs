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

		var google = Ioc.Services.GetRequiredService<IGoogleAuthService>();
		GoogleButton.IsVisible = google.IsConfigured;
	}

	void OnTogglePasswordEye(object? sender, TappedEventArgs e)
	{
		PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
		PasswordEyeImage.Source = PasswordEntry.IsPassword ? "unhide.png" : "hideicon.png";
	}

	async void OnTerms(object? sender, TappedEventArgs e) =>
		await DisplayAlert("Spendy", "Terms & Conditions placeholder.", "OK");

	async void OnGoogle(object? sender, EventArgs e) =>
		await TryGoogleAsync();

	async Task TryGoogleAsync()
	{
		var svc = Ioc.Services.GetRequiredService<IGoogleAuthService>();
		var err = await svc.SignInAsync();
		if (err is not null)
			await DisplayAlert("Spendy", err, "OK");
	}

	async void OnSignIn(object? sender, TappedEventArgs e)
	{
		if (AppNavigation.TryGetRootNavigationPage() is { } nav)
			await nav.PopAsync();
	}
}
