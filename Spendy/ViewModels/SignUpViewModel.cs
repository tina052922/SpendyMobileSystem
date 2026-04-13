using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Services;

namespace Spendy.ViewModels;

public partial class SignUpViewModel : ObservableObject
{
	readonly IAuthService _auth;

	[ObservableProperty]
	private string _firstName = string.Empty;

	[ObservableProperty]
	private string _lastName = string.Empty;

	[ObservableProperty]
	private string _email = string.Empty;

	[ObservableProperty]
	private DateTime _birthday = new(2000, 1, 15);

	[ObservableProperty]
	private string _password = string.Empty;

	[ObservableProperty]
	private string _confirmPassword = string.Empty;

	[ObservableProperty]
	private string _passwordStrengthLabel = string.Empty;

	[ObservableProperty]
	private Color _passwordStrengthColor = Color.FromArgb("#888888");

	public DateTime MaximumBirthdayDate => DateTime.Today;

	[ObservableProperty]
	private bool _termsAccepted;

	public SignUpViewModel(IAuthService auth)
	{
		_auth = auth;
	}

	partial void OnPasswordChanged(string value) => UpdateStrength();

	void UpdateStrength()
	{
		var score = PasswordPolicy.StrengthScore(Password);
		PasswordStrengthLabel = score == 0 ? "" : PasswordPolicy.StrengthLabel(score);
		PasswordStrengthColor = score switch
		{
			0 => Color.FromArgb("#888888"),
			1 => Color.FromArgb("#D32F2F"),
			2 => Color.FromArgb("#F57C00"),
			3 => Color.FromArgb("#1976D2"),
			_ => Color.FromArgb("#2E7D32")
		};
	}

	[RelayCommand]
	async Task SignUpAsync()
	{
		var page = Application.Current?.Windows.FirstOrDefault()?.Page;
		if (page is null)
			return;

		if (!TermsAccepted)
		{
			await page.DisplayAlert("Spendy", "Please agree to the Terms & Conditions.", "OK");
			return;
		}

		var err = await _auth.RegisterAsync(
			FirstName,
			LastName,
			Email,
			Birthday.Date,
			Password,
			ConfirmPassword);

		if (err is not null)
		{
			await page.DisplayAlert("Spendy", err, "OK");
			return;
		}

		AppNavigation.GoToMainShell();
	}
}
