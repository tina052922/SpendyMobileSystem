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

	public ImageSource ProfilePhoto => _profilePhoto.Photo;

	[ObservableProperty]
	private bool _hasPlans;

	public ObservableCollection<SavingPlan> Plans { get; } = new();

	public SavingsViewModel(ISpendyDataService data)
	{
		_data = data;
		_profilePhoto = Ioc.Services.GetRequiredService<IProfilePhotoService>();
		_profilePhoto.Changed += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => OnPropertyChanged(nameof(ProfilePhoto)));

		_data.DataChanged += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => _ = LoadAsync());
		_ = LoadAsync();
	}

	async Task LoadAsync()
	{
		Plans.Clear();
		foreach (var p in await _data.GetSavingPlansAsync(endedOnly: false))
			Plans.Add(p);

		HasPlans = Plans.Count > 0;
	}

	[RelayCommand]
	Task OpenNotificationsAsync() => AppNavigation.PushAsync(new NotificationPage());

	[RelayCommand]
	Task AddPlanAsync() => AppNavigation.PushAsync(new AddSavingPlanPage());

	public bool ShowEmptyState => !HasPlans;

	partial void OnHasPlansChanged(bool value) =>
		OnPropertyChanged(nameof(ShowEmptyState));
}
