using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class EditSavingPlanPage : ContentPage
{
	public EditSavingPlanPage(int planId)
	{
		InitializeComponent();
		BindingContext = new EditSavingPlanViewModel(Ioc.Services.GetRequiredService<ISpendyDataService>(), planId);
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is EditSavingPlanViewModel vm)
		{
			await vm.LoadAsync();
			vm.RefreshCalendar();
		}
	}

	async void OnBack(object? sender, EventArgs e) => await AppNavigation.PopAsync();

	async void OnNotificationsTapped(object? sender, TappedEventArgs e) =>
		await AppNavigation.PushAsync(new NotificationPage());
}
