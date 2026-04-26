namespace Spendy.Services;

public interface IGoogleAuthService
{
	/// <summary>
	/// True when Google/Firebase sign-in is configured on this build/device.
	/// </summary>
	bool IsConfigured { get; }

	/// <summary>
	/// Attempts Google sign-in. Returns error string on failure; null on success.
	/// </summary>
	Task<string?> SignInAsync(CancellationToken cancellationToken = default);
}

