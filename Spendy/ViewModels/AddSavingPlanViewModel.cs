using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Services;

namespace Spendy.ViewModels;

public partial class AddSavingPlanViewModel : ObservableObject
{
	readonly ISpendyDataService _data;
	readonly IProfilePhotoService _profilePhoto;

	public ImageSource ProfilePhoto => _profilePhoto.Photo;

	[ObservableProperty]
	private string _planName = string.Empty;

	[ObservableProperty]
	private string _targetAmountText = string.Empty;

	[ObservableProperty]
	private int _selectedDurationIndex;

	public ObservableCollection<string> DurationOptions { get; } =
		new(["1 Month", "3 Months", "6 Months", "1 Year"]);

	static readonly int[] DurationMonths = [1, 3, 6, 12];

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

		var idx = Math.Clamp(SelectedDurationIndex, 0, DurationMonths.Length - 1);
		var months = DurationMonths[idx];
		var targetDate = DateTime.Today.AddMonths(months);

		await _data.CreateSavingGoalAsync(PlanName.Trim(), target, targetDate);
		await AppNavigation.PopAsync();
	}
}
