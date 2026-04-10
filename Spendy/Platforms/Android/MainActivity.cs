using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Spendy;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);

		// Remove default purple status bar tint; keep consistent Spendy header color.
		var c = Android.Graphics.Color.ParseColor("#01143D");
		Window?.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
		Window?.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
		Window?.SetStatusBarColor(c);
		Window?.SetNavigationBarColor(c);
	}
}
