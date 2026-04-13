using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Data;
using Spendy.Models;
using Spendy.Services;

using Spendy.Views;

namespace Spendy.ViewModels;

public partial class AddTransactionViewModel : ObservableObject
{
	public const decimal MandatorySavingsIncomeThreshold = 20_000m;
	public const decimal MandatorySavingsRate = 0.02m;

	readonly ISpendyDataService _data;
	readonly IProfilePhotoService _profilePhoto;
	readonly ICurrencyService _currency;

	public ImageSource ProfilePhoto => _profilePhoto.Photo;

	[ObservableProperty]
	private bool _isExpenseMode = true;

	[ObservableProperty]
	private string _amountText = string.Empty;

	[ObservableProperty]
	private string _notes = string.Empty;

	[ObservableProperty]
	private int _selectedDay;

	[ObservableProperty]
	private CategoryPickItem? _selectedExpenseCategory;

	[ObservableProperty]
	private CategoryPickItem? _selectedIncomeCategory;

	public ObservableCollection<CategoryPickItem> ExpensePicker { get; } = new();
	public ObservableCollection<CategoryPickItem> IncomePicker { get; } = new();

	public bool IsIncomeMode => !IsExpenseMode;

	public Color ExpenseAccentBackground =>
		IsExpenseMode ? Color.FromArgb("#0335A3") : Color.FromArgb("#3E4E65");

	public Color IncomeAccentBackground =>
		!IsExpenseMode ? Color.FromArgb("#0335A3") : Color.FromArgb("#3E4E65");

	public AddTransactionViewModel(ISpendyDataService data)
	{
		_data = data;
		_profilePhoto = Ioc.Services.GetRequiredService<IProfilePhotoService>();
		_currency = Ioc.Services.GetRequiredService<ICurrencyService>();
		_profilePhoto.Changed += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => OnPropertyChanged(nameof(ProfilePhoto)));
		SelectedDay = DateTime.Today.Day;
	}

	/// <summary>Call from <see cref="AddTransactionPage"/> before first appear when opening from Dashboard (+).</summary>
	public void ApplyInitialMode(bool openWithIncomeMode) =>
		IsExpenseMode = !openWithIncomeMode;

	partial void OnIsExpenseModeChanged(bool value)
	{
		OnPropertyChanged(nameof(IsIncomeMode));
		OnPropertyChanged(nameof(ExpenseAccentBackground));
		OnPropertyChanged(nameof(IncomeAccentBackground));
		_ = PrepareAsync();
	}

	public async Task PrepareAsync()
	{
		var expense = await _data.GetCategoriesForPickerAsync(TransactionKind.Expense);
		var income = await _data.GetCategoriesForPickerAsync(TransactionKind.Income);

		ExpensePicker.Clear();
		foreach (var row in expense)
			ExpensePicker.Add(new CategoryPickItem { Id = row.Id, Icon = row.Icon, Name = row.Name });
		IncomePicker.Clear();
		foreach (var row in income)
			IncomePicker.Add(new CategoryPickItem { Id = row.Id, Icon = row.Icon, Name = row.Name });

		SelectedExpenseCategory = ExpensePicker.FirstOrDefault();
		SelectedIncomeCategory = IncomePicker.FirstOrDefault();

		// No income yet — show Income tab (expense is blocked until first income exists).
		if (!await _data.HasAnyIncomeAsync() && IsExpenseMode)
			IsExpenseMode = false;
	}

	partial void OnSelectedExpenseCategoryChanged(CategoryPickItem? value)
	{
		foreach (var x in ExpensePicker)
			x.IsSelected = x == value;
	}

	partial void OnSelectedIncomeCategoryChanged(CategoryPickItem? value)
	{
		foreach (var x in IncomePicker)
			x.IsSelected = x == value;
	}

	[RelayCommand]
	async Task SelectExpense()
	{
		if (!await _data.HasAnyIncomeAsync())
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert(
					"Spendy",
					"Please add income first for accurate budget tracking.",
					"OK");
			return;
		}

		IsExpenseMode = true;
	}

	[RelayCommand]
	void SelectIncome() => IsExpenseMode = false;

	[RelayCommand]
	Task OpenNotificationsAsync() => AppNavigation.PushAsync(new NotificationPage());

	[RelayCommand]
	async Task SaveAsync()
	{
		var text = AmountText.Trim();
		decimal amt = 0;
		if (!decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out amt)
			&& !decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out amt))
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert("Spendy", "Enter a valid amount.", "OK");
			return;
		}

		if (amt <= 0)
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert("Spendy", "Amount must be greater than zero.", "OK");
			return;
		}

		var kind = IsExpenseMode ? TransactionKind.Expense : TransactionKind.Income;
		if (kind == TransactionKind.Expense && !await _data.HasAnyIncomeAsync())
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert(
					"Spendy",
					"Please add income first for accurate budget tracking.",
					"OK");
			return;
		}

		var cat = IsExpenseMode ? SelectedExpenseCategory : SelectedIncomeCategory;
		if (cat is null)
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert("Spendy", "Pick a category.", "OK");
			return;
		}

		var y = DateTime.Today.Year;
		var m = DateTime.Today.Month;
		var dim = DateTime.DaysInMonth(y, m);
		var day = Math.Clamp(SelectedDay, 1, dim);
		var date = new DateTime(y, m, day, 12, 0, 0, DateTimeKind.Local);

		if (kind == TransactionKind.Income && amt >= MandatorySavingsIncomeThreshold)
		{
			var mandatory = decimal.Round(amt * MandatorySavingsRate, 2, MidpointRounding.AwayFromZero);
			if (mandatory < 0.01m)
				mandatory = 0.01m;

			var notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim();
			var allocVm = new MandatorySavingsAllocationViewModel(
				_data, _currency, _profilePhoto, amt, mandatory, cat.Id, date, notes);
			await AppNavigation.PushModalAsync(new MandatorySavingsAllocationPage(allocVm));
			return;
		}

		await _data.AddTransactionAsync(amt, kind, cat.Id, date,
			string.IsNullOrWhiteSpace(Notes) ? null : Notes);
		await AppNavigation.PopAsync();
	}
}
