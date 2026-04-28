namespace Spendy.Services;

/// <summary>Tracks the signed-in user id for SQLite row scoping.</summary>
public interface IUserSession
{
	int? CurrentUserId { get; }

	/// <param name="persistForNextLaunch">When false, session is in-memory only (no auto-login after app restart).</param>
	void SetCurrentUser(int userId, bool persistForNextLaunch = true);

	void Clear();

	/// <summary>Restores <see cref="CurrentUserId"/> from app preferences if present.</summary>
	void RestoreFromPreferences();
}
