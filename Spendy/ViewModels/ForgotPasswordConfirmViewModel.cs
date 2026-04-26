using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Services;

namespace Spendy.ViewModels;

public partial class ForgotPasswordConfirmViewModel : ObservableObject
{
	readonly IAuthService _auth;

	[ObservableProperty]
	private string _email = string.Empty;

	[ObservableProperty]
	private string _token = string.Empty;

	[ObservableProperty]
	private string _newPassword = string.Empty;

	[ObservableProperty]
	private string _confirmNewPassword = string.Empty;

	public ForgotPasswordConfirmViewModel(IAuthService auth, string email)
	{
		_auth = auth;
		Email = email ?? string.Empty;
	}

	[RelayCommand]
	async Task ConfirmAsync()
	{
		var page = Application.Current?.Windows.FirstOrDefault()?.Page;
		if (page is null)
			return;

		var err = await _auth.ConfirmPasswordResetAsync(Email, Token, NewPassword, ConfirmNewPassword);
		if (err is not null)
		{
			await page.DisplayAlert("Spendy", err, "OK");
			return;
		}

		await page.DisplayAlert("Spendy", "Password updated. Please sign in.", "OK");
		if (AppNavigation.TryGetRootNavigationPage() is { } nav)
			await nav.PopToRootAsync();
	}
}

