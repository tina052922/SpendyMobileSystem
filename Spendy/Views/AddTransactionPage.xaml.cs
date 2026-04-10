using System.Collections.Generic;
using System.ComponentModel;
using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class AddTransactionPage : ContentPage
{
	public AddTransactionPage()
	{
		InitializeComponent();
		BindingContext = Ioc.Services.GetRequiredService<AddTransactionViewModel>();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is AddTransactionViewModel vm)
		{
			vm.PropertyChanged += OnVmPropertyChanged;
			await vm.PrepareAsync();
			RebuildCalendar();
		}
	}

	protected override void OnDisappearing()
	{
		if (BindingContext is AddTransactionViewModel vm)
			vm.PropertyChanged -= OnVmPropertyChanged;
		base.OnDisappearing();
	}

	void OnVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(AddTransactionViewModel.SelectedDay))
			MainThread.BeginInvokeOnMainThread(RebuildCalendar);
	}

	void RebuildCalendar()
	{
		if (BindingContext is not AddTransactionViewModel vm)
			return;

		CalendarHost.Children.Clear();

		var today = DateTime.Today;
		var y = today.Year;
		var m = today.Month;
		var daysInMonth = DateTime.DaysInMonth(y, m);
		var first = new DateTime(y, m, 1);
		var startOffset = (int)first.DayOfWeek;
		var prevMonth = first.AddMonths(-1);
		var daysInPrev = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
		var selected = Math.Clamp(vm.SelectedDay, 1, daysInMonth);

		var dayHeaders = new[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
		foreach (var d in dayHeaders)
		{
			CalendarHost.Children.Add(new Label
			{
				Text = d,
				WidthRequest = 40,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromArgb("#BFD0E3"),
				FontSize = 11
			});
		}

		var cells = new List<(int num, bool inCurrentMonth, bool sel)>();
		for (var i = 0; i < startOffset; i++)
		{
			var pd = daysInPrev - startOffset + i + 1;
			cells.Add((pd, false, false));
		}

		for (var d = 1; d <= daysInMonth; d++)
			cells.Add((d, true, d == selected));

		var total = cells.Count;
		var pad = (7 - total % 7) % 7;
		for (var n = 1; n <= pad; n++)
			cells.Add((n, false, false));

		foreach (var c in cells)
		{
			if (!c.inCurrentMonth)
			{
				CalendarHost.Children.Add(new Label
				{
					Text = c.num.ToString(),
					WidthRequest = 38,
					HeightRequest = 38,
					HorizontalTextAlignment = TextAlignment.Center,
					VerticalTextAlignment = TextAlignment.Center,
					TextColor = Color.FromArgb("#62738A"),
					FontSize = 13
				});
				continue;
			}

			var b = new Button
			{
				Text = c.num.ToString(),
				FontSize = 13,
				WidthRequest = 38,
				HeightRequest = 38,
				CornerRadius = 19,
				BackgroundColor = c.sel ? Color.FromArgb("#0335A3") : Colors.Transparent,
				TextColor = Colors.White
			};
			var capture = c.num;
			b.Clicked += (_, _) => vm.SelectedDay = capture;
			CalendarHost.Children.Add(b);
		}
	}

	async void OnBack(object? sender, EventArgs e) => await AppNavigation.PopAsync();
}
