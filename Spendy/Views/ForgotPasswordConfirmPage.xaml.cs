using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class ForgotPasswordConfirmPage : ContentPage
{
	public ForgotPasswordConfirmPage(string email)
	{
		InitializeComponent();
		BindingContext = ActivatorUtilities.CreateInstance<ForgotPasswordConfirmViewModel>(Ioc.Services, email);
	}
}

