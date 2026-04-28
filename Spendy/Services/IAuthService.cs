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
		bool persistForNextLaunch = true,
		CancellationToken cancellationToken = default);

	/// <returns>Error message if failed, null if success.</returns>
	/// <param name="persistForNextLaunch">When false, sign-in is not restored after the app restarts.</param>
	Task<string?> LoginAsync(
		string email,
		string password,
		bool persistForNextLaunch = true,
		CancellationToken cancellationToken = default);

	void Logout();

	/// <returns>Error message if failed, null if success.</returns>
	Task<string?> ChangePasswordAsync(
		string currentPassword,
		string newPassword,
		string confirmNewPassword,
		CancellationToken cancellationToken = default);

	/// <summary>Loads saved user id from preferences and validates it still exists.</summary>
	Task<bool> TryRestoreSessionAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Requests a password reset for an email. Always returns a generic success message if the request is accepted,
	/// to avoid user enumeration.
	/// </summary>
	/// <returns>Error message if request was rejected (e.g., rate limited), otherwise null.</returns>
	Task<string?> RequestPasswordResetAsync(string email, CancellationToken cancellationToken = default);

	/// <summary>Completes a password reset using the emailed token.</summary>
	/// <returns>Error message if failed, null if success.</returns>
	Task<string?> ConfirmPasswordResetAsync(
		string email,
		string token,
		string newPassword,
		string confirmNewPassword,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Offline-only password reset: sets a new password for an account identified by email in local SQLite.
	/// </summary>
	/// <returns>Error message if failed, null if success.</returns>
	Task<string?> ResetPasswordByEmailAsync(
		string email,
		string newPassword,
		string confirmNewPassword,
		CancellationToken cancellationToken = default);
}
