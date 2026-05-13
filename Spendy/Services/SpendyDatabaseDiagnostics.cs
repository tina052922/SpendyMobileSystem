using System.Diagnostics;

namespace Spendy.Services;

/// <summary>Writes the SQLite path to channels Visual Studio always shows (Debug output + console).</summary>
public static class SpendyDatabaseDiagnostics
{
	public static void LogPath(string path, bool fileExists, long sizeBytes)
	{
		var line =
			$"[Spendy.Database] spendy.db path: {path} | Exists={fileExists} | SizeBytes={sizeBytes}";
		Debug.WriteLine(line);
		Trace.WriteLine(line);
		try
		{
			Console.WriteLine(line);
		}
		catch
		{
		}
	}
}
