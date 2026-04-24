using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Data;
using Spendy.Models;
using Spendy.Services;
using Spendy.Views;

namespace Spendy.ViewModels;

public sealed class DashboardCalendarDayCell
{
	public int? DayNumber { get; init; }
	public bool IsInMonth => DayNumber is not null;

	public string DayLabel => DayNumber?.ToString(CultureInfo.InvariantCulture) ?? "";

	public decimal ExpenseAmount { get; init; }
	public decimal IncomeAmount { get; init; }

	public required string ExpenseText { get; init; }
	public required string IncomeText { get; init; }

	public bool HasExpense => ExpenseAmount > 0;
	public bool HasIncome => IncomeAmount > 0;

	public bool IsTopExpense { get; init; }
	public bool IsTopIncome { get; init; }

	public Color BorderColor { get; init; } = Colors.Transparent;
	public double BorderThickness { get; init; }

	public Color ExpenseDotColor { get; init; } = Color.FromArgb("#01143D");
	public Color IncomeDotColor { get; init; } = Color.FromArgb("#00D4A5");
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

	public ObservableCollection<DashboardCalendarDayCell> MonthlyDays { get; } = new();

	public IReadOnlyList<string> WeekdayHeaders => ["SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"];

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
			MainThread.BeginInvokeOnMainThread(() =>
			{
				_ = LoadAsync();
				if (IsMonthlyViewOpen)
					_ = LoadMonthlyAsync(MonthlySelectedDate);
			});
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

		// Calendar grid: Sunday-first. Offset based on the first day-of-week.
		var daysInMonth = DateTime.DaysInMonth(first.Year, first.Month);
		var leading = (int)first.DayOfWeek; // Sunday=0 ... Saturday=6
		var totalCells = leading + daysInMonth;
		var rows = (int)Math.Ceiling(totalCells / 7d);
		var cells = rows * 7;

		var topExpenseColor = Color.FromArgb("#43B3EF");
		var topIncomeColor = Color.FromArgb("#43B3EF");
		var expenseDot = Color.FromArgb("#01143D");
		var incomeDot = Color.FromArgb("#00D4A5");

		for (var idx = 0; idx < cells; idx++)
		{
			var d = idx - leading + 1;
			if (d < 1 || d > daysInMonth)
			{
				MonthlyDays.Add(new DashboardCalendarDayCell
				{
					DayNumber = null,
					ExpenseAmount = 0,
					IncomeAmount = 0,
					ExpenseText = "",
					IncomeText = "",
					IsTopExpense = false,
					IsTopIncome = false,
					BorderColor = Colors.Transparent,
					BorderThickness = 0,
					ExpenseDotColor = expenseDot,
					IncomeDotColor = incomeDot
				});
				continue;
			}

			var exp = expenseByDay.TryGetValue(d, out var e) ? e : 0m;
			var inc = incomeByDay.TryGetValue(d, out var i) ? i : 0m;
			var isTopExpense = d == topExpenseDay && maxExpense > 0;
			var isTopIncome = d == topIncomeDay && maxIncome > 0;

			var borderColor = isTopExpense || isTopIncome ? topExpenseColor : Colors.Transparent;
			var borderThickness = isTopExpense || isTopIncome ? 2d : 0d;

			MonthlyDays.Add(new DashboardCalendarDayCell
			{
				DayNumber = d,
				ExpenseAmount = exp,
				IncomeAmount = inc,
				ExpenseText = exp <= 0 ? "" : $"{_currency.Symbol}{exp.ToString("N0", _currency.Culture)}",
				IncomeText = inc <= 0 ? "" : $"{_currency.Symbol}{inc.ToString("N0", _currency.Culture)}",
				IsTopExpense = isTopExpense,
				IsTopIncome = isTopIncome,
				BorderColor = borderColor,
				BorderThickness = borderThickness,
				ExpenseDotColor = isTopExpense ? topExpenseColor : expenseDot,
				IncomeDotColor = isTopIncome ? topIncomeColor : incomeDot,
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
