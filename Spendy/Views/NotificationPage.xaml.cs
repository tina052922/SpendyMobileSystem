using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class NotificationPage : ContentPage
{
	public NotificationPage()
	{
		InitializeComponent();
		BindingContext = Ioc.Services.GetRequiredService<NotificationViewModel>();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is NotificationViewModel vm)
			await vm.LoadAsync();
	}

	async void OnBack(object? sender, TappedEventArgs e) => await AppNavigation.PopAsync();
}
