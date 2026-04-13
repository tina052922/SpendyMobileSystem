namespace Spendy.Data.Entities;

public sealed class SavingGoalEntity
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public decimal TargetAmount { get; set; }
	public decimal CurrentAmount { get; set; }
	public DateTime TargetDate { get; set; }
	public bool IsEnded { get; set; }

	public int UserId { get; set; }
	public UserEntity? User { get; set; }

	public ICollection<SavingTransactionEntity> Transactions { get; set; } = new List<SavingTransactionEntity>();
}
