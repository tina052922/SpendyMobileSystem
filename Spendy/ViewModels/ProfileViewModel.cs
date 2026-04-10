using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Spendy.Data.Entities;
using Spendy.Services;

namespace Spendy.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
	readonly ISpendyDataService _data;
	readonly IProfilePhotoService _photoService;

	[ObservableProperty]
	private ImageSource? _profilePhoto;

	[ObservableProperty]
	private string _name = string.Empty;

	[ObservableProperty]
	private string _email = string.Empty;

	[ObservableProperty]
	private string _phone = string.Empty;

	[ObservableProperty]
	private string _birthday = string.Empty;

	[ObservableProperty]
	private string _gender = string.Empty;

	[ObservableProperty]
	private string _address = string.Empty;

	[ObservableProperty]
	private string _handle = string.Empty;

	public ProfileViewModel(ISpendyDataService data)
	{
		_data = data;
		_photoService = Ioc.Services.GetRequiredService<IProfilePhotoService>();
		_data.DataChanged += (_, _) =>
			MainThread.BeginInvokeOnMainThread(() => _ = LoadAsync());
	}

	public async Task LoadAsync()
	{
		_photoService.RefreshFromStorage();
		ProfilePhoto = _photoService.Photo;

		var u = await _data.GetCurrentUserAsync();
		if (u is null)
		{
			Name = Email = Phone = Birthday = Gender = Address = Handle = string.Empty;
			return;
		}

		Name = u.Name;
		Email = u.Email;
		Phone = u.Phone;
		Birthday = u.Birthday;
		Gender = u.Gender;
		Address = u.Address;
		Handle = u.Handle ?? string.Empty;
	}

	[RelayCommand]
	async Task Save()
	{
		await _data.UpsertUserAsync(new UserEntity
		{
			Name = Name.Trim(),
			Email = Email.Trim(),
			Phone = Phone.Trim(),
			Birthday = Birthday.Trim(),
			Gender = Gender.Trim(),
			Address = Address.Trim(),
			Handle = string.IsNullOrWhiteSpace(Handle) ? null : Handle.Trim()
		});

		if (Shell.Current is not null)
			await Shell.Current.DisplayAlertAsync("Spendy", "Profile saved.", "OK");
	}

	public void ApplyLocalPhoto(string filePath)
	{
		_photoService.SetPhotoPath(filePath);
		MainThread.BeginInvokeOnMainThread(() => ProfilePhoto = _photoService.Photo);
	}
}
