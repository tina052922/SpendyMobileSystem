using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;

namespace Spendy.ViewModels;

/// <summary>Shared start/end calendar UI for add and edit saving plan screens.</summary>
public abstract partial class SavingPlanCalendarViewModelBase : ObservableObject
{
	[ObservableProperty]
	private DateTime _startDate = DateTime.Today;

	[ObservableProperty]
	private DateTime _endDate = DateTime.Today.AddMonths(1);

	[ObservableProperty]
	private DateTime _calendarMonth = new(DateTime.Today.Year, DateTime.Today.Month, 1);

	[ObservableProperty]
	private bool _isSelectingStartDate = true;

	[ObservableProperty]
	private int _durationValue = 1;

	[ObservableProperty]
	private string _durationUnit = "Months";

	public IReadOnlyList<string> DurationUnits => ["Months", "Years"];

	public ObservableCollection<CalendarDayCell> CalendarDays { get; } = new();

	public string CalendarTitle => CalendarMonth.ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture);

	public string DurationText => SavingPlanDurationFormatter.Format(StartDate, EndDate);

	public Color StartToggleBg =>
		IsSelectingStartDate ? Color.FromArgb("00B2FF") : Color.FromArgb("3E4E65");

	public Color EndToggleBg =>
		!IsSelectingStartDate ? Color.FromArgb("00B2FF") : Color.FromArgb("3E4E65");

	protected SavingPlanCalendarViewModelBase()
	{
		RebuildCalendar();
	}

	partial void OnStartDateChanged(DateTime value)
	{
		RebuildCalendar();
		OnPropertyChanged(nameof(DurationText));
	}

	partial void OnEndDateChanged(DateTime value)
	{
		RebuildCalendar();
		OnPropertyChanged(nameof(DurationText));
	}

	partial void OnDurationValueChanged(int value)
	{
		if (value < 1)
			DurationValue = 1;
	}

	[RelayCommand]
	void ApplyDuration()
	{
		var v = DurationValue;
		if (v < 1)
			v = 1;

		EndDate = DurationUnit == "Years"
			? StartDate.Date.AddYears(v)
			: StartDate.Date.AddMonths(v);
	}

	partial void OnCalendarMonthChanged(DateTime value) => RebuildCalendar();

	partial void OnIsSelectingStartDateChanged(bool value)
	{
		RebuildCalendar();
		OnPropertyChanged(nameof(StartToggleBg));
		OnPropertyChanged(nameof(EndToggleBg));
	}

	protected void RebuildCalendar()
	{
		CalendarDays.Clear();
		foreach (var c in SavingPlanCalendarHelper.BuildCells(CalendarMonth, StartDate, EndDate, IsSelectingStartDate))
			CalendarDays.Add(c);
	}

	public void RefreshCalendar() => RebuildCalendar();

	[RelayCommand]
	void SelectStartMode() => IsSelectingStartDate = true;

	[RelayCommand]
	void SelectEndMode() => IsSelectingStartDate = false;

	[RelayCommand]
	void PrevMonth() => CalendarMonth = CalendarMonth.AddMonths(-1);

	[RelayCommand]
	void NextMonth() => CalendarMonth = CalendarMonth.AddMonths(1);

	[RelayCommand]
	void SelectDay(CalendarDayCell? cell)
	{
		if (cell is null || !cell.IsCurrentMonth)
			return;

		var d = cell.Date.Date;
		if (IsSelectingStartDate)
		{
			StartDate = d;
			if (EndDate.Date < StartDate.Date)
				EndDate = StartDate;
		}
		else
		{
			EndDate = d;
			if (EndDate.Date < StartDate.Date)
				StartDate = EndDate;
		}
	}
}
