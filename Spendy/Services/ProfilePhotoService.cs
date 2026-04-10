using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace Spendy.Services;

public interface IProfilePhotoService
{
	ImageSource Photo { get; }
	string? PhotoPath { get; }
	event EventHandler? Changed;
	void SetPhotoPath(string filePath);
	void RefreshFromStorage();
}

public sealed class ProfilePhotoService : IProfilePhotoService
{
	const string PrefKey = "SpendyProfilePhotoPath";

	ImageSource _photo = ImageSource.FromFile("blankprofile.png");
	string? _photoPath;

	public event EventHandler? Changed;

	public ImageSource Photo => _photo;
	public string? PhotoPath => _photoPath;

	public ProfilePhotoService()
	{
		RefreshFromStorage();
	}

	public void RefreshFromStorage()
	{
		var path = Preferences.Get(PrefKey, string.Empty);
		if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
		{
			_photoPath = path;
			_photo = ImageSource.FromFile(path);
		}
		else
		{
			_photoPath = null;
			_photo = ImageSource.FromFile("blankprofile.png");
		}

		Changed?.Invoke(this, EventArgs.Empty);
	}

	public void SetPhotoPath(string filePath)
	{
		if (string.IsNullOrWhiteSpace(filePath))
			return;

		Preferences.Set(PrefKey, filePath);
		_photoPath = filePath;
		_photo = ImageSource.FromFile(filePath);
		Changed?.Invoke(this, EventArgs.Empty);
	}
}

