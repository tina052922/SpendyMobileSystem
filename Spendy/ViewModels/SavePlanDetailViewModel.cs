using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Data;
using Spendy.Models;
using Spendy.Services;

namespace Spendy.ViewModels;

public partial class SavePlanDetailViewModel : ObservableObject
{
	readonly ISpendyDataService _data;
	readonly int _planId;
	readonly IProfilePhotoService _profilePhoto;

	public ImageSource ProfilePhoto => _profilePhoto.Photo;

	[ObservableProperty]
	private string _planTitle = string.Empty;

	[ObservableProperty]
	private string _amountLine = string.Empty;

	[ObservableProperty]
	private double _progress;

	[ObservableProperty]
	private string _amountInput = string.Empty;

	[ObservableProperty]
	private bool _canTransact = true;

	[ObservableProperty]
	private bool _hasIncome;

	[ObservableProperty]
	private bool _showRestoreGoal;

	public ObservableCollection<SavingHistoryLine> History { get; } = new();

	decimal _currentBalance;

	public SavePlanDetailViewModel(ISpendyDataService data, int planId)
	{
		_data = data;
		_planId = planId;
		_profilePhoto = Ioc.Services.GetRequiredService<IProfilePhotoService>();
		_profilePhoto.Changed += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => OnPropertyChanged(nameof(ProfilePhoto)));
	}

	public async Task LoadAsync()
	{
		HasIncome = await _data.HasAnyIncomeAsync();
		var plan = await _data.GetSavingPlanAsync(_planId);
		if (plan is null)
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert("Spendy", "Plan not found.", "OK");
			await AppNavigation.PopAsync();
			return;
		}

		PlanTitle = plan.Name;
		AmountLine = plan.AmountLine;
		Progress = plan.Progress;
		_currentBalance = plan.Current;

		CanTransact = !plan.IsEnded && HasIncome;
		ShowRestoreGoal = plan.IsEnded;

		History.Clear();
		foreach (var line in await _data.GetSavingHistoryAsync(_planId))
			History.Add(line);

		SaveMoneyCommand.NotifyCanExecuteChanged();
		WithdrawCommand.NotifyCanExecuteChanged();
	}

	[RelayCommand]
	async Task SaveMoneyAsync()
	{
		if (!HasIncome)
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert("Spendy", "Add an income transaction first to use saving plans.", "OK");
			return;
		}
		var (ok, amt) = await TryParseAmountAsync();
		if (!ok)
			return;

		await _data.AddSavingMovementAsync(_planId, amt, SavingMovement.Save, DateTime.Now, null);
		await LoadAsync();
		AmountInput = string.Empty;
	}

	[RelayCommand]
	async Task WithdrawAsync()
	{
		if (!HasIncome)
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert("Spendy", "Add an income transaction first to use saving plans.", "OK");
			return;
		}
		var (ok, amt) = await TryParseAmountAsync();
		if (!ok)
			return;

		if (amt > _currentBalance)
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert("Spendy", "You cannot withdraw more than the saved balance.", "OK");
			return;
		}

		await _data.AddSavingMovementAsync(_planId, amt, SavingMovement.Withdraw, DateTime.Now, null);
		await LoadAsync();
		AmountInput = string.Empty;
	}

	async Task<(bool ok, decimal amt)> TryParseAmountAsync()
	{
		var text = AmountInput.Trim();
		decimal amt = 0;
		if (!decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out amt)
			&& !decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out amt))
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert("Spendy", "Enter a valid amount.", "OK");
			return (false, 0);
		}

		if (amt <= 0)
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert("Spendy", "Amount must be greater than zero.", "OK");
			return (false, 0);
		}

		return (true, amt);
	}

	[RelayCommand]
	async Task EndGoalAsync()
	{
		if (Shell.Current is null)
			return;
		var ok = await Shell.Current.DisplayAlert(
			"Spendy",
			"Move this goal to Ended? You can restore it later from the Ended list.",
			"End goal",
			"Cancel");
		if (!ok)
			return;

		await _data.SetSavingGoalEndedAsync(_planId, true);
		await AppNavigation.PopAsync();
	}

	[RelayCommand]
	async Task RestoreGoalAsync()
	{
		if (Shell.Current is null)
			return;
		var ok = await Shell.Current.DisplayAlert(
			"Spendy",
			"Move this goal back to active savings?",
			"Restore",
			"Cancel");
		if (!ok)
			return;

		await _data.SetSavingGoalEndedAsync(_planId, false);
		await AppNavigation.PopAsync();
	}
}
