namespace Spendy.Services;

public interface IAuthService
{
	/// <returns>Error message if failed, null if success.</returns>
	Task<string?> RegisterAsync(
		string firstName,
		string lastName,
		string email,
		DateTime birthday,
		string password,
		string confirmPassword,
		CancellationToken cancellationToken = default);

	/// <returns>Error message if failed, null if success.</returns>
	Task<string?> LoginAsync(string email, string password, CancellationToken cancellationToken = default);

	void Logout();

	/// <returns>Error message if failed, null if success.</returns>
	Task<string?> ChangePasswordAsync(
		string currentPassword,
		string newPassword,
		string confirmNewPassword,
		CancellationToken cancellationToken = default);

	/// <summary>Loads saved user id from preferences and validates it still exists.</summary>
	Task<bool> TryRestoreSessionAsync(CancellationToken cancellationToken = default);
}
