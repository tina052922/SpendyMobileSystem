using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Models;
using Spendy.Services;
using Spendy.Views;

namespace Spendy.ViewModels;

public partial class SavingsViewModel : ObservableObject
{
	readonly ISpendyDataService _data;
	readonly IProfilePhotoService _profilePhoto;
	readonly SemaphoreSlim _loadGate = new(1, 1);
	CancellationTokenSource? _loadCts;

	public ImageSource ProfilePhoto => _profilePhoto.Photo;

	[ObservableProperty]
	private bool _hasPlans;

	[ObservableProperty]
	private string _userGreeting = "";

	public ObservableCollection<SavingPlan> Plans { get; } = new();

	public bool HasUserGreeting => !string.IsNullOrWhiteSpace(UserGreeting);

	partial void OnUserGreetingChanged(string value) =>
		OnPropertyChanged(nameof(HasUserGreeting));

	public SavingsViewModel(ISpendyDataService data)
	{
		_data = data;
		_profilePhoto = Ioc.Services.GetRequiredService<IProfilePhotoService>();
		_profilePhoto.Changed += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => OnPropertyChanged(nameof(ProfilePhoto)));

		_data.DataChanged += (_, _) =>
			RequestRefresh();
		RequestRefresh(immediate: true);
	}

	async Task LoadAsync()
	{
		RequestRefresh(immediate: true);
	}

	void RequestRefresh(bool immediate = false)
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
				await RefreshCoreAsync(ct).ConfigureAwait(false);
			}
			catch (OperationCanceledException)
			{
			}
			catch
			{
			}
		}, ct);
	}

	async Task RefreshCoreAsync(CancellationToken ct)
	{
		if (!await _loadGate.WaitAsync(0, ct).ConfigureAwait(false))
			return;
		try
		{
			var greeting = await _data.GetUserDisplayNameAsync(ct).ConfigureAwait(false) ?? string.Empty;
			var plans = await _data.GetSavingPlansAsync(endedOnly: false, ct).ConfigureAwait(false);
			var hasPlans = plans.Count > 0;

			await MainThread.InvokeOnMainThreadAsync(() =>
			{
				if (ct.IsCancellationRequested) return;
				UserGreeting = greeting;
				Plans.Clear();
				foreach (var p in plans)
					Plans.Add(p);
				HasPlans = hasPlans;
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
	Task AddPlanAsync() => AppNavigation.PushAsync(new AddSavingPlanPage());

	public bool ShowEmptyState => !HasPlans;

	partial void OnHasPlansChanged(bool value) =>
		OnPropertyChanged(nameof(ShowEmptyState));
}
