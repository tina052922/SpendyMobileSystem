using Microsoft.EntityFrameworkCore;
using Spendy.Data;
using Spendy.Data.Entities;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Spendy.Services;

public sealed class AuthService(
	IDbContextFactory<SpendyDbContext> dbFactory,
	IUserSession session,
	IPasswordHasher hasher,
	IProfilePhotoService profilePhoto,
	ISpendyDataService data,
	IEmailSender emailSender) : IAuthService
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
			return "This email is already in use. Please use another email or sign in.";

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

	public async Task<string?> RequestPasswordResetAsync(string email, CancellationToken cancellationToken = default)
	{
		var normalized = NormalizeEmail(email);
		if (normalized is null)
			return "Enter a valid email address.";

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

		// Rate limit: max 3 requests per hour per account.
		var user = await db.Users.FirstOrDefaultAsync(u => u.Email == normalized, cancellationToken);
		if (user is not null)
		{
			var since = DateTime.UtcNow.AddHours(-1);
			var recentCount = await db.PasswordResetTokens.AsNoTracking()
				.Where(t => t.UserId == user.Id && t.CreatedAtUtc >= since)
				.CountAsync(cancellationToken);
			if (recentCount >= 3)
				return "Too many reset requests. Please try again later.";

			var rawToken = GenerateToken();
			var tokenHash = Sha256Hex(rawToken);
			var now = DateTime.UtcNow;

			db.PasswordResetTokens.Add(new PasswordResetTokenEntity
			{
				UserId = user.Id,
				TokenHash = tokenHash,
				CreatedAtUtc = now,
				ExpiresAtUtc = now.AddHours(1),
				UsedAtUtc = null
			});
			await db.SaveChangesAsync(cancellationToken);

			var subject = "Spendy password reset";
			var body =
				$"""
				Hi,

				We received a request to reset your Spendy password.

				Your reset code (valid for 1 hour):
				{rawToken}

				If you didn’t request this, you can ignore this email.
				""";

			try
			{
				await emailSender.SendAsync(normalized, subject, body, cancellationToken);
			}
			catch
			{
				// If email delivery isn't configured, don't leak internals here.
				return "Password reset email could not be sent. Please contact support or try again later.";
			}
		}

		// Avoid email enumeration: if user doesn't exist we still return success.
		return null;
	}

	public async Task<string?> ConfirmPasswordResetAsync(
		string email,
		string token,
		string newPassword,
		string confirmNewPassword,
		CancellationToken cancellationToken = default)
	{
		var normalized = NormalizeEmail(email);
		if (normalized is null)
			return "Enter a valid email address.";

		if (string.IsNullOrWhiteSpace(token))
			return "Enter the reset code from your email.";

		if (!string.Equals(newPassword, confirmNewPassword, StringComparison.Ordinal))
			return "New password and confirmation must match.";

		if (!PasswordPolicy.TryValidate(newPassword, out var policyErr))
			return policyErr;

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var user = await db.Users.FirstOrDefaultAsync(u => u.Email == normalized, cancellationToken);
		if (user is null)
			return "Invalid reset code.";

		var now = DateTime.UtcNow;
		var tokenHash = Sha256Hex(token.Trim());
		var tokenRow = await db.PasswordResetTokens
			.Where(t => t.UserId == user.Id
			            && t.TokenHash == tokenHash
			            && t.UsedAtUtc == null
			            && t.ExpiresAtUtc >= now)
			.OrderByDescending(t => t.CreatedAtUtc)
			.FirstOrDefaultAsync(cancellationToken);
		if (tokenRow is null)
			return "Invalid or expired reset code.";

		user.PasswordHash = hasher.Hash(newPassword);
		tokenRow.UsedAtUtc = now;
		await db.SaveChangesAsync(cancellationToken);
		return null;
	}

	static string GenerateToken()
	{
		Span<byte> bytes = stackalloc byte[32];
		RandomNumberGenerator.Fill(bytes);
		// URL-safe base64 without padding.
		return Convert.ToBase64String(bytes)
			.Replace('+', '-')
			.Replace('/', '_')
			.TrimEnd('=');
	}

	static string Sha256Hex(string raw)
	{
		var bytes = Encoding.UTF8.GetBytes(raw);
		var hash = SHA256.HashData(bytes);
		return Convert.ToHexString(hash);
	}

	static string? NormalizeEmail(string? email)
	{
		if (string.IsNullOrWhiteSpace(email))
			return null;
		var t = email.Trim().ToLowerInvariant();
		return t.Contains('@', StringComparison.Ordinal) ? t : null;
	}
}
