using Spendy.ViewModels;

namespace Spendy.Views;

public partial class MandatorySavingsAllocationPage : ContentPage
{
	public MandatorySavingsAllocationPage(MandatorySavingsAllocationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is MandatorySavingsAllocationViewModel vm)
			await vm.LoadAsync();
	}

	protected override bool OnBackButtonPressed()
	{
		MainThread.BeginInvokeOnMainThread(async () =>
		{
			if (BindingContext is MandatorySavingsAllocationViewModel vm)
				await vm.HandleBackRequestedAsync();
		});
		return true;
	}
}
