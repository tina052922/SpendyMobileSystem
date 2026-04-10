using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Models;
using Spendy.Services;

namespace Spendy.ViewModels;

public partial class EndedSavingsViewModel : ObservableObject
{
	readonly ISpendyDataService _data;

	public ObservableCollection<SavingPlan> Unfinished { get; } = new();
	public ObservableCollection<SavingPlan> Finished { get; } = new();

	[ObservableProperty]
	private bool _hasUnfinished;

	[ObservableProperty]
	private bool _hasFinished;

	[ObservableProperty]
	private bool _showEmptyState = true;

	public EndedSavingsViewModel(ISpendyDataService data)
	{
		_data = data;
		_data.DataChanged += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => _ = LoadAsync());
		_ = LoadAsync();
	}

	public async Task LoadAsync()
	{
		var all = await _data.GetSavingPlansAsync(endedOnly: true);

		Unfinished.Clear();
		Finished.Clear();
		foreach (var p in all.Where(p => !p.IsFinished))
			Unfinished.Add(p);
		foreach (var p in all.Where(p => p.IsFinished))
			Finished.Add(p);

		HasUnfinished = Unfinished.Count > 0;
		HasFinished = Finished.Count > 0;
		ShowEmptyState = all.Count == 0;
	}

	[RelayCommand]
	async Task RestoreToActive(SavingPlan? plan)
	{
		if (plan is null)
			return;
		await _data.SetSavingGoalEndedAsync(plan.Id, false);
	}
}
