using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
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

		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "spendy.db");
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

		var app = builder.Build();
		Ioc.Services = app.Services;

		return app;
	}
}
