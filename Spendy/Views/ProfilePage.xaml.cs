using Spendy.Services;
using Spendy.ViewModels;

namespace Spendy.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
		BindingContext = Ioc.Services.GetRequiredService<ProfileViewModel>();
	}

	async void OnBack(object? sender, EventArgs e) => await AppNavigation.PopAsync();

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is ProfileViewModel vm)
			await vm.LoadAsync();
	}

	async void OnChangePhoto(object? sender, TappedEventArgs e)
	{
		if (BindingContext is not ProfileViewModel vm)
			return;

		var pick = await DisplayActionSheet("Profile photo", "Cancel", null, "Choose from gallery", "Take photo");
		if (pick is null || pick == "Cancel")
			return;

		try
		{
			FileResult? file = pick == "Choose from gallery"
				? await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions { Title = "Select photo" })
				: await MediaPicker.Default.CapturePhotoAsync();

			if (file is null)
				return;

			var dest = Path.Combine(FileSystem.AppDataDirectory, $"profile_{Guid.NewGuid():N}.jpg");
			await using (var s = await file.OpenReadAsync())
			await using (var o = File.Create(dest))
				await s.CopyToAsync(o);

			vm.ApplyLocalPhoto(dest);
		}
		catch (Exception ex)
		{
			await DisplayAlert("Spendy", $"Could not update photo: {ex.Message}", "OK");
		}
	}
}
