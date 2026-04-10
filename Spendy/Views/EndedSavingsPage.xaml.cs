using Spendy.Models;
using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class EndedSavingsPage : ContentPage
{
	public EndedSavingsPage()
	{
		InitializeComponent();
		BindingContext = new EndedSavingsViewModel(Ioc.Services.GetRequiredService<ISpendyDataService>());
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is EndedSavingsViewModel vm)
			await vm.LoadAsync();
	}

	async void OnBack(object? sender, EventArgs e) => await AppNavigation.PopAsync();

	async void OnPlanTapped(object? sender, TappedEventArgs e)
	{
		if (sender is BindableObject b && b.BindingContext is SavingPlan plan)
			await AppNavigation.PushAsync(new SavePlanDetailPage(plan.Id));
	}
}
