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
		MainThread.BeginInvokeOnMainThread(() =>
		{
			if (Application.Current.Windows.Count > 0)
				Application.Current.Windows[0].Page = new Spendy.AppShell();
		});
	}

	/// <summary>Logout / return to auth: full-screen sign-in stack without bottom nav.</summary>
	public static void GoToSignInStack()
	{
		if (Application.Current is null)
			return;
		MainThread.BeginInvokeOnMainThread(() =>
		{
			if (Application.Current.Windows.Count > 0)
				Application.Current.Windows[0].Page = new NavigationPage(new SignInPage());
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
