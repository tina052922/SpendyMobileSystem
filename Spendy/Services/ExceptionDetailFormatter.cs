using System.Text;

namespace Spendy.Services;

/// <summary>Unwraps EF/SQLite exceptions for logs and optional user-visible diagnostics.</summary>
public static class ExceptionDetailFormatter
{
	public static string DescribeChain(Exception ex, int maxDepth = 8)
	{
		var sb = new StringBuilder();
		var depth = 0;
		for (var e = ex; e is not null && depth < maxDepth; e = e.InnerException, depth++)
		{
			if (sb.Length > 0)
				sb.Append(" → ");
			sb.Append(e.Message.Trim());
		}

		return sb.ToString();
	}

	/// <summary>Short message suitable for an alert (truncated).</summary>
	public static string DescribeForAlert(Exception ex, int maxLen = 420)
	{
		var s = DescribeChain(ex);
		if (s.Length <= maxLen)
			return s;
		return s[..(maxLen - 3)] + "...";
	}
}
