using Spendy.Views;

namespace Spendy.Controls;

public partial class SpendyTabBar : ContentView
{
	public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
		nameof(SelectedIndex),
		typeof(int),
		typeof(SpendyTabBar),
		0,
		propertyChanged: OnSelectedIndexChanged);

	/// <summary>When false, hides the tab bar (navbar=none — e.g. if host ever shows chrome without tabs).</summary>
	public static readonly BindableProperty ShowTabBarProperty = BindableProperty.Create(
		nameof(ShowTabBar),
		typeof(bool),
		typeof(SpendyTabBar),
		true,
		propertyChanged: OnShowTabBarChanged);

	public int SelectedIndex
	{
		get => (int)GetValue(SelectedIndexProperty);
		set => SetValue(SelectedIndexProperty, value);
	}

	public bool ShowTabBar
	{
		get => (bool)GetValue(ShowTabBarProperty);
		set => SetValue(ShowTabBarProperty, value);
	}

	public event EventHandler<int>? TabChanged;

	public SpendyTabBar()
	{
		InitializeComponent();
		UpdateStripImage();
	}

	static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SpendyTabBar bar)
			bar.UpdateStripImage();
	}

	static void OnShowTabBarChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is not SpendyTabBar bar)
			return;
		var show = (bool)(newValue ?? true);
		bar.IsVisible = show;
		bar.HeightRequest = show ? 86 : 0;
	}

	void OnHomeTapped(object? sender, EventArgs e) => SetTab(0);

	void OnStatsTapped(object? sender, EventArgs e) => SetTab(1);

	void OnSavingsTapped(object? sender, EventArgs e) => SetTab(2);

	void OnSettingsTapped(object? sender, EventArgs e) => SetTab(3);

	void SetTab(int index)
	{
		if (Shell.Current?.CurrentPage is MainShellPage)
		{
			if (SelectedIndex == index)
				return;
			SelectedIndex = index;
			TabChanged?.Invoke(this, index);
			return;
		}

		_ = MainShellPage.SwitchToTabFromOverlayAsync(index);
	}

	void UpdateStripImage()
	{
		// MauiImage resource names match renamed files under images/ (see Spendy.csproj).
		StripImage.Source = SelectedIndex switch
		{
			-1 => "navbar_none.png",
			0 => "navbar_home.png",
			1 => "navbar_statistics.png",
			2 => "navbar_savings.png",
			_ => "navbar_settings.png",
		};
	}
}
