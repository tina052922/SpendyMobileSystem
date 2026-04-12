using Microsoft.Maui.Graphics;

namespace Spendy.ViewModels;

/// <summary>One cell in the savings plan month grid (tap sets start or end date).</summary>
public sealed class CalendarDayCell
{
	public DateTime Date { get; init; }
	public string DayText { get; init; } = "";
	public bool IsCurrentMonth { get; init; }
	public bool IsSelected { get; init; }
	public Color TextColor { get; init; } = Colors.White;
	public Color CellBackground { get; init; } = Colors.Transparent;
}
