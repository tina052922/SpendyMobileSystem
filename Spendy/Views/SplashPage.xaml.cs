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
		await Ioc.Services.GetRequiredService<SpendyDbInitializer>().InitializeAsync();

		var auth = Ioc.Services.GetRequiredService<IAuthService>();
		if (await auth.TryRestoreSessionAsync())
		{
			AppNavigation.GoToMainShell();
			return;
		}

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

			await Task.Delay(550);
			await MainThread.InvokeOnMainThreadAsync(async () =>
			{
				var slide = LogoRow.TranslateTo(-36, 0, 420, Easing.CubicOut);
				var fadeIn = WordMark.FadeTo(1, 420, Easing.CubicOut);
				var nudge = WordMark.TranslateTo(0, 0, 420, Easing.CubicOut);
				await Task.WhenAll(slide, fadeIn, nudge);
			});
		}
		catch
		{
			// If animation fails on a platform, continue normally.
		}

		await Task.Delay(700);
		if (AppNavigation.TryGetRootNavigationPage() is { } nav)
			await nav.PushAsync(new GetStartedPage());
	}
}
