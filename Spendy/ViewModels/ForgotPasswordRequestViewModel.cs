using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Services;
using Spendy.Views;

namespace Spendy.ViewModels;

public partial class ForgotPasswordRequestViewModel : ObservableObject
{
	readonly IAuthService _auth;

	[ObservableProperty]
	private string _email = string.Empty;

	public ForgotPasswordRequestViewModel(IAuthService auth)
	{
		_auth = auth;
	}

	[RelayCommand]
	async Task SendResetAsync()
	{
		var page = Application.Current?.Windows.FirstOrDefault()?.Page;
		if (page is null)
			return;

		var err = await _auth.RequestPasswordResetAsync(Email);
		if (err is not null)
		{
			await page.DisplayAlert("Spendy", err, "OK");
			return;
		}

		await page.DisplayAlert("Spendy", "If an account exists for this email, we sent a reset code.", "OK");
		if (AppNavigation.TryGetRootNavigationPage() is { } nav)
			await nav.PushAsync(new ForgotPasswordConfirmPage(Email.Trim()));
	}
}

