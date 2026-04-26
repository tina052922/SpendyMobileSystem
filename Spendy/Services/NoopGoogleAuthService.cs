namespace Spendy.Services;

/// <summary>
/// Safe fallback when Firebase/Google isn't configured.
/// Keeps the app functional without external dependencies.
/// </summary>
public sealed class NoopGoogleAuthService : IGoogleAuthService
{
	public bool IsConfigured => false;

	public Task<string?> SignInAsync(CancellationToken cancellationToken = default)
	{
		_ = cancellationToken;
		return Task.FromResult<string?>(
			"Google sign-in isn’t configured on this build. You can still sign in using email and password.");
	}
}

