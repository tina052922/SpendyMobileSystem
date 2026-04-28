using Spendy.Data;
using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class TransactionHistoryPage : ContentPage
{
	readonly TransactionKind _kind;

	public TransactionHistoryPage(TransactionKind kind)
	{
		InitializeComponent();
		_kind = kind;
		BindingContext = Ioc.Services.GetRequiredService<TransactionHistoryViewModel>();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is TransactionHistoryViewModel vm)
			await vm.LoadAsync(_kind);
	}
}

