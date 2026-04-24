using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Data;
using Spendy.Models;
using Spendy.Services;
using Spendy.Views;

namespace Spendy.ViewModels;

public sealed class MonthlyDayLine
{
	public int Day { get; init; }
	public required string DayLabel { get; init; }
	public required string ExpenseText { get; init; }
	public required string IncomeText { get; init; }
	public double ExpenseProgress { get; init; }
	public double IncomeProgress { get; init; }
	public bool IsTopExpense { get; init; }
	public bool IsTopIncome { get; init; }
	public Color ExpenseBarColor { get; init; } = Color.FromArgb("#01143D");
	public Color IncomeBarColor { get; init; } = Color.FromArgb("#00D4A5");
}

public partial class DashboardViewModel : ObservableObject
{
	readonly ISpendyDataService _data;
	readonly IProfilePhotoService _profilePhoto;
	readonly ICurrencyService _currency;

	[ObservableProperty]
	private bool _isExpenseMode = true;

	[ObservableProperty]
	private string _summaryLabel = "Total Expenditure";

	[ObservableProperty]
	private string _summaryAmount = "₱0";

	[ObservableProperty]
	private Color _summaryColor = Color.FromArgb("#FF0000");

	[ObservableProperty]
	private string _dateLabel = "";

	[ObservableProperty]
	private string _availableBalance = "₱0.00";

	[ObservableProperty]
	private bool _hasTransactions;

	[ObservableProperty]
	private string _userGreeting = "";

	public ObservableCollection<TransactionItem> Transactions { get; } = new();

	[ObservableProperty]
	private bool _isMonthlyViewOpen;

	[ObservableProperty]
	private DateTime _monthlySelectedDate = DateTime.Today;

	[ObservableProperty]
	private string _monthlyMonthLabel = "";

	[ObservableProperty]
	private string _topSpendingDayHint = "";

	[ObservableProperty]
	private string _topIncomeDayHint = "";

	public ObservableCollection<MonthlyDayLine> MonthlyDays { get; } = new();

	public ImageSource ProfilePhoto => _profilePhoto.Photo;

	public bool HasUserGreeting => !string.IsNullOrWhiteSpace(UserGreeting);

	partial void OnUserGreetingChanged(string value) =>
		OnPropertyChanged(nameof(HasUserGreeting));

	public DashboardViewModel(ISpendyDataService data)
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

	async Task LoadAsync()
	{
		UserGreeting = await _data.GetUserDisplayNameAsync() ?? string.Empty;

		var bal = await _data.GetBalanceAsync();
		AvailableBalance = $"{_currency.Symbol}{bal.ToString("N2", _currency.Culture)}";

		var kind = IsExpenseMode ? TransactionKind.Expense : TransactionKind.Income;
		var day = DateTime.Today;
		var dash = await _data.GetDashboardAsync(day, kind);

		DateLabel = dash.DateLabel;
		SummaryLabel = dash.SummaryLabel;
		SummaryAmount = dash.SummaryAmount;
		SummaryColor = dash.SummaryColor;

		Transactions.Clear();
		foreach (var t in dash.Items)
			Transactions.Add(t);

		HasTransactions = Transactions.Count > 0;
	}

	partial void OnMonthlySelectedDateChanged(DateTime value)
	{
		if (!IsMonthlyViewOpen)
			return;
		_ = LoadMonthlyAsync(value);
	}

