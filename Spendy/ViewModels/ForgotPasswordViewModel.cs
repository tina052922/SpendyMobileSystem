using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Services;

namespace Spendy.ViewModels;

public partial class ForgotPasswordViewModel : ObservableObject
{
	readonly IAuthService _auth;

	[ObservableProperty]
	private string _email = string.Empty;

	[ObservableProperty]
	private DateTime _birthday = new(2000, 1, 15);

	public DateTime MaximumBirthdayDate => DateTime.Today;

	[ObservableProperty]
	private string _newPassword = string.Empty;

	[ObservableProperty]
	private string _confirmNewPassword = string.Empty;

	public ForgotPasswordViewModel(IAuthService auth)
	{
		_auth = auth;
	}

	[RelayCommand]
	async Task ResetAsync()
	{
		var page = Application.Current?.Windows.FirstOrDefault()?.Page;
		if (page is null)
			return;

		var err = await _auth.ResetPasswordAsync(Email, Birthday.Date, NewPassword, ConfirmNewPassword);
		if (err is not null)
		{
			await page.DisplayAlert("Spendy", err, "OK");
			return;
		}

		await page.DisplayAlert("Spendy", "Password reset successfully. Please sign in.", "OK");
		if (AppNavigation.TryGetRootNavigationPage() is { } nav)
			await nav.PopAsync();
	}
}

