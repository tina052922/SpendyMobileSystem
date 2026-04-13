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

	public ImageSource ProfilePhoto => _profilePhoto.Photo;

	[ObservableProperty]
	private string _selectedCurrency = "PHP";

	public SettingsViewModel(ICurrencyService currency, IProfilePhotoService profilePhoto)
	{
		_currency = currency;
		_profilePhoto = profilePhoto;
		_profilePhoto.Changed += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => OnPropertyChanged(nameof(ProfilePhoto)));
		_selectedCurrency = _currency.Current == AppCurrency.USD ? "USD" : "PHP";
		_currency.Changed += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() =>
			{
				SelectedCurrency = _currency.Current == AppCurrency.USD ? "USD" : "PHP";
			});
	}

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

