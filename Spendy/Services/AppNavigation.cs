using Microsoft.EntityFrameworkCore;
using Spendy.Data;
using Spendy.Views;

namespace Spendy.Services;

public static class AppNavigation
{
	/// <summary>Navigation stack used before sign-in (Splash → Get Started → Sign In).</summary>
	public static NavigationPage? TryGetRootNavigationPage()
	{
		if (Application.Current?.Windows is not { Count: > 0 } windows)
			return null;
		return windows[0].Page as NavigationPage;
	}

	public static void GoToMainShell()
	{
		if (Application.Current is null)
			return;
		MainThread.BeginInvokeOnMainThread(() => _ = GoToMainShellAsync());
	}

	static async Task GoToMainShellAsync()
	{
		if (Application.Current is null)
			return;

		var session = Ioc.Services.GetRequiredService<IUserSession>();
		// Only pull from preferences when we don't already have an in-memory session.
		// This avoids clobbering a successful sign-in where "Remember me" was unchecked.
		if (session.CurrentUserId is null)
			session.RestoreFromPreferences();
		if (session.CurrentUserId is null)
			return;

		await using var db = await Ioc.Services.GetRequiredService<IDbContextFactory<SpendyDbContext>>()
			.CreateDbContextAsync();
		if (!await db.Users.AnyAsync(u => u.Id == session.CurrentUserId.Value))
		{
			Ioc.Services.GetRequiredService<IAuthService>().Logout();
			return;
		}

		if (Application.Current.Windows.Count > 0)
		{
			var win = Application.Current.Windows[0];
			if (win.Page is { } oldPage)
				await oldPage.FadeTo(0, 160, Easing.CubicOut);
			var shell = new Spendy.AppShell { Opacity = 0 };
			win.Page = shell;
			await shell.FadeTo(1, 200, Easing.CubicIn);
		}
	}

	/// <summary>Logout / return to auth: full-screen sign-in stack without bottom nav.</summary>
	public static void GoToSignInStack()
	{
		if (Application.Current is null)
			return;
		MainThread.BeginInvokeOnMainThread(async () =>
		{
			if (Application.Current.Windows.Count > 0)
			{
				var win = Application.Current.Windows[0];
				if (win.Page is { } oldPage)
					await oldPage.FadeTo(0, 160, Easing.CubicOut);
				var root = new NavigationPage(new SignInPage()) { Opacity = 0 };
				win.Page = root;
				await root.FadeTo(1, 200, Easing.CubicIn);
			}
		});
	}

	public static Task PushAsync(Page page, bool animated = true)
	{
		if (Shell.Current is null)
			throw new InvalidOperationException("Shell not active.");
		return Shell.Current.Navigation.PushAsync(page, animated);
	}

	public static Task PopAsync()
	{
		if (Shell.Current is null)
			throw new InvalidOperationException("Shell not active.");
		return Shell.Current.Navigation.PopAsync();
	}

	public static Task PushModalAsync(Page page)
	{
		if (Shell.Current is null)
			throw new InvalidOperationException("Shell not active.");
		return Shell.Current.Navigation.PushModalAsync(page);
	}

	public static Task PopModalAsync()
	{
		if (Shell.Current is null)
			throw new InvalidOperationException("Shell not active.");
		return Shell.Current.Navigation.PopModalAsync();
	}
}
