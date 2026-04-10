using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class AddSavingPlanPage : ContentPage
{
	public AddSavingPlanPage()
	{
		InitializeComponent();
		BindingContext = Ioc.Services.GetRequiredService<AddSavingPlanViewModel>();
	}

	async void OnBack(object? sender, EventArgs e) => await AppNavigation.PopAsync();

	async void OnNotifications(object? sender, EventArgs e) =>
		await AppNavigation.PushAsync(new NotificationPage());
}
