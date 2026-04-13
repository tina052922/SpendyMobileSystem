using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Services;

namespace Spendy.ViewModels;

public partial class SignInViewModel : ObservableObject
{
	readonly IAuthService _auth;

	[ObservableProperty]
	private string _email = string.Empty;

	[ObservableProperty]
	private string _password = string.Empty;

	public SignInViewModel(IAuthService auth)
	{
		_auth = auth;
	}

	[RelayCommand]
	async Task SignInAsync()
	{
		var page = Application.Current?.Windows.FirstOrDefault()?.Page;
		if (page is null)
			return;

		var err = await _auth.LoginAsync(Email, Password);
		if (err is not null)
		{
			await page.DisplayAlert("Spendy", err, "OK");
			return;
		}

		AppNavigation.GoToMainShell();
	}
}
