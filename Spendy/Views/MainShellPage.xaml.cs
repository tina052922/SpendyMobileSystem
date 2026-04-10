namespace Spendy.Views;

public partial class MainShellPage : ContentPage
{
	readonly DashboardView _dashboard;
	readonly StatisticsView _statistics;
	readonly SavingsView _savings;
	readonly SettingsView _settings;

	public MainShellPage()
	{
		InitializeComponent();
		_dashboard = new DashboardView();
		_statistics = new StatisticsView();
		_savings = new SavingsView();
		_settings = new SettingsView();
		SectionHost.Content = _dashboard;
	}

	void OnTabChanged(object? sender, int index)
	{
		SectionHost.Content = index switch
		{
			0 => _dashboard,
			1 => _statistics,
			2 => _savings,
			_ => _settings
		};
	}
}
