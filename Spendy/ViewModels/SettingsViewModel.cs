using CommunityToolkit.Mvvm.ComponentModel;
using Spendy.Services;

namespace Spendy.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
	readonly ICurrencyService _currency;

	[ObservableProperty]
	private string _selectedCurrency = "PHP";

	public SettingsViewModel(ICurrencyService currency)
	{
		_currency = currency;
		_selectedCurrency = _currency.Current == AppCurrency.USD ? "USD" : "PHP";
		_currency.Changed += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() =>
			{
				SelectedCurrency = _currency.Current == AppCurrency.USD ? "USD" : "PHP";
			});
	}

	partial void OnSelectedCurrencyChanged(string value)
	{
		var v = string.Equals(value, "USD", StringComparison.OrdinalIgnoreCase)
			? AppCurrency.USD
			: AppCurrency.PHP;
		_currency.Set(v);
	}
}

