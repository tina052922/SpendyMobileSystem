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

	public decimal Amount { get; init; }
	public required string AmountText { get; init; }
	public bool HasAmount => Amount > 0;

	public Color BorderColor { get; init; } = Colors.Transparent;
	public double BorderThickness { get; init; }

	public Color DotColor { get; init; } = Color.FromArgb("#01143D");
}

public sealed class MonthBar
{
	public required string DayLabel { get; init; }
	public double BarHeight { get; init; }
	public Color BarColor { get; init; } = Color.FromArgb("#01143D");
	public bool IsTop { get; init; }
}

public partial class DashboardViewModel : ObservableObject
{
	readonly ISpendyDataService _data;
	readonly IProfilePhotoService _profilePhoto;
	readonly ICurrencyService _currency;
	int _monthlyLoadToken;
	readonly SemaphoreSlim _loadGate = new(1, 1);
	CancellationTokenSource? _loadCts;

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
	private string _monthlyKindLabel = "";

	[ObservableProperty]
	private bool _isDayDetailOpen;

	[ObservableProperty]
	private string _selectedDayTitle = "";

	[ObservableProperty]
	private string _selectedDayTotal = "";

	[ObservableProperty]
	private int? _selectedDayNumber;

	public ObservableCollection<DashboardCalendarDayCell> MonthlyDays { get; } = new();
	public ObservableCollection<CategoryStat> SelectedDayBreakdown { get; } = new();

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
			RequestRefresh();

