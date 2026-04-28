using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Spendy.Services;

namespace Spendy.ViewModels;

public partial class ForgotPasswordRequestViewModel : ObservableObject
{
	readonly IAuthService _auth;

	[ObservableProperty]
	private bool _isBusy;

	[ObservableProperty]
	private string _email = string.Empty;

	[ObservableProperty]
	private string _newPassword = string.Empty;

	[ObservableProperty]
	private string _confirmNewPassword = string.Empty;

	public ForgotPasswordRequestViewModel(IAuthService auth)
	{
		_auth = auth;
	}

	[RelayCommand]
	async Task SendResetAsync()
	{
		if (IsBusy)
			return;

		var page = Application.Current?.Windows.FirstOrDefault()?.Page;
		if (page is null)
			return;

		IsBusy = true;
		try
		{
			var err = await _auth.ResetPasswordByEmailAsync(Email, NewPassword, ConfirmNewPassword);
			if (err is not null)
			{
				await page.DisplayAlert("Spendy", err, "OK");
				return;
			}

			try
			{
				await Toast.Make("Password has been reset successfully", ToastDuration.Short).Show();
			}
			catch
			{
			}

			await page.DisplayAlert(
				"Spendy",
				"Password has been reset successfully. You can now sign in with your new password.",
				"OK");

			// Clear fields and return to sign-in for a smooth flow.
			Email = string.Empty;
			NewPassword = string.Empty;
			ConfirmNewPassword = string.Empty;
			if (AppNavigation.TryGetRootNavigationPage() is { } nav)
				await nav.PopAsync();
		}
		finally
		{
			IsBusy = false;
		}
	}
}

