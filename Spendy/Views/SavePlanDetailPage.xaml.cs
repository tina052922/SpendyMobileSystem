using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class SavePlanDetailPage : ContentPage
{
	public SavePlanDetailPage(int planId)
	{
		InitializeComponent();
		BindingContext = new SavePlanDetailViewModel(Ioc.Services.GetRequiredService<ISpendyDataService>(), planId);
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is not SavePlanDetailViewModel vm)
			return;

		await vm.LoadAsync();
	}

	async void OnBack(object? sender, EventArgs e) => await AppNavigation.PopAsync();

	async void OnNotificationsTapped(object? sender, TappedEventArgs e) =>
		await AppNavigation.PushAsync(new NotificationPage());
}
