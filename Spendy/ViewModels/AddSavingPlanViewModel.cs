using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Services;

namespace Spendy.ViewModels;

public partial class AddSavingPlanViewModel : SavingPlanCalendarViewModelBase
{
	readonly ISpendyDataService _data;
	readonly IProfilePhotoService _profilePhoto;

	public ImageSource ProfilePhoto => _profilePhoto.Photo;

	[ObservableProperty]
	private string _planName = string.Empty;

	[ObservableProperty]
	private string _targetAmountText = string.Empty;

	public AddSavingPlanViewModel(ISpendyDataService data)
	{
		_data = data;
		_profilePhoto = Ioc.Services.GetRequiredService<IProfilePhotoService>();
		_profilePhoto.Changed += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => OnPropertyChanged(nameof(ProfilePhoto)));
	}

	[RelayCommand]
	async Task CreateAsync()
	{
		if (string.IsNullOrWhiteSpace(PlanName))
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

		await _data.CreateSavingGoalAsync(PlanName.Trim(), target, EndDate.Date);
		await AppNavigation.PopAsync();
	}
}
