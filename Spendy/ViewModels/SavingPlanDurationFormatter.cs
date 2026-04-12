namespace Spendy.ViewModels;

internal static class SavingPlanDurationFormatter
{
	public static string Format(DateTime start, DateTime end)
	{
		var startD = start.Date;
		var endD = end.Date;
		var totalDays = (int)(endD - startD).TotalDays;
		if (totalDays <= 0)
			return "Same day";

		var months = 0;
		var cursor = startD;
		while (cursor.AddMonths(1) <= endD)
		{
			months++;
			cursor = cursor.AddMonths(1);
		}

		var remDays = (int)(endD - cursor).TotalDays;
		if (months > 0 && remDays == 0)
			return months == 1 ? "1 Month" : $"{months} Months";
		if (months > 0)
			return $"{months} Month(s), {remDays} Days";

		if (totalDays % 7 == 0 && totalDays is >= 7 and <= 56)
		{
			var w = totalDays / 7;
			return w == 1 ? "1 Week" : $"{w} Weeks";
		}

		return totalDays == 1 ? "1 Day" : $"{totalDays} Days";
	}
}
