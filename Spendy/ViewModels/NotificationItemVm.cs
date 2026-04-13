using Microsoft.Maui.Controls;

namespace Spendy.ViewModels;

/// <summary>One row in the in-app notification feed (bell screen).</summary>
public sealed class NotificationItemVm
{
	public required string Title { get; init; }
	public required string Message { get; init; }
	public required string TimeText { get; init; }
	public Color AccentColor { get; init; } = Color.FromArgb("#0335A3");
	public string IconGlyph { get; init; } = "🔔";
}