	async Task LoadMonthlyAsync(DateTime anyDayInMonth)
	{
		var first = new DateTime(anyDayInMonth.Year, anyDayInMonth.Month, 1);
		MonthlySelectedDate = first;
		MonthlyMonthLabel = first.ToString("MMMM yyyy", _currency.Culture).ToUpperInvariant();

		var expense = await _data.GetStatisticsAsync(first.Year, first.Month, TransactionKind.Expense);
		var income = await _data.GetStatisticsAsync(first.Year, first.Month, TransactionKind.Income);

		var expenseByDay = expense.Points.ToDictionary(p => p.Day, p => p.Amount);
		var incomeByDay = income.Points.ToDictionary(p => p.Day, p => p.Amount);

		var maxExpense = expense.Points.Count == 0 ? 0m : expense.Points.Max(p => p.Amount);
		var maxIncome = income.Points.Count == 0 ? 0m : income.Points.Max(p => p.Amount);
		if (maxExpense < 0) maxExpense = 0;
		if (maxIncome < 0) maxIncome = 0;

		var topExpenseDay = expense.Points.Where(p => p.Amount == maxExpense && p.Amount > 0).Select(p => p.Day).FirstOrDefault();
		var topIncomeDay = income.Points.Where(p => p.Amount == maxIncome && p.Amount > 0).Select(p => p.Day).FirstOrDefault();

		TopSpendingDayHint = maxExpense > 0 && topExpenseDay > 0
			? $"Pinaka gasto: Day {topExpenseDay} · {_currency.Symbol}{maxExpense.ToString("N0", _currency.Culture)}"
			: "Pinaka gasto: —";

		TopIncomeDayHint = maxIncome > 0 && topIncomeDay > 0
			? $"Pinaka gaining: Day {topIncomeDay} · {_currency.Symbol}{maxIncome.ToString("N0", _currency.Culture)}"
			: "Pinaka gaining: —";

		MonthlyDays.Clear();
		var daysInMonth = DateTime.DaysInMonth(first.Year, first.Month);
		for (var d = 1; d <= daysInMonth; d++)
		{
			var exp = expenseByDay.TryGetValue(d, out var e) ? e : 0m;
			var inc = incomeByDay.TryGetValue(d, out var i) ? i : 0m;
			var expProgress = maxExpense <= 0 ? 0 : (double)(exp / maxExpense);
			var incProgress = maxIncome <= 0 ? 0 : (double)(inc / maxIncome);
			expProgress = double.IsFinite(expProgress) ? Math.Clamp(expProgress, 0, 1) : 0;
			incProgress = double.IsFinite(incProgress) ? Math.Clamp(incProgress, 0, 1) : 0;

			MonthlyDays.Add(new MonthlyDayLine
			{
				Day = d,
				DayLabel = d.ToString(CultureInfo.InvariantCulture),
				ExpenseText = $"{_currency.Symbol}{exp.ToString("N0", _currency.Culture)}",
				IncomeText = $"{_currency.Symbol}{inc.ToString("N0", _currency.Culture)}",
				ExpenseProgress = expProgress,
				IncomeProgress = incProgress,
				IsTopExpense = d == topExpenseDay && maxExpense > 0,
				IsTopIncome = d == topIncomeDay && maxIncome > 0,
				ExpenseBarColor = d == topExpenseDay && maxExpense > 0 ? Color.FromArgb("#43B3EF") : Color.FromArgb("#01143D"),
				IncomeBarColor = d == topIncomeDay && maxIncome > 0 ? Color.FromArgb("#43B3EF") : Color.FromArgb("#00D4A5"),
			});
		}
	}

	public bool ShowEmptyState => !HasTransactions;

	partial void OnHasTransactionsChanged(bool value) =>
		OnPropertyChanged(nameof(ShowEmptyState));

	[RelayCommand]
	void SelectExpense() => IsExpenseMode = true;

	[RelayCommand]
	void SelectIncome() => IsExpenseMode = false;

	[RelayCommand]
	Task OpenNotificationsAsync() => AppNavigation.PushAsync(new NotificationPage());

	[RelayCommand]
	Task AddTransactionAsync() =>
		AppNavigation.PushAsync(new AddTransactionPage(!IsExpenseMode));

	[RelayCommand]
	async Task OpenMonthlyViewAsync()
	{
		IsMonthlyViewOpen = true;
		await LoadMonthlyAsync(DateTime.Today);
	}

	[RelayCommand]
	void CloseMonthlyView() => IsMonthlyViewOpen = false;

	[RelayCommand]
	async Task PrevMonthAsync() =>
		await LoadMonthlyAsync(new DateTime(MonthlySelectedDate.Year, MonthlySelectedDate.Month, 1).AddMonths(-1));

	[RelayCommand]
	async Task NextMonthAsync() =>
		await LoadMonthlyAsync(new DateTime(MonthlySelectedDate.Year, MonthlySelectedDate.Month, 1).AddMonths(1));
}
