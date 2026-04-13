using Microsoft.Maui.Storage;

namespace Spendy.Services;

public sealed class UserSession : IUserSession
{
	const string UserIdKey = "SpendySignedInUserId_v1";

	public int? CurrentUserId { get; private set; }

	public void SetCurrentUser(int userId)
	{
		if (userId <= 0)
			throw new ArgumentOutOfRangeException(nameof(userId));
		CurrentUserId = userId;
		Preferences.Set(UserIdKey, userId);
	}

	public void Clear()
	{
		CurrentUserId = null;
		Preferences.Remove(UserIdKey);
	}

	public void RestoreFromPreferences()
	{
		var id = Preferences.Get(UserIdKey, 0);
		CurrentUserId = id > 0 ? id : null;
	}
}
