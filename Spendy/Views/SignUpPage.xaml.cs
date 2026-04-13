using Spendy.Services;

namespace Spendy.Views;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
		LoadPickers();
		PasswordEyeImage.Source = "unhide.png";
	}

	void LoadPickers()
	{
		MonthPicker.ItemsSource = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
		MonthPicker.SelectedIndex = 7;
		for (var d = 1; d <= 31; d++)
			DayPicker.Items.Add(d.ToString());
		DayPicker.SelectedIndex = 22;
		for (var y = 2030; y >= 1980; y--)
			YearPicker.Items.Add(y.ToString());
		YearPicker.SelectedIndex = YearPicker.Items.IndexOf("2025");
		if (YearPicker.SelectedIndex < 0)
			YearPicker.SelectedIndex = 0;
	}

	void OnTogglePasswordEye(object? sender, TappedEventArgs e)
	{
		PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
		PasswordEyeImage.Source = PasswordEntry.IsPassword ? "unhide.png" : "hideicon.png";
	}

	async void OnTerms(object? sender, TappedEventArgs e) =>
		await DisplayAlert("Spendy", "Terms & Conditions placeholder.", "OK");

	void OnSignUp(object? sender, EventArgs e) => AppNavigation.GoToMainShell();

	async void OnGoogle(object? sender, EventArgs e) =>
		await DisplayAlert("Spendy", "Google sign-up would continue here.", "OK");

	async void OnSignIn(object? sender, TappedEventArgs e)
	{
		if (AppNavigation.TryGetRootNavigationPage() is { } nav)
			await nav.PopAsync();
	}
}