		_data.DataChanged += (_, _) =>
			RequestRefresh();
		RequestRefresh(immediate: true);
	}

	partial void OnIsExpenseModeChanged(bool value)
	{
		RequestRefresh();
		OnPropertyChanged(nameof(ExpenseButtonBackground));
		OnPropertyChanged(nameof(IncomeButtonBackground));
	}

	public Color ExpenseButtonBackground =>
		IsExpenseMode ? Color.FromArgb("#01143D") : Color.FromArgb("#3E4E65");

	public Color IncomeButtonBackground =>
		!IsExpenseMode ? Color.FromArgb("#01143D") : Color.FromArgb("#3E4E65");

	async Task LoadAsync()
	{
		// Legacy entrypoint - kept for safety. Prefer RequestRefresh().
		RequestRefresh(immediate: true);
	}

	void RequestRefresh(bool immediate = false)
	{
		_loadCts?.Cancel();
		_loadCts?.Dispose();
		_loadCts = new CancellationTokenSource();
		var ct = _loadCts.Token;

		_ = Task.Run(async () =>
		{
			try
			{
				if (!immediate)
					await Task.Delay(180, ct).ConfigureAwait(false); // debounce frequent DB writes
				await RefreshCoreAsync(ct).ConfigureAwait(false);
			}
			catch (OperationCanceledException)
			{
			}
			catch
			{
				// Best-effort: UI should stay responsive even if refresh fails.
			}
		}, ct);
	}

	async Task RefreshCoreAsync(CancellationToken ct)
	{
		if (!await _loadGate.WaitAsync(0, ct).ConfigureAwait(false))
			return;
		try
		{
			var greeting = await _data.GetUserDisplayNameAsync(ct).ConfigureAwait(false) ?? string.Empty;
			var bal = await _data.GetBalanceAsync(ct).ConfigureAwait(false);

			var kind = IsExpenseMode ? TransactionKind.Expense : TransactionKind.Income;
			var day = DateTime.Today;
			var dash = await _data.GetDashboardAsync(day, kind, ct).ConfigureAwait(false);

			// Precompute outside UI thread
			var available = $"{_currency.Symbol}{bal.ToString("N2", _currency.Culture)}";
			var items = dash.Items;
			var hasTx = items.Count > 0;

			await MainThread.InvokeOnMainThreadAsync(() =>
			{
				if (ct.IsCancellationRequested) return;
				UserGreeting = greeting;
				AvailableBalance = available;
				DateLabel = dash.DateLabel;
				SummaryLabel = dash.SummaryLabel;
				SummaryAmount = dash.SummaryAmount;
				SummaryColor = dash.SummaryColor;

				Transactions.Clear();
				foreach (var t in items)
					Transactions.Add(t);
				HasTransactions = hasTx;
			});

			if (IsMonthlyViewOpen && !ct.IsCancellationRequested)
				await LoadMonthlyAsync(MonthlySelectedDate).ConfigureAwait(false);
		}
		finally
		{
			_loadGate.Release();
		}
	}

	partial void OnMonthlySelectedDateChanged(DateTime value)
	{
		if (!IsMonthlyViewOpen)
			return;
		_ = LoadMonthlyAsync(value);
	}

	async Task LoadMonthlyAsync(DateTime anyDayInMonth)
	{
		var loadToken = Interlocked.Increment(ref _monthlyLoadToken);
		var first = new DateTime(anyDayInMonth.Year, anyDayInMonth.Month, 1);
		MonthlySelectedDate = first;
		MonthlyMonthLabel = first.ToString("MMMM yyyy", _currency.Culture).ToUpperInvariant();

		var kind = IsExpenseMode ? TransactionKind.Expense : TransactionKind.Income;
		MonthlyKindLabel = IsExpenseMode ? "EXPENSE HISTORY" : "INCOME HISTORY";
		var stats = await _data.GetStatisticsAsync(first.Year, first.Month, kind);

		if (loadToken != _monthlyLoadToken || !IsMonthlyViewOpen)
			return;

		var amountByDay = stats.Points.ToDictionary(p => p.Day, p => p.Amount);

		var daysInMonth = DateTime.DaysInMonth(first.Year, first.Month);

		MonthlyDays.Clear();

		// Calendar grid: Sunday-first. Offset based on the first day-of-week.
		var leading = (int)first.DayOfWeek; // Sunday=0 ... Saturday=6
		var totalCells = leading + daysInMonth;
		var rows = (int)Math.Ceiling(totalCells / 7d);
		var cells = rows * 7;

		var selectedStroke = Color.FromArgb("#43B3EF");
		var dot = IsExpenseMode ? Color.FromArgb("#01143D") : Color.FromArgb("#00D4A5");
		var decimals = kind == TransactionKind.Income ? 2 : 0;

		for (var idx = 0; idx < cells; idx++)
		{
			var d = idx - leading + 1;
			if (d < 1 || d > daysInMonth)
			{
				MonthlyDays.Add(new DashboardCalendarDayCell
				{
					DayNumber = null,
					Amount = 0,
					AmountText = "",
					BorderColor = Colors.Transparent,
					BorderThickness = 0,
					DotColor = dot
				});
				continue;
			}

			var amt = amountByDay.TryGetValue(d, out var a) ? a : 0m;
			// Highlight the selected day when detail is open.
			var borderColor = (IsDayDetailOpen && SelectedDayNumber == d) ? selectedStroke : Colors.Transparent;
			var borderThickness = borderColor == Colors.Transparent ? 0d : 2d;

			MonthlyDays.Add(new DashboardCalendarDayCell
			{
				DayNumber = d,
				Amount = amt,
				AmountText = amt <= 0 ? "" : _currency.Format(amt, decimals: decimals),
				BorderColor = borderColor,
				BorderThickness = borderThickness,
				DotColor = dot,
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
	Task OpenHistoryAsync()
	{
		var kind = IsExpenseMode ? TransactionKind.Expense : TransactionKind.Income;
		return AppNavigation.PushAsync(new TransactionHistoryPage(kind));
	}

	[RelayCommand]
	Task AddTransactionAsync() =>
		AppNavigation.PushAsync(new AddTransactionPage(!IsExpenseMode));

	[RelayCommand]
	async Task OpenMonthlyViewAsync()
	{
		IsMonthlyViewOpen = true;
		IsDayDetailOpen = false;
		SelectedDayNumber = null;
		SelectedDayTitle = string.Empty;
		SelectedDayTotal = string.Empty;
		SelectedDayBreakdown.Clear();
		await LoadMonthlyAsync(DateTime.Today);
	}

	[RelayCommand]
	void CloseMonthlyView() => IsMonthlyViewOpen = false;

	[RelayCommand]
	async Task SelectCalendarDayAsync(int? dayNumber)
	{
		if (!IsMonthlyViewOpen)
			return;
		if (dayNumber is null || dayNumber.Value <= 0)
			return;

		var day = dayNumber.Value;
		var date = new DateTime(MonthlySelectedDate.Year, MonthlySelectedDate.Month, day);
		var kind = IsExpenseMode ? TransactionKind.Expense : TransactionKind.Income;
		var breakdown = await _data.GetDayBreakdownAsync(date, kind);

		SelectedDayNumber = day;
		SelectedDayTitle = date.ToString("MMMM d, yyyy", _currency.Culture);
		var decimals = kind == TransactionKind.Income ? 2 : 0;
		SelectedDayTotal = _currency.Format(breakdown.Total, decimals: decimals);

		SelectedDayBreakdown.Clear();
		foreach (var row in breakdown.ByCategory)
			SelectedDayBreakdown.Add(row);

		IsDayDetailOpen = true;
		// Avoid reloading month stats (DB + full CollectionView refresh) on every tap.
		// We only need to update the selected border highlight.
		RefreshCalendarSelectionHighlight();
	}

	void RefreshCalendarSelectionHighlight()
	{
		// Force the CollectionView to re-evaluate bindings that depend on selection state.
		// This is cheaper than re-querying and rebuilding the entire month.
		OnPropertyChanged(nameof(SelectedDayNumber));
		OnPropertyChanged(nameof(IsDayDetailOpen));
	}

	[RelayCommand]
	void BackToCalendar()
	{
		IsDayDetailOpen = false;
		SelectedDayNumber = null;
		RefreshCalendarSelectionHighlight();
	}

	[RelayCommand]
	async Task PrevMonthAsync() =>
		await LoadMonthlyAsync(new DateTime(MonthlySelectedDate.Year, MonthlySelectedDate.Month, 1).AddMonths(-1));

	[RelayCommand]
	async Task NextMonthAsync() =>
		await LoadMonthlyAsync(new DateTime(MonthlySelectedDate.Year, MonthlySelectedDate.Month, 1).AddMonths(1));

}
