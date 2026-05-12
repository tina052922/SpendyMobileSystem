namespace Spendy.ViewModels;

/// <summary>Human-readable span between start and target dates (e.g. "6 months", "1 year").</summary>
internal static class SavingPlanDurationFormatter
{
	public static string Format(DateTime start, DateTime end)
	{
		var startD = start.Date;
		var endD = end.Date;
		if (endD < startD)
			(startD, endD) = (endD, startD);

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
		var years = months / 12;
		months %= 12;

		if (years > 0 && months > 0)
			return years == 1
				? $"1 year, {months} month{(months == 1 ? "" : "s")}"
				: $"{years} years, {months} month{(months == 1 ? "" : "s")}";

		if (years > 0)
		{
			if (remDays <= 0)
				return years == 1 ? "1 year" : $"{years} years";
			return years == 1
				? $"1 year, {remDays} day{(remDays == 1 ? "" : "s")}"
				: $"{years} years, {remDays} day{(remDays == 1 ? "" : "s")}";
		}

		if (months > 0)
		{
			if (remDays <= 0)
				return months == 1 ? "1 month" : $"{months} months";
			return $"{months} month{(months == 1 ? "" : "s")}, {remDays} day{(remDays == 1 ? "" : "s")}";
		}

		if (totalDays % 7 == 0 && totalDays is >= 7 and <= 56)
		{
			var w = totalDays / 7;
			return w == 1 ? "1 week" : $"{w} weeks";
		}

		return totalDays == 1 ? "1 day" : $"{totalDays} days";
	}
}
