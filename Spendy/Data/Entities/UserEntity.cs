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

	/// <summary>Local file path to the profile image (AppDataDirectory or other persisted location).</summary>
	public string? ProfilePhotoPath { get; set; }

	/// <summary>BCrypt or compatible password hash. Null/empty = account must set password on first login.</summary>
	public string? PasswordHash { get; set; }
}
