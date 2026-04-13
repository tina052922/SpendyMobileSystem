using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Spendy.Data.Entities;
using Spendy.Services;
using Spendy.Views;

namespace Spendy.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
	readonly ISpendyDataService _data;
	readonly IProfilePhotoService _photoService;

	public string[] GenderOptions { get; } =
		["Female", "Male", "Rather not say"];

	[ObservableProperty]
	private ImageSource? _profilePhoto;

	[ObservableProperty]
	private string _name = string.Empty;

	[ObservableProperty]
	private string _email = string.Empty;

	[ObservableProperty]
	private string _phone = string.Empty;

	[ObservableProperty]
	private DateTime _birthdayDate = new(2000, 1, 15);

	[ObservableProperty]
	private string _gender = "Rather not say";

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
			Name = Email = Phone = Address = Handle = string.Empty;
			Gender = "Rather not say";
			BirthdayDate = new DateTime(2000, 1, 15);
			return;
		}

		Name = u.Name;
		Email = u.Email;
		Phone = u.Phone;
		if (DateTime.TryParse(
			    u.Birthday,
			    System.Globalization.CultureInfo.InvariantCulture,
			    System.Globalization.DateTimeStyles.None,
			    out var bd))
			BirthdayDate = bd.Date;
		else if (DateTime.TryParse(u.Birthday, out var bd2))
			BirthdayDate = bd2.Date;
		Gender = NormalizeGender(u.Gender);
		Address = u.Address;
		Handle = u.Handle ?? string.Empty;
	}

	static string NormalizeGender(string? stored)
	{
		if (string.IsNullOrWhiteSpace(stored))
			return "Rather not say";
		var t = stored.Trim();
		foreach (var o in new[] { "Female", "Male", "Rather not say" })
		{
			if (string.Equals(t, o, StringComparison.OrdinalIgnoreCase))
				return o;
		}

		// Legacy values
		if (t.Equals("F", StringComparison.OrdinalIgnoreCase) ||
		    t.Contains("Female", StringComparison.OrdinalIgnoreCase))
			return "Female";
		if (t.Equals("M", StringComparison.OrdinalIgnoreCase) ||
		    t.Contains("Male", StringComparison.OrdinalIgnoreCase))
			return "Male";
		return "Rather not say";
	}

	[RelayCommand]
	async Task Save()
	{
		await _data.UpsertUserAsync(new UserEntity
		{
			Name = Name.Trim(),
			Email = Email.Trim(),
			Phone = Phone.Trim(),
			Birthday = BirthdayDate.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
			Gender = Gender.Trim(),
			Address = Address.Trim(),
			Handle = string.IsNullOrWhiteSpace(Handle) ? null : Handle.Trim()
		});

		if (Shell.Current is not null)
			await Shell.Current.DisplayAlert("Spendy", "Profile saved successfully", "OK");
	}

	public void ApplyLocalPhoto(string filePath)
	{
		_photoService.SetPhotoPath(filePath);
		MainThread.BeginInvokeOnMainThread(() => ProfilePhoto = _photoService.Photo);
	}

	[RelayCommand]
	Task OpenNotificationsAsync() => AppNavigation.PushAsync(new NotificationPage());
}
