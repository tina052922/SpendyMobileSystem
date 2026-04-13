using CommunityToolkit.Mvvm.ComponentModel;

namespace Spendy.Models;

/// <summary>Expense/income category chip for the add-transaction picker.</summary>
public partial class CategoryPickItem : ObservableObject
{
	public required int Id { get; init; }
	public required string Icon { get; init; }
	public required string Name { get; init; }

	[ObservableProperty]
	private bool _isSelected;
}

public sealed class TransactionItem
{
	public required string Category { get; init; }
	public required string Icon { get; init; }
	public decimal Amount { get; init; }
	public required string Time { get; init; }
	public string CurrencySymbol { get; init; } = "₱";

	public string FormattedAmount =>
		Amount < 0 ? $"-{CurrencySymbol}{Math.Abs(Amount):N0}" : $"+{CurrencySymbol}{Amount:N0}";

	public Color AmountColor =>
		Amount < 0 ? Color.FromArgb("#FF0000") : Color.FromArgb("#00D4A5");
}

public sealed class CategoryStat
{
	public required string Name { get; init; }
	public required string Icon { get; init; }
	public decimal Amount { get; init; }
	public Color AmountColor { get; init; } = Color.FromArgb("#01143D");
	public string CurrencySymbol { get; init; } = "₱";
	public bool IsTopCategory { get; init; }

	public string FormattedAmount => $"{CurrencySymbol}{Amount:N0}";

	public Color HighlightStroke =>
		IsTopCategory ? Color.FromArgb("#43B3EF") : Colors.Transparent;

	public double HighlightStrokeThickness => IsTopCategory ? 2 : 0;
}

public sealed class ChartPoint
{
	public int Day { get; init; }
	public decimal Amount { get; init; }
}

public sealed class SavingPlan
{
	public int Id { get; init; }
	public required string Name { get; init; }
	public decimal Current { get; init; }
	public decimal Target { get; init; }
	public required string TargetDate { get; init; }
	public string CurrencySymbol { get; init; } = "₱";
	/// <summary>Target date for edit forms / date pickers.</summary>
	public DateTime TargetDateValue { get; init; }
	public bool IsEnded { get; init; }
	public bool IsFinished { get; init; }

	public double Progress => Target <= 0 ? 0 : (double)(Current / Target);

	public double ProgressPercent => Progress * 100;

	public string AmountLine => $"{CurrencySymbol}{Current:N0} / {CurrencySymbol}{Target:N0}";
}

public sealed class HistoryLine
{
	public required string Date { get; init; }
	public decimal Amount { get; init; }
}

/// <summary>Row in saving goal transaction history (Save / Withdraw).</summary>
public sealed class SavingHistoryLine
{
	public required string DateText { get; init; }
	public required string AmountText { get; init; }
	public Color AmountColor { get; init; } = Color.FromArgb("#01143D");
}
