using Spendy.Data;

namespace Spendy.Data.Entities;

public sealed class SavingTransactionEntity
{
	public int Id { get; set; }
	public int SavingGoalId { get; set; }
	public SavingGoalEntity? SavingGoal { get; set; }
	public decimal Amount { get; set; }
	public SavingMovement Type { get; set; }
	public DateTime Date { get; set; }
	public string? Notes { get; set; }
}
