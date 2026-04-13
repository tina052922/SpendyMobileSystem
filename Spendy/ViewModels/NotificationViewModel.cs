using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Spendy.Data;
using Spendy.Models;
using Spendy.Services;

namespace Spendy.ViewModels;

public partial class NotificationViewModel : ObservableObject
{
	readonly ISpendyDataService _data;
	readonly ICurrencyService _currency;

	public ObservableCollection<NotificationItemVm> Items { get; } = new();

	[ObservableProperty]
	private int _badgeCount;

	public bool HasAlerts => BadgeCount > 0;

	partial void OnBadgeCountChanged(int value) =>
		OnPropertyChanged(nameof(HasAlerts));

	public NotificationViewModel(ISpendyDataService data, ICurrencyService currency)
	{
		_data = data;
		_currency = currency;
		_data.DataChanged += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => _ = LoadAsync());
	}

	public async Task LoadAsync()
	{
		Items.Clear();
		var today = DateTime.Today;
		var accent = Color.FromArgb("#0335A3");
		var warn = Color.FromArgb("#00B2FF");

		var plans = await _data.GetSavingPlansAsync(endedOnly: false);
		foreach (var p in plans.Where(p => !p.IsEnded && !p.IsFinished))
		{
			var days = (p.TargetDateValue.Date - today).Days;
			if (days is < 0 or > 14)
				continue;

			var msg = days switch
			{
				0 => $"\"{p.Name}\" ends today.",
				1 => $"Your {p.Name} goal ends tomorrow.",
				_ => $"Your {p.Name} goal ends in {days} days."
			};
			Items.Add(new NotificationItemVm
			{
				Title = "Savings goal reminder",
				Message = msg,
				TimeText = "Goal deadline",
				AccentColor = accent,
				IconGlyph = "🎯"
			});
		}

		var balance = await _data.GetBalanceAsync();
		if (balance < 0)
		{
			Items.Add(new NotificationItemVm
			{
				Title = "Budget warning",
				Message = $"Available balance is negative ({_currency.Symbol}{balance:N2}). Review expenses.",
				TimeText = "Balance",
				AccentColor = warn,
				IconGlyph = "⚠️"
			});
		}
		else if (balance < 500)
		{
			Items.Add(new NotificationItemVm
			{
				Title = "Low balance",
				Message = $"Available balance is {_currency.Symbol}{balance:N2}. Track spending to stay on budget.",
				TimeText = "Balance",
				AccentColor = warn,
				IconGlyph = "📉"
			});
		}

		var expenseMonth = await SumExpenseMonthAsync(today.Year, today.Month);
		var incomeMonth = await SumIncomeMonthAsync(today.Year, today.Month);
		if (incomeMonth > 0 && expenseMonth > incomeMonth)
		{
			Items.Add(new NotificationItemVm
			{
				Title = "Overspending this month",
				Message = "Expenses exceed income for the current month. Consider adjusting your budget.",
				TimeText = "This month",
				AccentColor = accent,
				IconGlyph = "📊"
			});
		}

		BadgeCount = Items.Count;
	}

	async Task<decimal> SumExpenseMonthAsync(int y, int m)
	{
		var stats = await _data.GetStatisticsAsync(y, m, TransactionKind.Expense);
		return stats.Points.Sum(p => p.Amount);
	}

	async Task<decimal> SumIncomeMonthAsync(int y, int m)
	{
		var stats = await _data.GetStatisticsAsync(y, m, TransactionKind.Income);
		return stats.Points.Sum(p => p.Amount);
	}
}
