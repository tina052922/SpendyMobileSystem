using Spendy.Data;
using Spendy.Services;

namespace Spendy.Views;

public partial class SplashPage : ContentPage
{
	public SplashPage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		// Complete SQLite migration/seed before login — avoids races where SavingGoals inserts hit an unfinished schema.
		try
		{
			var init = Ioc.Services.GetRequiredService<SpendyDbInitializer>();
			await init.InitializeAsync();
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"[Spendy] Database initialization failed: {ex}");
		}

		// Always show Splash → Get Started → Sign In. Restored sessions are applied on SignInPage (Remember me / saved user id).

		// Animation: show big symbol, then slide left and reveal wordmark.
		try
		{
			if (WordMark is not null)
			{
				WordMark.Opacity = 0;
				WordMark.TranslationX = 12;
			}
			if (LogoRow is not null)
				LogoRow.TranslationX = 0;

			// Keep the wordmark from "lagging" behind the logo.
			await Task.Delay(150);
			await MainThread.InvokeOnMainThreadAsync(async () =>
			{
				if (LogoRow is null || WordMark is null)
					return;

				var slide = LogoRow.TranslateTo(-36, 0, 380, Easing.CubicOut);
				var fadeIn = WordMark.FadeTo(1, 380, Easing.CubicOut);
				var nudge = WordMark.TranslateTo(0, 0, 380, Easing.CubicOut);
				await Task.WhenAll(slide, fadeIn, nudge);
			});
		}
		catch
		{
			// If animation fails on a platform, continue normally.
		}

		await Task.Delay(350);
		if (AppNavigation.TryGetRootNavigationPage() is { } nav)
			await nav.PushAsync(new GetStartedPage());
	}
}
