using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Models;
using Spendy.Services;

namespace Spendy.ViewModels;

public partial class MandatoryGoalPickRow : ObservableObject
{
	public SavingPlan Plan { get; }

	[ObservableProperty]
	private bool _isSelected;

	public MandatoryGoalPickRow(SavingPlan plan)
	{
		Plan = plan;
	}
}

public partial class MandatorySavingsAllocationViewModel : ObservableObject
{
	readonly ISpendyDataService _data;
	readonly ICurrencyService _currency;
	readonly decimal _incomeAmount;
	readonly decimal _mandatoryAmount;
	readonly int _categoryId;
	readonly DateTime _transactionDate;
	readonly string? _notes;

	[ObservableProperty]
	private string _messageText = string.Empty;

	[ObservableProperty]
	private string _subtitleText = string.Empty;

	[ObservableProperty]
	private bool _hasGoals;

	/// <summary>False until <see cref="LoadAsync"/> completes — avoids showing "go back" before we know if goals exist.</summary>
	bool _goalsLoaded;

	[ObservableProperty]
	private bool _canConfirm;

	[ObservableProperty]
	private MandatoryGoalPickRow? _selectedRow;

	public ObservableCollection<MandatoryGoalPickRow> GoalRows { get; } = new();

	public MandatorySavingsAllocationViewModel(
		ISpendyDataService data,
		ICurrencyService currency,
		decimal incomeAmount,
		decimal mandatoryAmount,
		int categoryId,
		DateTime transactionDate,
		string? notes)
	{
		_data = data;
		_currency = currency;
		_incomeAmount = incomeAmount;
		_mandatoryAmount = mandatoryAmount;
		_categoryId = categoryId;
		_transactionDate = transactionDate;
		_notes = notes;
		MessageText = $"{_currency.Symbol}{mandatoryAmount.ToString("N2", _currency.Culture)} must be allocated to savings.";
		SubtitleText =
			$"2% of your {_currency.Symbol}{incomeAmount.ToString("N2", _currency.Culture)} income (required when income is {_currency.Symbol}{AddTransactionViewModel.MandatorySavingsIncomeThreshold.ToString("N0", _currency.Culture)} or more).";
	}

	partial void OnSelectedRowChanged(MandatoryGoalPickRow? value)
	{
		foreach (var r in GoalRows)
			r.IsSelected = r == value;

		UpdateCanConfirm();
	}

	partial void OnHasGoalsChanged(bool value)
	{
		OnPropertyChanged(nameof(HasNoGoals));
		UpdateCanConfirm();
		CancelWithoutSavingCommand.NotifyCanExecuteChanged();
	}

	public bool HasNoGoals => !HasGoals;

	void UpdateCanConfirm() =>
		CanConfirm = HasGoals && SelectedRow is not null;

	public async Task LoadAsync()
	{
		SelectedRow = null;
		GoalRows.Clear();
		_goalsLoaded = false;
		CancelWithoutSavingCommand.NotifyCanExecuteChanged();

		foreach (var p in await _data.GetSavingPlansAsync(endedOnly: false))
			GoalRows.Add(new MandatoryGoalPickRow(p));

		HasGoals = GoalRows.Count > 0;
		_goalsLoaded = true;
		CancelWithoutSavingCommand.NotifyCanExecuteChanged();
		UpdateCanConfirm();
	}

	[RelayCommand]
	void SelectGoal(MandatoryGoalPickRow? row)
	{
		if (row is null)
			return;
		SelectedRow = row;
	}

	[RelayCommand]
	async Task RefreshGoalsAsync() => await LoadAsync();

	[RelayCommand]
	async Task ConfirmAllocationAsync()
	{
		if (SelectedRow is null || !HasGoals)
			return;

		await _data.AddIncomeWithMandatorySavingsAsync(
			_incomeAmount,
			_categoryId,
			_transactionDate,
			_notes,
			SelectedRow.Plan.Id,
			_mandatoryAmount);

		await AppNavigation.PopModalAsync();
		await AppNavigation.PopAsync();
	}

	bool CanCancelWithoutSaving() => _goalsLoaded && !HasGoals;

	[RelayCommand(CanExecute = nameof(CanCancelWithoutSaving))]
	async Task CancelWithoutSavingAsync()
	{
		if (HasGoals || Shell.Current is null)
			return;

		var ok = await Shell.Current.DisplayAlert(
			"Go back without saving?",
			"This income will not be saved. Add a saving goal on the Savings tab, then try again with a lower amount or after creating a goal.",
			"Go back",
			"Stay");
		if (!ok)
			return;

		await PopAddTransactionWithoutSavingAsync();
	}

	static async Task PopAddTransactionWithoutSavingAsync()
	{
		await AppNavigation.PopModalAsync();
		await AppNavigation.PopAsync();
	}

	/// <summary>Hardware back: blocked when goals exist; when none exist, same confirm as CancelWithoutSaving.</summary>
	public async Task HandleBackRequestedAsync()
	{
		if (Shell.Current is null)
			return;

		if (!_goalsLoaded)
			return;

		if (HasGoals)
		{
			await Shell.Current.DisplayAlert(
				"Allocate Mandatory Savings",
				"Select a saving goal, then tap Confirm Allocation to continue.",
				"OK");
			return;
		}

		var ok = await Shell.Current.DisplayAlert(
			"Go back without saving?",
			"This income will not be saved. Add a saving goal on the Savings tab, then try again.",
			"Go back",
			"Stay");
		if (ok)
			await PopAddTransactionWithoutSavingAsync();
	}
}
