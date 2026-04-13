using Spendy.Data;

namespace Spendy.Data.Entities;

public sealed class TransactionEntity
{
	public int Id { get; set; }
	public decimal Amount { get; set; }
	public TransactionKind Type { get; set; }
	public int CategoryId { get; set; }
	public CategoryEntity? Category { get; set; }
	public DateTime Date { get; set; }
	public string? Notes { get; set; }
	public DateTime CreatedAt { get; set; }

	public int UserId { get; set; }
	public UserEntity? User { get; set; }
}
