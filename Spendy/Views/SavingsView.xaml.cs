using Spendy.Models;
using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class SavingsView : ContentView
{
	public SavingsView()
	{
		InitializeComponent();
		BindingContext = Ioc.Services.GetRequiredService<SavingsViewModel>();
	}

	async void OnEndedTapped(object? sender, TappedEventArgs e) =>
		await AppNavigation.PushAsync(new EndedSavingsPage());

	async void OnPlanTapped(object? sender, TappedEventArgs e)
	{
		if (sender is BindableObject b && b.BindingContext is SavingPlan plan)
			await AppNavigation.PushAsync(new SavePlanDetailPage(plan.Id));
	}

	async void OnEditClicked(object? sender, EventArgs e)
	{
		if (sender is BindableObject b && b.BindingContext is SavingPlan plan)
			await AppNavigation.PushAsync(new EditSavingPlanPage(plan.Id));
	}

}
