using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
using Spendy.Data;
using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "spendy.db");
		builder.Services.AddDbContextFactory<SpendyDbContext>(options =>
			options.UseSqlite($"Data Source={dbPath}"));

		builder.Services.AddSingleton<SpendyDbInitializer>();
		builder.Services.AddSingleton<IProfilePhotoService, ProfilePhotoService>();
		builder.Services.AddSingleton<ICurrencyService, CurrencyService>();
		builder.Services.AddSingleton<ISpendyDataService, SpendyDataService>();

		builder.Services.AddSingleton<DashboardViewModel>();
		builder.Services.AddSingleton<StatisticsViewModel>();
		builder.Services.AddSingleton<SavingsViewModel>();
		builder.Services.AddSingleton<SettingsViewModel>();
		builder.Services.AddTransient<AddTransactionViewModel>();
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
