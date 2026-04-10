namespace Spendy.Data.Entities;

public sealed class UserEntity
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string Birthday { get; set; } = string.Empty;
	public string Gender { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string? Handle { get; set; }
}
