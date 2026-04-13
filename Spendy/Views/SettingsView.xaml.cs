using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class SettingsView : ContentView
{
	bool _showCurrent;
	bool _showNew;
	bool _showConfirm;

	public SettingsView()
	{
		InitializeComponent();
		BindingContext = Ioc.Services.GetRequiredService<SettingsViewModel>();
	}

	async void OnProfileTapped(object? sender, TappedEventArgs e) =>
		await AppNavigation.PushAsync(new ProfilePage());

	async void OnCurrencyTapped(object? sender, TappedEventArgs e)
	{
		var host = Shell.Current?.CurrentPage ?? Shell.Current;
		if (host is null || BindingContext is not SettingsViewModel vm)
			return;

		var pick = await host.DisplayActionSheet("Currency", "Cancel", null, "PHP", "USD");
		if (pick is null || pick == "Cancel")
			return;

		vm.SelectedCurrency = pick;
	}

	void OnTogglePassword(object? sender, TappedEventArgs e)
	{
		PasswordFields.IsVisible = !PasswordFields.IsVisible;
		Chevron.Text = PasswordFields.IsVisible ? "⌄" : "›";
	}

	void OnToggleCurrent(object? sender, EventArgs e)
	{
		_showCurrent = !_showCurrent;
		CurrentPassword.IsPassword = !_showCurrent;
		if (sender is ImageButton b)
			b.Source = _showCurrent ? "hideicon.png" : "unhide.png";
	}

	void OnToggleNew(object? sender, EventArgs e)
	{
		_showNew = !_showNew;
		NewPassword.IsPassword = !_showNew;
		if (sender is ImageButton b)
			b.Source = _showNew ? "hideicon.png" : "unhide.png";
	}

	void OnToggleConfirm(object? sender, EventArgs e)
	{
		_showConfirm = !_showConfirm;
		ConfirmPassword.IsPassword = !_showConfirm;
		if (sender is ImageButton b)
			b.Source = _showConfirm ? "hideicon.png" : "unhide.png";
	}

	void OnLogout(object? sender, EventArgs e)
	{
		Ioc.Services.GetRequiredService<IAuthService>().Logout();
		AppNavigation.GoToSignInStack();
	}

	void OnLogoutTapped(object? sender, TappedEventArgs e)
	{
		Ioc.Services.GetRequiredService<IAuthService>().Logout();
		AppNavigation.GoToSignInStack();
	}
}
