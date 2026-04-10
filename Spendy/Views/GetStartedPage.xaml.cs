using Spendy.Services;

namespace Spendy.Views;

public partial class GetStartedPage : ContentPage
{
	public GetStartedPage()
	{
		InitializeComponent();
	}

	async void OnGetStartedClicked(object? sender, EventArgs e)
	{
		if (AppNavigation.TryGetRootNavigationPage() is { } nav)
			await nav.PushAsync(new SignInPage());
	}
}
