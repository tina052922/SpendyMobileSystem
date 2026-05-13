using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SQLitePCL;
using Spendy.Data;
using Spendy.Services;
using Spendy.ViewModels;
using CommunityToolkit.Maui;

namespace Spendy;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		// Required for SQLitePCLRaw.bundle_green and sqlite-net-pcl (professor requirement).
		Batteries_V2.Init();

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Single canonical file name per installation; path is always FileSystem.AppDataDirectory/spendy.db.
		// EnsureCreated only creates tables when the file is new — existing spendy.db is never replaced here.
		var dbPath = SpendyDbPathResolver.ResolveSqlitePath();
		SpendyDatabasePaths.SqliteDatabasePath = dbPath;
		var existsBefore = File.Exists(dbPath);
		long sz0 = 0;
		if (existsBefore)
		{
			try
			{
				sz0 = new FileInfo(dbPath).Length;
			}
			catch
			{
			}
		}

		SpendyDatabaseDiagnostics.LogPath(dbPath, existsBefore, sz0);

		builder.Services.AddDbContextFactory<SpendyDbContext>(options =>
			options.UseSqlite($"Data Source={dbPath}"));

		builder.Services.AddSingleton<SpendyDbInitializer>();
		builder.Services.AddSingleton<IUserSession, UserSession>();
		builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
		builder.Services.AddSingleton<ICurrencyService, CurrencyService>();
		builder.Services.AddSingleton<ISpendyDataService, SpendyDataService>();
		builder.Services.AddSingleton<IProfilePhotoService, ProfilePhotoService>();
		// Offline app: no email sender / cloud sync / google sign-in.
		builder.Services.AddSingleton<IAuthService, AuthService>();

		builder.Services.AddTransient<SignInViewModel>();
		builder.Services.AddTransient<SignUpViewModel>();
		builder.Services.AddTransient<ForgotPasswordRequestViewModel>();

		builder.Services.AddSingleton<DashboardViewModel>();
		builder.Services.AddSingleton<StatisticsViewModel>();
		builder.Services.AddSingleton<SavingsViewModel>();
		builder.Services.AddSingleton<SettingsViewModel>();
		builder.Services.AddTransient<AddTransactionViewModel>();
		builder.Services.AddTransient<NotificationViewModel>();
		builder.Services.AddTransient<ProfileViewModel>();
		builder.Services.AddTransient<AddSavingPlanViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif
		// Show Information-level logs from our category in Debug Output (some templates default to Warning).
		builder.Logging.AddFilter("Spendy.Database", LogLevel.Information);

		var app = builder.Build();
		Ioc.Services = app.Services;

		try
		{
			var log = app.Services.GetRequiredService<ILoggerFactory>()
				.CreateLogger("Spendy.Database");
			var path = SpendyDatabasePaths.SqliteDatabasePath;
			var exists = File.Exists(path);
			long bytes = 0;
			if (exists)
			{
				try
				{
					bytes = new FileInfo(path).Length;
				}
				catch
				{
				}
			}

			log.LogInformation(
				"SQLite database: Path={DbPath}, Exists={Exists}, SizeBytes={Size}",
				path,
				exists,
				bytes);
			log.LogInformation(
				"Note: Windows vs Android use different sandboxes. DB Browser on PC cannot open Android internal paths; pull spendy.db first (adb or Device File Explorer).");
		}
		catch
		{
			SpendyDatabaseDiagnostics.LogPath(
				SpendyDatabasePaths.SqliteDatabasePath,
				File.Exists(SpendyDatabasePaths.SqliteDatabasePath),
				0);
		}

		return app;
	}
}
