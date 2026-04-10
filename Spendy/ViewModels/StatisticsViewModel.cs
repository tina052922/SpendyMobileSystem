using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Data;
using Spendy.Models;
using Spendy.Services;
using Spendy.Views;

namespace Spendy.ViewModels;

public sealed class ChartBarDisplay
{
	public int Day { get; init; }
	public double BarHeight { get; init; }
}

public partial class StatisticsViewModel : ObservableObject
{
	readonly ISpendyDataService _data;
	readonly IProfilePhotoService _profilePhoto;
	readonly ICurrencyService _currency;

	[ObservableProperty]
	private bool _isExpenseMode = true;

	[ObservableProperty]
	private string _chartTitle = "";

	[ObservableProperty]
	private string _monthLabel = "";

	[ObservableProperty]
	private string _availableBalance = "₱0.00";

	[ObservableProperty]
	private bool _hasCategoryBreakdown;

	public ObservableCollection<CategoryStat> Categories { get; } = new();

	public ObservableCollection<ChartBarDisplay> Bars { get; } = new();

	public Color BarColor { get; private set; } = Color.FromArgb("#022268");

	public StatisticsViewModel(ISpendyDataService data)
	{
		_data = data;
		_profilePhoto = Ioc.Services.GetRequiredService<IProfilePhotoService>();
		_currency = Ioc.Services.GetRequiredService<ICurrencyService>();
		_profilePhoto.Changed += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => OnPropertyChanged(nameof(ProfilePhoto)));
		_currency.Changed += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => _ = LoadAsync());

		_data.DataChanged += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => _ = LoadAsync());
		_ = LoadAsync();
	}

	public ImageSource ProfilePhoto => _profilePhoto.Photo;
	public string CurrencySymbol => _currency.Symbol;

	public string YAxis200 => $"{CurrencySymbol}200";
	public string YAxis150 => $"{CurrencySymbol}150";
	public string YAxis100 => $"{CurrencySymbol}100";
	public string YAxis50 => $"{CurrencySymbol}50";
	public string YAxis0 => $"{CurrencySymbol}0";

	partial void OnIsExpenseModeChanged(bool value)
	{
		_ = LoadAsync();
		OnPropertyChanged(nameof(ExpenseButtonBackground));
		OnPropertyChanged(nameof(IncomeButtonBackground));
	}

	public Color ExpenseButtonBackground =>
		IsExpenseMode ? Color.FromArgb("#43B3EF") : Color.FromArgb("#3E4E65");

	public Color IncomeButtonBackground =>
		!IsExpenseMode ? Color.FromArgb("#43B3EF") : Color.FromArgb("#3E4E65");

	async Task LoadAsync()
	{
		var bal = await _data.GetBalanceAsync();
		AvailableBalance = $"{_currency.Symbol}{bal.ToString("N2", _currency.Culture)}";

		var kind = IsExpenseMode ? TransactionKind.Expense : TransactionKind.Income;
		var now = DateTime.Now;
		var data = await _data.GetStatisticsAsync(now.Year, now.Month, kind);

		ChartTitle = data.ChartTitle;
		MonthLabel = now.ToString("MMMM", _currency.Culture).ToUpperInvariant();
		OnPropertyChanged(nameof(CurrencySymbol));
		OnPropertyChanged(nameof(YAxis200));
		OnPropertyChanged(nameof(YAxis150));
		OnPropertyChanged(nameof(YAxis100));
		OnPropertyChanged(nameof(YAxis50));
		OnPropertyChanged(nameof(YAxis0));
		BarColor = data.BarColor;
		OnPropertyChanged(nameof(BarColor));

		Categories.Clear();
		foreach (var c in data.Categories)
			Categories.Add(c);

		HasCategoryBreakdown = Categories.Count > 0;

		Bars.Clear();
		var max = data.Points.Count == 0 ? 1 : data.Points.Max(p => p.Amount);
		if (max <= 0)
			max = 1;
		const double chartHeight = 140;
		foreach (var p in data.Points)
		{
			double h = 0;
			if (p.Amount > 0)
			{
				h = (double)(p.Amount / max) * chartHeight;
				h = Math.Max(h, 4);
			}

			Bars.Add(new ChartBarDisplay { Day = p.Day, BarHeight = h });
		}
	}

	[RelayCommand]
	void SelectExpense() => IsExpenseMode = true;

	[RelayCommand]
	void SelectIncome() => IsExpenseMode = false;

	[RelayCommand]
	Task OpenNotificationsAsync() => AppNavigation.PushAsync(new NotificationPage());

	public bool ShowCategoryEmpty => !HasCategoryBreakdown;

	partial void OnHasCategoryBreakdownChanged(bool value) =>
		OnPropertyChanged(nameof(ShowCategoryEmpty));
}
