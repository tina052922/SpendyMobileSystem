using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Services;

namespace Spendy.ViewModels;

public partial class EditSavingPlanViewModel : SavingPlanCalendarViewModelBase
{
	readonly ISpendyDataService _data;
	readonly int _planId;
	readonly IProfilePhotoService _profilePhoto;

	public ImageSource ProfilePhoto => _profilePhoto.Photo;

	[ObservableProperty]
	private string _name = string.Empty;

	[ObservableProperty]
	private string _targetAmountText = string.Empty;

	public EditSavingPlanViewModel(ISpendyDataService data, int planId)
	{
		_data = data;
		_planId = planId;
		_profilePhoto = Ioc.Services.GetRequiredService<IProfilePhotoService>();
		_profilePhoto.Changed += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => OnPropertyChanged(nameof(ProfilePhoto)));
	}

	public async Task LoadAsync()
	{
		var plan = await _data.GetSavingPlanAsync(_planId);
		if (plan is null)
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert("Spendy", "Plan not found.", "OK");
			await AppNavigation.PopAsync();
			return;
		}

		Name = plan.Name;
		TargetAmountText = plan.Target.ToString("0.##", CultureInfo.InvariantCulture);
		var end = plan.TargetDateValue.Date;
		EndDate = end;
		var start = end.AddMonths(-1);
		if (start >= end)
			start = end.AddDays(-14);
		StartDate = start;
		CalendarMonth = new DateTime(end.Year, end.Month, 1);
	}

	[RelayCommand]
	async Task SaveAsync()
	{
		if (string.IsNullOrWhiteSpace(Name))
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert("Spendy", "Enter a plan name.", "OK");
			return;
		}

		var text = TargetAmountText.Trim();
		decimal target = 0;
		if (!decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out target)
			&& !decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out target))
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert("Spendy", "Enter a valid target amount.", "OK");
			return;
		}

		if (target <= 0)
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert("Spendy", "Target amount must be greater than zero.", "OK");
			return;
		}

		if (EndDate.Date < StartDate.Date)
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert("Spendy", "End date must be on or after the start date.", "OK");
			return;
		}

		await _data.UpdateSavingGoalAsync(_planId, Name.Trim(), target, EndDate.Date);
		await AppNavigation.PopAsync();
	}
}
