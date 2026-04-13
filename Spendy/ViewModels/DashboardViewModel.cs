using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Data;
using Spendy.Models;
using Spendy.Services;
using Spendy.Views;

namespace Spendy.ViewModels;

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
		IsExpenseMode ? Color.FromArgb("#01143D") : Color.FromArgb("#9AA5B1");

	public Color IncomeButtonBackground =>
		!IsExpenseMode ? Color.FromArgb("#01143D") : Color.FromArgb("#9AA5B1");

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
}
