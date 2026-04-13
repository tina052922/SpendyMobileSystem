using Spendy.Data;
using Spendy.Data.Entities;
using Spendy.Models;

namespace Spendy.Services;

public interface ISpendyDataService
{
	event EventHandler? DataChanged;

	Task<decimal> GetBalanceAsync(CancellationToken cancellationToken = default);
	Task<bool> HasAnyIncomeAsync(CancellationToken cancellationToken = default);

	Task<UserEntity?> GetCurrentUserAsync(CancellationToken cancellationToken = default);

	Task UpsertUserAsync(UserEntity user, CancellationToken cancellationToken = default);

	Task<DashboardData> GetDashboardAsync(DateTime day, TransactionKind kind, CancellationToken cancellationToken = default);

	Task<StatisticsData> GetStatisticsAsync(int year, int month, TransactionKind kind, CancellationToken cancellationToken = default);

	Task<IReadOnlyList<SavingPlan>> GetSavingPlansAsync(bool endedOnly, CancellationToken cancellationToken = default);

	Task<SavingPlan?> GetSavingPlanAsync(int id, CancellationToken cancellationToken = default);

	Task<IReadOnlyList<SavingHistoryLine>> GetSavingHistoryAsync(int goalId, CancellationToken cancellationToken = default);

	Task<IReadOnlyList<(int Id, string Icon, string Name)>> GetCategoriesForPickerAsync(TransactionKind kind, CancellationToken cancellationToken = default);

	Task AddTransactionAsync(decimal amount, TransactionKind kind, int categoryId, DateTime date, string? notes, CancellationToken cancellationToken = default);

	/// <summary>Records full income, a mandatory savings deposit to the goal, and a matching expense so available balance reflects the allocation (single save).</summary>
	Task AddIncomeWithMandatorySavingsAsync(
		decimal incomeAmount,
		int categoryId,
		DateTime date,
		string? notes,
		int savingGoalId,
		decimal mandatorySavingsAmount,
		CancellationToken cancellationToken = default);

	Task<int> CreateSavingGoalAsync(string name, decimal targetAmount, DateTime targetDate, CancellationToken cancellationToken = default);

	Task UpdateSavingGoalAsync(int id, string name, decimal targetAmount, DateTime targetDate, CancellationToken cancellationToken = default);

	Task SetSavingGoalEndedAsync(int id, bool isEnded, CancellationToken cancellationToken = default);

	Task AddSavingMovementAsync(int goalId, decimal amount, SavingMovement movement, DateTime date, string? notes, CancellationToken cancellationToken = default);
}

public sealed record DashboardData(
	string DateLabel,
	string SummaryLabel,
	string SummaryAmount,
	Color SummaryColor,
	IReadOnlyList<TransactionItem> Items);

public sealed record StatisticsData(
	string ChartTitle,
	IReadOnlyList<ChartPoint> Points,
	IReadOnlyList<CategoryStat> Categories,
	Color BarColor);
