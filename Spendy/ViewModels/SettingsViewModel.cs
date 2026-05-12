using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls;
using Spendy.Services;
using Spendy.Views;

namespace Spendy.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
	readonly ICurrencyService _currency;
	readonly IProfilePhotoService _profilePhoto;
	readonly ISpendyDataService _data;
	readonly IAuthService _auth;
	readonly SemaphoreSlim _loadGate = new(1, 1);
	CancellationTokenSource? _loadCts;

	public ImageSource ProfilePhoto => _profilePhoto.Photo;

	[ObservableProperty]
	private string _selectedCurrency = "PHP";

	[ObservableProperty]
	private string _userGreeting = "";

	public bool HasUserGreeting => !string.IsNullOrWhiteSpace(UserGreeting);

	partial void OnUserGreetingChanged(string value) =>
		OnPropertyChanged(nameof(HasUserGreeting));

	[ObservableProperty]
	private string _currentPassword = string.Empty;

	[ObservableProperty]
	private string _newPassword = string.Empty;

	[ObservableProperty]
	private string _confirmNewPassword = string.Empty;

	[ObservableProperty]
	private string _newPasswordStrengthLabel = string.Empty;

	[ObservableProperty]
	private Color _newPasswordStrengthColor = Color.FromArgb("#888888");

	public SettingsViewModel(
		ICurrencyService currency,
		IProfilePhotoService profilePhoto,
		ISpendyDataService data,
		IAuthService auth)
	{
		_currency = currency;
		_profilePhoto = profilePhoto;
		_data = data;
		_auth = auth;

		_profilePhoto.Changed += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => OnPropertyChanged(nameof(ProfilePhoto)));
		_data.DataChanged += (_, _) =>
			RequestGreetingRefresh();
		_selectedCurrency = _currency.Current == AppCurrency.USD ? "USD" : "PHP";
		_currency.Changed += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() =>
			{
				SelectedCurrency = _currency.Current == AppCurrency.USD ? "USD" : "PHP";
			});
		RequestGreetingRefresh(immediate: true);
	}

	partial void OnNewPasswordChanged(string value)
	{
		var score = PasswordPolicy.StrengthScore(NewPassword);
		NewPasswordStrengthLabel = score == 0 ? "" : PasswordPolicy.StrengthLabel(score);
		NewPasswordStrengthColor = score switch
		{
			0 => Color.FromArgb("#888888"),
			1 => Color.FromArgb("#D32F2F"),
			2 => Color.FromArgb("#F57C00"),
			3 => Color.FromArgb("#1976D2"),
			_ => Color.FromArgb("#2E7D32")
		};
	}

	[RelayCommand]
	async Task UpdatePasswordAsync()
	{
		if (Shell.Current is null)
			return;

		var err = await _auth.ChangePasswordAsync(CurrentPassword, NewPassword, ConfirmNewPassword);
		if (err is null)
		{
			CurrentPassword = string.Empty;
			NewPassword = string.Empty;
			ConfirmNewPassword = string.Empty;
			NewPasswordStrengthLabel = string.Empty;
			await Shell.Current.DisplayAlert("Spendy", "Password updated successfully.", "OK");
		}
		else
			await Shell.Current.DisplayAlert("Spendy", err, "OK");
	}

	void RequestGreetingRefresh(bool immediate = false)
	{
		_loadCts?.Cancel();
		_loadCts?.Dispose();
		_loadCts = new CancellationTokenSource();
		var ct = _loadCts.Token;

		_ = Task.Run(async () =>
		{
			try
			{
				if (!immediate)
					await Task.Delay(180, ct).ConfigureAwait(false);
				await RefreshGreetingCoreAsync(ct).ConfigureAwait(false);
			}
			catch (OperationCanceledException)
			{
			}
			catch
			{
			}
		}, ct);
	}

	async Task RefreshGreetingCoreAsync(CancellationToken ct)
	{
		if (!await _loadGate.WaitAsync(0, ct).ConfigureAwait(false))
			return;
		try
		{
			var greeting = await _data.GetUserDisplayNameAsync(ct).ConfigureAwait(false) ?? string.Empty;
			await MainThread.InvokeOnMainThreadAsync(() =>
			{
				if (ct.IsCancellationRequested) return;
				UserGreeting = greeting;
			});
		}
		finally
		{
			_loadGate.Release();
		}
	}

	[RelayCommand]
	Task OpenNotificationsAsync() => AppNavigation.PushAsync(new NotificationPage());

	/// <summary>Copies absolute path of spendy.db (same folder across restarts; differs per device/OS).</summary>
	[RelayCommand]
	async Task CopyDatabasePathAsync()
	{
		if (Shell.Current is null)
			return;

		var path = string.IsNullOrEmpty(SpendyDatabasePaths.SqliteDatabasePath)
			? SpendyDbPathResolver.ResolveSqlitePath()
			: SpendyDatabasePaths.SqliteDatabasePath;

		await Clipboard.Default.SetTextAsync(path);

		await Shell.Current.DisplayAlert(
			"Spendy · Database path",
			"Copied to clipboard:\n\n" + path +
			"\n\nEach phone/PC keeps its own app data folder. To use the same data on Windows and Android, copy this file after closing the app, or use Share database backup.",
			"OK");
	}

	[RelayCommand]
	async Task ShareDatabaseBackupAsync()
	{
		if (Shell.Current is null)
			return;

		var path = string.IsNullOrEmpty(SpendyDatabasePaths.SqliteDatabasePath)
			? SpendyDbPathResolver.ResolveSqlitePath()
			: SpendyDatabasePaths.SqliteDatabasePath;

		if (!File.Exists(path))
		{
			await Shell.Current.DisplayAlert(
				"Spendy",
				"No database file yet. Sign in and add data first.",
				"OK");
			return;
		}

		await Share.Default.RequestAsync(new ShareFileRequest
		{
			Title = "Spendy database backup (spendy.db)",
			File = new ShareFile(path)
		});
	}

	partial void OnSelectedCurrencyChanged(string value)
	{
		var v = string.Equals(value, "USD", StringComparison.OrdinalIgnoreCase)
			? AppCurrency.USD
			: AppCurrency.PHP;
		_currency.Set(v);
	}
}

