using Microsoft.Maui.Graphics;

namespace Spendy.ViewModels;

internal static class SavingPlanCalendarHelper
{
	static readonly Color Accent = Color.FromArgb("00B2FF");
	static readonly Color DimText = Color.FromArgb("55FFFFFF");
	static readonly Color NormalText = Colors.White;

	internal static IEnumerable<CalendarDayCell> BuildCells(
		DateTime calendarMonth,
		DateTime startDate,
		DateTime endDate,
		bool selectingStart)
	{
		var first = new DateTime(calendarMonth.Year, calendarMonth.Month, 1);
		var offset = (int)first.DayOfWeek;
		var gridStart = first.AddDays(-offset);
		var selected = selectingStart ? startDate.Date : endDate.Date;

		for (var i = 0; i < 42; i++)
		{
			var d = gridStart.AddDays(i);
			var inMonth = d.Month == calendarMonth.Month;
			var isSelected = d.Date == selected;
			Color text;
			var bg = Colors.Transparent;
			if (isSelected)
			{
				text = Colors.White;
				bg = Accent;
			}
			else if (!inMonth)
				text = DimText;
			else
				text = NormalText;

			yield return new CalendarDayCell
			{
				Date = d,
				DayText = d.Day.ToString(),
				IsCurrentMonth = inMonth,
				IsSelected = isSelected,
				TextColor = text,
				CellBackground = bg,
			};
		}
	}
}
