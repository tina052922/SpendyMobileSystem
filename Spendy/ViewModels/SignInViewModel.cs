using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Spendy.Services;
using Microsoft.Maui.Storage;

namespace Spendy.ViewModels;

public partial class SignInViewModel : ObservableObject
{
	readonly IAuthService _auth;
	readonly IUserSession _session;
	const string RememberMeKey = "SpendyRememberMe_v1";
	const string RememberedEmailKey = "SpendyRememberedEmail_v1";

	[ObservableProperty]
	private bool _isBusy;

	[ObservableProperty]
	private string _email = string.Empty;

	[ObservableProperty]
	private string _password = string.Empty;

	[ObservableProperty]
	private bool _rememberMe = true;

	public SignInViewModel(IAuthService auth, IUserSession session)
	{
		_auth = auth;
		_session = session;
		LoadRememberMeState();
	}

	void LoadRememberMeState()
	{
		try
		{
			RememberMe = Preferences.Get(RememberMeKey, true);
			if (RememberMe)
			{
				var saved = Preferences.Get(RememberedEmailKey, string.Empty);
				if (!string.IsNullOrWhiteSpace(saved))
					Email = saved;
			}
		}
		catch
		{
			// Best-effort: keep sign-in usable.
		}
	}

	void PersistRememberMeState()
	{
		try
		{
			Preferences.Set(RememberMeKey, RememberMe);
			if (RememberMe)
				Preferences.Set(RememberedEmailKey, (Email ?? string.Empty).Trim().ToLowerInvariant());
			else
				Preferences.Remove(RememberedEmailKey);
		}
		catch
		{
			// Don't block login if preferences are unavailable.
		}
	}

	[RelayCommand]
	async Task SignInAsync()
	{
		if (IsBusy)
			return;

		var page = Application.Current?.Windows.FirstOrDefault()?.Page;
		if (page is null)
			return;

		IsBusy = true;
		try
		{
			var err = await _auth.LoginAsync(Email, Password, RememberMe);
			if (err is not null)
			{
				await page.DisplayAlert("Spendy", err, "OK");
				return;
			}

			// Only proceed if a session was actually established.
			if (_session.CurrentUserId is null)
			{
				await page.DisplayAlert("Spendy", "Sign-in didn’t complete. Please try again.", "OK");
				return;
			}

			try
			{
				await Toast.Make("Login successful", ToastDuration.Short).Show();
			}
			catch
			{
			}

			AppNavigation.GoToMainShell();

			PersistRememberMeState();
		}
		finally
		{
			IsBusy = false;
		}
	}
}
