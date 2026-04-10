using Spendy.Services;

namespace Spendy.Views;

public partial class NotificationPage : ContentPage
{
	public NotificationPage()
	{
		InitializeComponent();
	}

	async void OnBack(object? sender, EventArgs e) => await AppNavigation.PopAsync();
}
