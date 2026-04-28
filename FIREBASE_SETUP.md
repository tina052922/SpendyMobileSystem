## Firebase Authentication setup (Spendy .NET MAUI)

Spendy is designed to work **without Firebase**. Google sign-in only appears when you configure it.

### Fix: “Access blocked / Error 400: invalid_request” (Google OAuth)
This usually means your **OAuth Web client** is missing the custom redirect URI, or your **OAuth consent screen** is still in **Testing** without your account added.

1. Open **Google Cloud Console** (same Google Cloud project linked to Firebase) → **APIs & Services** → **Credentials**.
2. Open the **OAuth 2.0 Client ID** of type **Web application** (this is the `client_type: 3` client id inside `google-services.json`).
3. Under **Authorized redirect URIs**, add exactly (Google’s **Web** client rejects custom schemes like `spendy://`):
   - `https://127.0.0.1`
   - (Optional) also add `https://127.0.0.1/` if Google shows redirects with a trailing slash only.
4. Save.
5. Go to **OAuth consent screen**:
   - If publishing status is **Testing**, add your Gmail under **Test users** (example: `macalisangchristina@gmail.com`).
6. If you still get blocked, try again **without VPN** (Google may flag unusual network paths).

### Android: browser shows `127.0.0.1` / VPN “page unavailable” after picking a Google account
That happens when the **web OAuth redirect** (`https://127.0.0.1`) opens in the **system browser** instead of returning to the app, or when a **VPN** intercepts traffic to loopback.

**Current Spendy behavior (Android):** Google sign-in uses **Google Play Services** (native account picker). It does **not** rely on loading `https://127.0.0.1` in a browser, so VPN/browser loopback issues should no longer block sign-in.

You still need the **Web** OAuth client id in `google-services.json` (`client_type: 3`) because native sign-in calls `RequestIdToken` with that id.

**Required:** In Firebase Console → Project settings → your **Android** app, add your app’s **SHA-1** (debug and/or release keystore). If SHA-1 is missing, Google Play Services returns a **developer error** (error code 10) when signing in. Then download an updated `google-services.json`.

### Automatic Firebase values (no manual paste on Android)
Spendy bundles `Spendy/Platforms/Android/google-services.json` and automatically reads:
- `project_info.project_id` → Firestore project id
- Web OAuth `client_id` (`client_type: 3`) → Google sign-in client id

### What’s already implemented in code
- **Forgot Password**: secure reset codes (random token, SHA-256 hashed in DB, 1 hour expiry, rate limit) via `AuthService`.
- **Google Sign-In button**: shown/hidden based on `IGoogleAuthService.IsConfigured`.
- **Google Sign-In flow** (`Spendy/Services/GoogleOAuthAuthService.cs`):
  - **Android:** native **Google Play Services** sign-in (no `127.0.0.1` browser redirect).
  - **Windows / iOS / Mac Catalyst:** OAuth **WebAuthenticator** + PKCE with redirect `https://127.0.0.1`.
  - This does **not** require Firebase to work.
  - It maps the Google email to a local SQLite user (existing user logs in; otherwise it creates a local user).

### 1) Create OAuth client in Google Cloud Console
1. Go to Google Cloud Console → **APIs & Services** → **Credentials**.
2. Create an **OAuth client ID**:
   - For Android: type **Android**
   - For iOS: type **iOS**
3. Make sure you enable the Google Identity / OAuth access as required.

### 2) Configure the callback scheme (Windows / iOS / Mac Catalyst)
For platforms that use **WebAuthenticator**, register this redirect on the **Web application** OAuth client:

- **Redirect URI**: `https://127.0.0.1` (HTTPS loopback; required by Google Cloud for **Web application** clients)

Related code:
- **Android (legacy / unused for sign-in now):** `Spendy/Platforms/Android/WebAuthenticatorCallbackActivity.cs` (`https` + host `127.0.0.1`) — kept for compatibility; Android sign-in uses Play Services instead.
- **Spendy**: `Spendy/Services/GoogleOAuthAuthService.cs` (loopback URI for non-Android targets).

### 3) Google Client ID + Firebase Project ID in the app (Android)
On Android builds, Spendy copies these values from the bundled `google-services.json` into app preferences at startup.

If the Google button is still hidden, it means the bundled config file is missing from the build you installed.

### 4) (Optional) Use Firebase instead
If you want Firebase Authentication (Email/Password + Google) specifically:
1. Create a Firebase project.
2. Enable **Authentication** → **Email/Password** and **Google** providers.
3. Add your apps:
   - Android package name should match `ApplicationId` in `Spendy/Spendy.csproj` (currently `com.companyname.spendy`)
   - iOS bundle id should match your iOS bundle id
4. Download config files:
   - Android: `google-services.json`
   - iOS: `GoogleService-Info.plist`
5. Add the Firebase SDK / NuGet packages you choose for MAUI and follow their platform setup.

Spendy will continue to work even if Firebase is not configured.

