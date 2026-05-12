using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Spendy.ViewModels;

/// <summary>Start/target dates, read-only duration text, and optional calendar overlay for add/edit plan screens.</summary>
public abstract partial class SavingPlanCalendarViewModelBase : ObservableObject
{
	[ObservableProperty]
	private DateTime _startDate = DateTime.Today;

	[ObservableProperty]
	private DateTime _endDate = DateTime.Today.AddMonths(1);

	[ObservableProperty]
	private bool _showStartDateOverlay;

	[ObservableProperty]
	private bool _showEndDateOverlay;

	public string StartDateDisplay =>
		StartDate.ToString("dddd, MMM d, yyyy", CultureInfo.CurrentCulture);

	public string EndDateDisplay =>
		EndDate.ToString("dddd, MMM d, yyyy", CultureInfo.CurrentCulture);

	public string PlanDurationText =>
		SavingPlanDurationFormatter.Format(StartDate.Date, EndDate.Date);

	public bool ShowDateDimmer => ShowStartDateOverlay || ShowEndDateOverlay;

	partial void OnStartDateChanged(DateTime value)
	{
		if (EndDate.Date < value.Date)
			EndDate = value.Date;
		NotifyDateBindings();
	}

	partial void OnEndDateChanged(DateTime value)
	{
		if (value.Date < StartDate.Date)
			StartDate = value.Date;
		NotifyDateBindings();
	}

	partial void OnShowStartDateOverlayChanged(bool value) =>
		OnPropertyChanged(nameof(ShowDateDimmer));

	partial void OnShowEndDateOverlayChanged(bool value) =>
		OnPropertyChanged(nameof(ShowDateDimmer));

	void NotifyDateBindings()
	{
		OnPropertyChanged(nameof(StartDateDisplay));
		OnPropertyChanged(nameof(EndDateDisplay));
		OnPropertyChanged(nameof(PlanDurationText));
	}

	[RelayCommand]
	void OpenStartDatePicker()
	{
		ShowEndDateOverlay = false;
		ShowStartDateOverlay = true;
	}

	[RelayCommand]
	void OpenEndDatePicker()
	{
		ShowStartDateOverlay = false;
		ShowEndDateOverlay = true;
	}

	[RelayCommand]
	void CloseDateOverlay()
	{
		ShowStartDateOverlay = false;
		ShowEndDateOverlay = false;
	}
}
