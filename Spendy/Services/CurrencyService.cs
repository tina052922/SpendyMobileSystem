using System.Globalization;
using Microsoft.Maui.Storage;

namespace Spendy.Services;

public enum AppCurrency
{
	PHP,
	USD
}

public interface ICurrencyService
{
	AppCurrency Current { get; }
	string Symbol { get; }
	CultureInfo Culture { get; }
	event EventHandler? Changed;
	void Set(AppCurrency currency);
	string Format(decimal amount, int decimals);
}

public sealed class CurrencyService : ICurrencyService
{
	const string PrefKey = "SpendyCurrency";

	static readonly CultureInfo CulturePh = CultureInfo.GetCultureInfo("en-PH");
	static readonly CultureInfo CultureUs = CultureInfo.GetCultureInfo("en-US");

	AppCurrency _current;

	public event EventHandler? Changed;

	public CurrencyService()
	{
		var raw = Preferences.Get(PrefKey, nameof(AppCurrency.PHP));
		_current = Enum.TryParse<AppCurrency>(raw, out var v) ? v : AppCurrency.PHP;
	}

	public AppCurrency Current => _current;

	public string Symbol => _current == AppCurrency.USD ? "$" : "₱";

	public CultureInfo Culture => _current == AppCurrency.USD ? CultureUs : CulturePh;

	public void Set(AppCurrency currency)
	{
		if (_current == currency)
			return;

		_current = currency;
		Preferences.Set(PrefKey, currency.ToString());
		Changed?.Invoke(this, EventArgs.Empty);
	}

	public string Format(decimal amount, int decimals)
	{
		var fmt = decimals <= 0 ? "N0" : "N2";
		return $"{Symbol}{amount.ToString(fmt, Culture)}";
	}
}

