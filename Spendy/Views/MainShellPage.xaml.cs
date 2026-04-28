using Microsoft.EntityFrameworkCore;
using Spendy.Data;
using Spendy.Services;

namespace Spendy.Views;

public partial class MainShellPage : ContentPage
{
	public static MainShellPage? Instance { get; private set; }

	readonly DashboardView _dashboard;
	readonly StatisticsView _statistics;
	readonly SavingsView _savings;
	readonly SettingsView _settings;

	public MainShellPage()
	{
		InitializeComponent();
		Instance = this;
		_dashboard = new DashboardView();
		_statistics = new StatisticsView();
		_savings = new SavingsView();
		_settings = new SettingsView();
		SectionHost.Content = _dashboard;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		try
		{
			var session = Ioc.Services.GetRequiredService<IUserSession>();
			// Only restore persisted session if we don't already have one (e.g., fresh sign-in).
			if (session.CurrentUserId is null)
				session.RestoreFromPreferences();
			if (session.CurrentUserId is null)
			{
				AppNavigation.GoToSignInStack();
				return;
			}

			await using var db = await Ioc.Services.GetRequiredService<IDbContextFactory<SpendyDbContext>>()
				.CreateDbContextAsync();
			if (!await db.Users.AnyAsync(u => u.Id == session.CurrentUserId.Value))
			{
				Ioc.Services.GetRequiredService<IAuthService>().Logout();
				AppNavigation.GoToSignInStack();
			}
		}
		catch
		{
			AppNavigation.GoToSignInStack();
		}
	}

	/// <summary>Switch tab from code (e.g. after closing an overlay page).</summary>
	public void SelectTab(int index)
	{
		if (index is < 0 or > 3)
			return;
		TabBar.SelectedIndex = index;
		ApplyTabContent(index);
	}

	void ApplyTabContent(int index)
	{
		SectionHost.Content = index switch
		{
			0 => _dashboard,
			1 => _statistics,
			2 => _savings,
			_ => _settings
		};
	}

	void OnTabChanged(object? sender, int index) => ApplyTabContent(index);

	/// <summary>Pop to root (e.g. from Profile) and show the selected main tab.</summary>
	public static async Task SwitchToTabFromOverlayAsync(int index, CancellationToken cancellationToken = default)
	{
		if (Instance is null || Shell.Current is null)
			return;
		if (index is < 0 or > 3)
			return;

		await Shell.Current.Navigation.PopToRootAsync(animated: true);
		cancellationToken.ThrowIfCancellationRequested();
		await MainThread.InvokeOnMainThreadAsync(() => Instance!.SelectTab(index));
	}
}
