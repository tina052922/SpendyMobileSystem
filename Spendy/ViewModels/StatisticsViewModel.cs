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
	public string Icon { get; init; } = "";
	public string Caption { get; init; } = "";
	public double BarHeight { get; init; }
	public Color BarColor { get; init; } = Color.FromArgb("#01143D");
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
	private string _topCategoryHint = "";

	[ObservableProperty]
	private string _availableBalance = "₱0.00";

	[ObservableProperty]
	private bool _hasCategoryBreakdown;

	[ObservableProperty]
	private string _yAxisMaxLabel = "";

	[ObservableProperty]
	private string _yAxisHighLabel = "";

	[ObservableProperty]
	private string _yAxisMidLabel = "";

	[ObservableProperty]
	private string _yAxisLowLabel = "";

	[ObservableProperty]
	private string _yAxisZeroLabel = "";

	[ObservableProperty]
	private string _userGreeting = "";

	/// <summary>First day of the month/year used for statistics queries.</summary>
	[ObservableProperty]
	private DateTime _selectedStatisticsMonth = new(DateTime.Today.Year, DateTime.Today.Month, 1);

	/// <summary>Overlay month/year picker (calendar icon).</summary>
	[ObservableProperty]
	private bool _showStatisticsMonthPicker;

	[ObservableProperty]
	private int _selectedPickerMonthIndex;

	[ObservableProperty]
	private int _selectedPickerYear;

	public ObservableCollection<string> MonthPickerItems { get; } = new();

	public ObservableCollection<int> YearPickerItems { get; } = new();

	public ObservableCollection<CategoryStat> Categories { get; } = new();

	public ObservableCollection<ChartBarDisplay> Bars { get; } = new();

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

		for (var m = 1; m <= 12; m++)
			MonthPickerItems.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m));

		var y0 = DateTime.Today.Year;
		for (var y = y0 - 15; y <= y0 + 2; y++)
			YearPickerItems.Add(y);

		_ = LoadAsync();
	}

	public ImageSource ProfilePhoto => _profilePhoto.Photo;
	public string CurrencySymbol => _currency.Symbol;

	public bool HasUserGreeting => !string.IsNullOrWhiteSpace(UserGreeting);

	partial void OnUserGreetingChanged(string value) =>
		OnPropertyChanged(nameof(HasUserGreeting));

	partial void OnIsExpenseModeChanged(bool value)
	{
		_ = LoadAsync();
		OnPropertyChanged(nameof(ExpenseButtonBackground));
		OnPropertyChanged(nameof(IncomeButtonBackground));
	}

	public Color ExpenseButtonBackground =>
		IsExpenseMode ? Color.FromArgb("#01143D") : Color.FromArgb("#3E4E65");

	public Color IncomeButtonBackground =>
		!IsExpenseMode ? Color.FromArgb("#01143D") : Color.FromArgb("#3E4E65");

	string FormatAxis(decimal amount) =>
		$"{_currency.Symbol}{decimal.Round(amount, 0).ToString("N0", _currency.Culture)}";

	async Task LoadAsync()
	{
		try
		{
			UserGreeting = await _data.GetUserDisplayNameAsync() ?? string.Empty;

			var bal = await _data.GetBalanceAsync();
			AvailableBalance = $"{_currency.Symbol}{bal.ToString("N2", _currency.Culture)}";

			var kind = IsExpenseMode ? TransactionKind.Expense : TransactionKind.Income;
			var month = SelectedStatisticsMonth;
			var data = await _data.GetStatisticsAsync(month.Year, month.Month, kind);

			ChartTitle = kind == TransactionKind.Expense
				? "Spending by category"
				: "Income by category";
			MonthLabel = month.ToString("MMMM yyyy", _currency.Culture).ToUpperInvariant();
			OnPropertyChanged(nameof(CurrencySymbol));

			Categories.Clear();
			foreach (var c in data.Categories)
				Categories.Add(c);

			HasCategoryBreakdown = Categories.Count > 0;
			TopCategoryHint = Categories.Count > 0 && Categories[0].Amount > 0
				? $"Highest: {Categories[0].Name} · {Categories[0].FormattedAmount}"
				: "";

			Bars.Clear();
			var catList = data.Categories.Take(8).ToList();
			var maxAmt = catList.Count == 0 ? 1m : catList.Max(c => c.Amount);
			if (maxAmt <= 0)
				maxAmt = 1;
			const double chartHeight = 140;
			var expenseBar = Color.FromArgb("#01143D");
			var incomeBar = Color.FromArgb("#00D4A5");
			var topExpense = Color.FromArgb("#43B3EF");
			var topIncome = Color.FromArgb("#43B3EF");
			foreach (var c in catList)
			{
				var h = 0d;
				if (c.Amount > 0)
				{
					h = (double)(c.Amount / maxAmt) * chartHeight;
					h = Math.Max(h, 4);
				}

				var caption = c.Name.Length > 10 ? c.Name[..7] + "…" : c.Name;
				var isTop = c.IsTopCategory && c.Amount > 0;
				var barColor = kind == TransactionKind.Expense
					? (isTop ? topExpense : expenseBar)
					: (isTop ? topIncome : incomeBar);
				Bars.Add(new ChartBarDisplay
				{
					Icon = c.Icon,
					Caption = caption,
					BarHeight = h,
					BarColor = barColor
				});
			}

			YAxisZeroLabel = FormatAxis(0);
			YAxisLowLabel = FormatAxis(maxAmt * 0.25m);
			YAxisMidLabel = FormatAxis(maxAmt * 0.5m);
			YAxisHighLabel = FormatAxis(maxAmt * 0.75m);
			YAxisMaxLabel = FormatAxis(maxAmt);
		}
		catch (Exception)
		{
			// Keep UI stable; next refresh may succeed.
		}
	}

	[RelayCommand]
	void PickStatisticsMonth()
	{
		var m = SelectedStatisticsMonth;
		SelectedPickerMonthIndex = Math.Clamp(m.Month - 1, 0, 11);
		var y = m.Year;
		if (YearPickerItems.Count > 0)
		{
			if (y < YearPickerItems[0])
				y = YearPickerItems[0];
			else if (y > YearPickerItems[^1])
				y = YearPickerItems[^1];
		}

		SelectedPickerYear = y;
		ShowStatisticsMonthPicker = true;
	}

	[RelayCommand]
	async Task ApplyStatisticsMonthPickerAsync()
	{
		ShowStatisticsMonthPicker = false;
		var month = Math.Clamp(SelectedPickerMonthIndex, 0, 11) + 1;
		var year = SelectedPickerYear;
		SelectedStatisticsMonth = new DateTime(year, month, 1);
		await LoadAsync();
	}

	[RelayCommand]
	void CancelStatisticsMonthPicker() =>
		ShowStatisticsMonthPicker = false;

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
