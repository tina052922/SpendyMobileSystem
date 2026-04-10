using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class StatisticsView : ContentView
{
	public StatisticsView()
	{
		InitializeComponent();
		BindingContext = Ioc.Services.GetRequiredService<StatisticsViewModel>();
	}
}
