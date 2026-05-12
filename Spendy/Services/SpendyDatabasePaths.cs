namespace Spendy.Services;

/// <summary>Holds the resolved SQLite path for the running session (set during app startup).</summary>
public static class SpendyDatabasePaths
{
	/// <summary>Full path to <see cref="SpendyDbPathResolver.DatabaseFileName"/> for this process.</summary>
	public static string SqliteDatabasePath { get; internal set; } = string.Empty;
}
