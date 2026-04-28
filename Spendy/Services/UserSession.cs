using Microsoft.Maui.Storage;

namespace Spendy.Services;

public sealed class UserSession : IUserSession
{
	const string UserIdKey = "SpendySignedInUserId_v1";

	public int? CurrentUserId { get; private set; }

	public void SetCurrentUser(int userId, bool persistForNextLaunch = true)
	{
		if (userId <= 0)
			throw new ArgumentOutOfRangeException(nameof(userId));
		CurrentUserId = userId;
		if (persistForNextLaunch)
			Preferences.Set(UserIdKey, userId);
		else
			Preferences.Remove(UserIdKey);
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
