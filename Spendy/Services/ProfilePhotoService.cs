using Microsoft.Maui.Controls;

namespace Spendy.Services;

public interface IProfilePhotoService
{
	ImageSource Photo { get; }
	string? PhotoPath { get; }
	event EventHandler? Changed;
	void SetPhotoPath(string filePath);
	Task SyncFromCurrentUserAsync(ISpendyDataService data, CancellationToken cancellationToken = default);
	void ClearForLogout();
}

public sealed class ProfilePhotoService : IProfilePhotoService
{
	ImageSource _photo = ImageSource.FromFile("blankprofile.png");
	string? _photoPath;

	readonly WeakEventManager _changed = new();

	public event EventHandler? Changed
	{
		add => _changed.AddEventHandler(value);
		remove => _changed.RemoveEventHandler(value);
	}

	public ImageSource Photo => _photo;
	public string? PhotoPath => _photoPath;

	public void SetPhotoPath(string filePath)
	{
		if (string.IsNullOrWhiteSpace(filePath))
			return;

		_photoPath = filePath.Trim();
		_photo = ImageSource.FromFile(_photoPath);
		_changed.HandleEvent(this, EventArgs.Empty, nameof(Changed));
	}

	public async Task SyncFromCurrentUserAsync(ISpendyDataService data, CancellationToken cancellationToken = default)
	{
		var u = await data.GetCurrentUserAsync(cancellationToken);
		var path = u?.ProfilePhotoPath?.Trim();
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

		_changed.HandleEvent(this, EventArgs.Empty, nameof(Changed));
		await Task.CompletedTask;
	}

	public void ClearForLogout()
	{
		_photoPath = null;
		_photo = ImageSource.FromFile("blankprofile.png");
		_changed.HandleEvent(this, EventArgs.Empty, nameof(Changed));
	}
}
