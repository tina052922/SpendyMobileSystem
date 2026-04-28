using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendy.Data;
using Spendy.Models;
using Spendy.Services;

namespace Spendy.ViewModels;

public partial class TransactionHistoryViewModel : ObservableObject
{
	readonly ISpendyDataService _data;
	readonly ICurrencyService _currency;
	readonly SemaphoreSlim _loadGate = new(1, 1);

	[ObservableProperty]
	private bool _isBusy;

	[ObservableProperty]
	private string _title = "HISTORY";

	public ObservableCollection<TransactionItem> Items { get; } = new();

	public TransactionHistoryViewModel(ISpendyDataService data, ICurrencyService currency)
	{
		_data = data;
		_currency = currency;
	}

	public async Task LoadAsync(TransactionKind kind, CancellationToken cancellationToken = default)
	{
		if (!await _loadGate.WaitAsync(0, cancellationToken).ConfigureAwait(false))
			return;
		try
		{
			IsBusy = true;
			Title = kind == TransactionKind.Expense ? "EXPENSE HISTORY" : "INCOME HISTORY";
			var rows = await _data.GetTransactionHistoryAsync(kind, cancellationToken).ConfigureAwait(false);

			await MainThread.InvokeOnMainThreadAsync(() =>
			{
				Items.Clear();
				foreach (var r in rows)
					Items.Add(r);
			});
		}
		finally
		{
			IsBusy = false;
			_loadGate.Release();
		}
	}

	[RelayCommand]
	Task BackAsync() => AppNavigation.PopAsync();
}

