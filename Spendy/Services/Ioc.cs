namespace Spendy.Services;

/// <summary>Root service provider set at startup (MAUI host).</summary>
public static class Ioc
{
	public static IServiceProvider Services { get; set; } = default!;
}
