using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
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

	/// <summary>Debug-only UI for inspecting SQLite (hidden in Release).</summary>
	public bool ShowDeveloperDatabaseSection =>
#if DEBUG
		true;
#else
		false;
#endif

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

	[RelayCommand]
	async Task CopyDatabasePathForDebugAsync()
	{
#if DEBUG
		if (Shell.Current is null)
			return;

		var path = string.IsNullOrEmpty(SpendyDatabasePaths.SqliteDatabasePath)
			? SpendyDbPathResolver.ResolveSqlitePath()
			: SpendyDatabasePaths.SqliteDatabasePath;

		await Clipboard.Default.SetTextAsync(path);

		var nl = Environment.NewLine;
		var onAndroid = DeviceInfo.Current.Platform == DevicePlatform.Android;
		string hint;
		if (onAndroid)
		{
			hint =
				"DB Browser on Windows cannot open this Android-only folder." + nl + nl +
				"Copy the database to your PC first, then open the copy:" + nl +
				"- Android Studio: View → Tool Windows → Device File Explorer → …/files/spendy.db → Save As." + nl +
				"- Or (USB debugging): adb pull \"" + path + "\" C:\\Temp\\spendy.db" + nl + nl +
				"In DB Browser use File → Open Database and select that saved spendy.db file.";
		}
		else
		{
			hint =
				"In DB Browser: File → Open Database, browse to the folder above and pick spendy.db " +
				"(use the full file name ending in .db, not only \"spendy\")." + nl + nl +
				"You can paste the path into Windows File Explorer’s address bar to open the folder.";
		}

		await Shell.Current.DisplayAlert(
			"Spendy (debug)",
			"Path copied to clipboard." + nl + nl + path + nl + nl + hint + nl + nl +
			"Visual Studio: set Output to Show output from: Debug — search for [Spendy.Database].",
			"OK");
#else
		await Task.CompletedTask;
#endif
	}

	partial void OnSelectedCurrencyChanged(string value)
	{
		var v = string.Equals(value, "USD", StringComparison.OrdinalIgnoreCase)
			? AppCurrency.USD
			: AppCurrency.PHP;
		_currency.Set(v);
	}
}

