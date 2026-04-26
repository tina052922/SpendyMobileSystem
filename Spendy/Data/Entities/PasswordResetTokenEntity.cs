namespace Spendy.Data.Entities;

public sealed class PasswordResetTokenEntity
{
	public long Id { get; set; }
	public int UserId { get; set; }
	public UserEntity? User { get; set; }

	/// <summary>SHA-256 of the raw token.</summary>
	public string TokenHash { get; set; } = string.Empty;

	public DateTime CreatedAtUtc { get; set; }
	public DateTime ExpiresAtUtc { get; set; }
	public DateTime? UsedAtUtc { get; set; }
}

