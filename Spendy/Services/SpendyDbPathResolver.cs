using Microsoft.Maui.Storage;

namespace Spendy.Services;

/// <summary>
/// Resolves the single SQLite file path for this app installation (Microsoft.Maui.Storage.FileSystem.AppDataDirectory
/// plus <see cref="DatabaseFileName"/>). That location persists across restarts and updates (unlike CacheDirectory).
/// </summary>
/// <remarks>
/// Each OS keeps app storage isolated: a Windows build and an Android build on a phone/emulator are
/// different machines with different folders. They cannot share one physical <c>spendy.db</c> without
/// cloud sync, your own backend, or manually copying the file (e.g. USB/adb). This resolver still gives
/// one stable file per install so data does not “randomly reset” when reopening the app on the same device.
/// </remarks>
public static class SpendyDbPathResolver
{
	public const string DatabaseFileName = "spendy.db";

	/// <summary>Returns the full path to <see cref="DatabaseFileName"/> under app data.</summary>
	public static string ResolveSqlitePath()
	{
		var root = FileSystem.AppDataDirectory.TrimEnd(
			Path.DirectorySeparatorChar,
			Path.AltDirectorySeparatorChar);

		try
		{
			if (!string.IsNullOrEmpty(root))
				Directory.CreateDirectory(root);
		}
		catch
		{
			// Best-effort; SQLite may still create the file next to an existing parent.
		}

		return Path.Combine(root, DatabaseFileName);
	}
}
