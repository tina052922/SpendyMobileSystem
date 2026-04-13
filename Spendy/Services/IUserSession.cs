namespace Spendy.Services;

/// <summary>Tracks the signed-in user id for SQLite row scoping.</summary>
public interface IUserSession
{
	int? CurrentUserId { get; }

	void SetCurrentUser(int userId);

	void Clear();

	/// <summary>Restores <see cref="CurrentUserId"/> from app preferences if present.</summary>
	void RestoreFromPreferences();
}
