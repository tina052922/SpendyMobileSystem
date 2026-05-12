using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Services;
using Spendy.Views;

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
		StartDate = DateTime.Today;
		EndDate = DateTime.Today.AddMonths(1);
	}

	[RelayCommand]
	async Task CreateAsync()
	{
		ShowStartDateOverlay = false;
		ShowEndDateOverlay = false;

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
				await Shell.Current.DisplayAlert("Spendy", "Target date must be on or after the start date.", "OK");
			return;
		}

		try
		{
			await _data.CreateSavingGoalAsync(PlanName.Trim(), target, StartDate.Date, EndDate.Date)
				.ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			await MainThread.InvokeOnMainThreadAsync(async () =>
			{
				if (Shell.Current is not null)
					await Shell.Current.DisplayAlert(
						"Spendy",
						$"Could not create plan.\n\n{ExceptionDetailFormatter.DescribeForAlert(ex)}",
						"OK");
			});
			return;
		}

		await MainThread.InvokeOnMainThreadAsync(async () =>
		{
			if (Shell.Current is not null)
				await Shell.Current.DisplayAlert("Spendy", "Saving plan created successfully!", "OK");
			await AppNavigation.PopAsync();
			MainShellPage.Instance?.SelectTab(2);
		});
	}
}
