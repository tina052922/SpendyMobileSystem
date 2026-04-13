using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Spendy.Services;
using Spendy.Views;

namespace Spendy.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
	readonly ICurrencyService _currency;
	readonly IProfilePhotoService _profilePhoto;
	readonly ISpendyDataService _data;
	readonly IAuthService _auth;

	public ImageSource ProfilePhoto => _profilePhoto.Photo;

	[ObservableProperty]
	private string _selectedCurrency = "PHP";

	[ObservableProperty]
	private string _userGreeting = "";

	public bool HasUserGreeting => !string.IsNullOrWhiteSpace(UserGreeting);

	partial void OnUserGreetingChanged(string value) =>
		OnPropertyChanged(nameof(HasUserGreeting));

	[ObservableProperty]
	private string _currentPassword = string.Empty;

	[ObservableProperty]
	private string _newPassword = string.Empty;

	[ObservableProperty]
	private string _confirmNewPassword = string.Empty;

	[ObservableProperty]
	private string _newPasswordStrengthLabel = string.Empty;

	[ObservableProperty]
	private Color _newPasswordStrengthColor = Color.FromArgb("#888888");

	public SettingsViewModel(ICurrencyService currency, IProfilePhotoService profilePhoto, ISpendyDataService data, IAuthService auth)
	{
		_currency = currency;
		_profilePhoto = profilePhoto;
		_data = data;
		_auth = auth;
		_profilePhoto.Changed += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => OnPropertyChanged(nameof(ProfilePhoto)));
		_data.DataChanged += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => _ = LoadUserGreetingAsync());
		_selectedCurrency = _currency.Current == AppCurrency.USD ? "USD" : "PHP";
		_currency.Changed += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() =>
			{
				SelectedCurrency = _currency.Current == AppCurrency.USD ? "USD" : "PHP";
			});
		_ = LoadUserGreetingAsync();
	}

	partial void OnNewPasswordChanged(string value)
	{
		var score = PasswordPolicy.StrengthScore(NewPassword);
		NewPasswordStrengthLabel = score == 0 ? "" : PasswordPolicy.StrengthLabel(score);
		NewPasswordStrengthColor = score switch
		{
			0 => Color.FromArgb("#888888"),
			1 => Color.FromArgb("#D32F2F"),
			2 => Color.FromArgb("#F57C00"),
			3 => Color.FromArgb("#1976D2"),
			_ => Color.FromArgb("#2E7D32")
		};
	}

	[RelayCommand]
	async Task UpdatePasswordAsync()
	{
		if (Shell.Current is null)
			return;

		var err = await _auth.ChangePasswordAsync(CurrentPassword, NewPassword, ConfirmNewPassword);
		if (err is null)
		{
			CurrentPassword = string.Empty;
			NewPassword = string.Empty;
			ConfirmNewPassword = string.Empty;
			NewPasswordStrengthLabel = string.Empty;
			await Shell.Current.DisplayAlert("Spendy", "Password updated successfully.", "OK");
		}
		else
			await Shell.Current.DisplayAlert("Spendy", err, "OK");
	}

	async Task LoadUserGreetingAsync() =>
		UserGreeting = await _data.GetUserDisplayNameAsync() ?? string.Empty;

	[RelayCommand]
	Task OpenNotificationsAsync() => AppNavigation.PushAsync(new NotificationPage());

	partial void OnSelectedCurrencyChanged(string value)
	{
		var v = string.Equals(value, "USD", StringComparison.OrdinalIgnoreCase)
			? AppCurrency.USD
			: AppCurrency.PHP;
		_currency.Set(v);
	}
}

