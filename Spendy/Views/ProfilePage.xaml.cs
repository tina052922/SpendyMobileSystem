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

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is ProfileViewModel vm)
			await vm.LoadAsync();
	}

	void OnFieldUnfocused(object? sender, FocusEventArgs e)
	{
		if (BindingContext is ProfileViewModel vm && vm.SaveCommand.CanExecute(null))
			vm.SaveCommand.Execute(null);
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
