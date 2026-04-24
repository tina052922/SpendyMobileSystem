using Microsoft.EntityFrameworkCore;
using Spendy.Data;
using Spendy.Data.Entities;
using System.Globalization;

namespace Spendy.Services;

public sealed class AuthService(
	IDbContextFactory<SpendyDbContext> dbFactory,
	IUserSession session,
	IPasswordHasher hasher,
	IProfilePhotoService profilePhoto,
	ISpendyDataService data) : IAuthService
{
	public async Task<string?> RegisterAsync(
		string firstName,
		string lastName,
		string email,
		DateTime birthday,
		string password,
		string confirmPassword,
		CancellationToken cancellationToken = default)
	{
		var fn = (firstName ?? string.Empty).Trim();
		var ln = (lastName ?? string.Empty).Trim();
		if (string.IsNullOrWhiteSpace(fn) || string.IsNullOrWhiteSpace(ln))
			return "First and last name are required.";

		var normalized = NormalizeEmail(email);
		if (normalized is null)
			return "Enter a valid email address.";

		if (!string.Equals(password, confirmPassword, StringComparison.Ordinal))
			return "Password and confirm password must match.";

		if (!PasswordPolicy.TryValidate(password, out var policyErr))
			return policyErr;

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		if (await db.Users.AnyAsync(u => u.Email == normalized, cancellationToken))
			return "An account with this email already exists.";

		var displayName = $"{fn} {ln}".Trim();
		var user = new UserEntity
		{
			Name = displayName,
			Email = normalized,
			Phone = string.Empty,
			Birthday = birthday.Date.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
			Gender = string.Empty,
			Address = string.Empty,
			Handle = null,
			ProfilePhotoPath = null,
			PasswordHash = hasher.Hash(password)
		};

		db.Users.Add(user);
		await db.SaveChangesAsync(cancellationToken);

		session.SetCurrentUser(user.Id);
		await profilePhoto.SyncFromCurrentUserAsync(data, cancellationToken);
		data.NotifyDataChanged();
		return null;
	}

	public async Task<string?> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
	{
		var normalized = NormalizeEmail(email);
		if (normalized is null)
			return "Enter a valid email address.";
		if (string.IsNullOrEmpty(password))
			return "Password is required.";

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var user = await db.Users.FirstOrDefaultAsync(u => u.Email == normalized, cancellationToken);
		if (user is null)
			return "Invalid email or password.";

		if (string.IsNullOrWhiteSpace(user.PasswordHash))
		{
			if (!PasswordPolicy.TryValidate(password, out var err))
				return err;
			user.PasswordHash = hasher.Hash(password);
			await db.SaveChangesAsync(cancellationToken);
		}
		else if (!hasher.Verify(password, user.PasswordHash))
			return "Invalid email or password.";

		session.SetCurrentUser(user.Id);
		await profilePhoto.SyncFromCurrentUserAsync(data, cancellationToken);
		data.NotifyDataChanged();
		return null;
	}

	public void Logout()
	{
		session.Clear();
		profilePhoto.ClearForLogout();
		data.NotifyDataChanged();
	}

	public async Task<string?> ChangePasswordAsync(
		string currentPassword,
		string newPassword,
		string confirmNewPassword,
		CancellationToken cancellationToken = default)
	{
		var uid = session.CurrentUserId;
		if (uid is null)
			return "You are not signed in.";

		if (!string.Equals(newPassword, confirmNewPassword, StringComparison.Ordinal))
			return "New password and confirmation must match.";

		if (!PasswordPolicy.TryValidate(newPassword, out var policyErr))
			return policyErr;

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var user = await db.Users.FirstOrDefaultAsync(u => u.Id == uid.Value, cancellationToken);
		if (user is null)
			return "Account not found.";

		if (string.IsNullOrWhiteSpace(user.PasswordHash))
			user.PasswordHash = hasher.Hash(newPassword);
		else
		{
			if (!hasher.Verify(currentPassword, user.PasswordHash))
				return "Current password is incorrect.";
			user.PasswordHash = hasher.Hash(newPassword);
		}

		await db.SaveChangesAsync(cancellationToken);
		return null;
	}

	public async Task<bool> TryRestoreSessionAsync(CancellationToken cancellationToken = default)
	{
		session.RestoreFromPreferences();
		var uid = session.CurrentUserId;
		if (uid is null)
			return false;

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var exists = await db.Users.AnyAsync(u => u.Id == uid.Value, cancellationToken);
		if (!exists)
		{
			session.Clear();
			return false;
		}

		await profilePhoto.SyncFromCurrentUserAsync(data, cancellationToken);
		data.NotifyDataChanged();
		return true;
	}

	public async Task<string?> ResetPasswordAsync(
		string email,
		DateTime birthday,
		string newPassword,
		string confirmNewPassword,
		CancellationToken cancellationToken = default)
	{
		var normalized = NormalizeEmail(email);
		if (normalized is null)
			return "Enter a valid email address.";

		if (!string.Equals(newPassword, confirmNewPassword, StringComparison.Ordinal))
			return "New password and confirmation must match.";

		if (!PasswordPolicy.TryValidate(newPassword, out var policyErr))
			return policyErr;

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var user = await db.Users.FirstOrDefaultAsync(u => u.Email == normalized, cancellationToken);
		if (user is null)
			return "Account not found.";

		var expectedBirthday = birthday.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
		if (!string.Equals((user.Birthday ?? string.Empty).Trim(), expectedBirthday, StringComparison.Ordinal))
			return "Birthday does not match this account.";

		user.PasswordHash = hasher.Hash(newPassword);
		await db.SaveChangesAsync(cancellationToken);
		return null;
	}

	static string? NormalizeEmail(string? email)
	{
		if (string.IsNullOrWhiteSpace(email))
			return null;
		var t = email.Trim().ToLowerInvariant();
		return t.Contains('@', StringComparison.Ordinal) ? t : null;
	}
}
