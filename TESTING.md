# Spendy — Testing Guide

This document describes how to build Spendy, verify core behavior, and run structured tests for the **.NET MAUI** app (`Spendy/Spendy.csproj`). Use it for QA, release checks, and regression testing after changes.

---

## 1. Prerequisites

| Requirement | Notes |
|-------------|--------|
| **.NET SDK** | Matches the project (e.g. .NET 10 per `Spendy.csproj` `TargetFrameworks`). Run `dotnet --version`. |
| **Workload** | MAUI: `dotnet workload install maui` (or install via Visual Studio Installer). |
| **IDE** (optional) | Visual Studio 2022+ with MAUI, or VS Code + CLI. |
| **Device / emulator** | Android emulator or device, Windows (WinUI target), iOS/Mac only on macOS with Xcode. |

**Build (example — Windows):**

```bash
dotnet build Spendy/Spendy.csproj -f net10.0-windows10.0.19041.0
```

**Build Android (example):**

```bash
dotnet build Spendy/Spendy.csproj -f net10.0-android
```

Adjust target framework names if your `Spendy.csproj` differs.

---

## 2. First launch & database

1. Install and open the app (debug or release).
2. **Splash** runs, initializes SQLite (`spendy.db` under app data), seeds **categories** if empty, and applies any **schema migrations** (e.g. user-scoped columns).
3. If a **saved session** exists (signed-in user id in preferences), the app may go straight to the **main shell**; otherwise you see **Get Started → Sign In**.

**Checks:**

- [ ] App starts without crash.
- [ ] Cold start: unauthenticated flow reaches Sign In / Sign Up.
- [ ] No unhandled exception dialogs during DB init.

---

## 3. Authentication

### 3.1 Sign up (register)

**Path:** Sign In → Sign up (or equivalent navigation).

**Password rules (enforced):**

- Minimum **8** characters  
- At least **one uppercase** letter  
- At least **one digit**  
- At least **one special character**  
- **Confirm password** must match  

**Checks:**

- [ ] Empty or weak password shows a clear error (alert).
- [ ] Mismatched confirm password is rejected.
- [ ] Terms checkbox required before successful sign-up (if implemented).
- [ ] Successful sign-up creates the user with **hashed** password (not plain text) and navigates to the **main app** (shell).
- [ ] **Birthday** `DatePicker` cannot pick a date **after today** (`MaximumDate`).

### 3.2 Sign in

**Checks:**

- [ ] Wrong email/password shows an error; no navigation to shell.
- [ ] Correct credentials reach the main shell.
- [ ] **Legacy accounts** without a stored hash: first successful login with a **policy-valid** password can establish the hash (migration path).

### 3.3 Session & logout

**Checks:**

- [ ] **Log out** (e.g. Settings) clears session and returns to the **Sign In** stack.
- [ ] After logout, **profile photo** resets appropriately (no other user’s image).
- [ ] **Remember me** (if only UI today): verify behavior matches product spec when implemented.

### 3.4 Optional: session restore

After a successful login, kill the app and relaunch.

- [ ] If session persistence is enabled, user may return to shell without signing in again (verify against current implementation).

---

## 4. Per-user data isolation (critical)

Use **two distinct test accounts**, e.g.:

- `user-a@example.com`
- `user-b@example.com`

**Procedure:**

1. Register and sign in as **User A**. Add a **transaction**, a **savings goal**, and set **profile** fields (name, photo if applicable).
2. **Log out**.
3. Register and sign in as **User B**.

**Checks:**

- [ ] **Dashboard / Statistics / Transactions** show **no** data from User A (empty or only B’s data).
- [ ] **Savings** lists only User B’s goals.
- [ ] **Profile** shows only User B’s profile; switching back to User A after logout/login shows only A’s data.
- [ ] No mixing of balances, goals, or profile between users.

---

## 5. Profile

**Checks:**

- [ ] Load profile: fields match the **current** user in SQLite.
- [ ] Save profile: updates persist after navigating away and app restart (same user).
- [ ] Email change: if allowed, duplicate email used by another account should fail with a clear message.
- [ ] Profile image (if used): path stored per user; correct image after user switch.

---

## 6. Settings

**Checks:**

- [ ] **Currency** selection updates formatting where applicable.
- [ ] **Update Password** (expand section):  
  - [ ] Current / new / confirm fields are readable (contrast).  
  - [ ] Same password policy as registration.  
  - [ ] Success message after a valid change.  
  - [ ] Wrong current password rejected.

---

## 7. Dashboard & transactions

**Checks:**

- [ ] **Expense / Income** toggle loads the correct summary for **today**.
- [ ] **Add Transaction**: amount, category, date, save — appears in dashboard list and affects **balance**.
- [ ] **Mandatory savings** flow (if used from income): allocates to selected goal per app rules.
- [ ] Data scoped to **logged-in user** only (see §4).

---

## 8. Statistics

**Checks:**

- [ ] Month chart and category breakdown match transactions for the selected month and mode (expense/income).
- [ ] Empty month shows sensible empty state (no crash).

---

## 9. Savings

**Checks:**

- [ ] **Add savings plan**: name, amount, dates — saves and appears in list.
- [ ] **Edit** / **detail** / **ended** flows open correct goal **for current user**.
- [ ] Movements (save/withdraw) update goal balance and related notifications if implemented.

---

## 10. Notifications (in-app)

**Checks:**

- [ ] Items derive from **current user’s** goals/balance rules (e.g. upcoming deadlines).

---

## 11. UI / UX smoke (auth & settings)

**Checks:**

- [ ] **Sign In / Sign Up**: labels, placeholders, and buttons readable on white card; navy header readable.
- [ ] **Add / Edit savings plan** screens: layout consistent with **Add Transaction** dark sheet styling (no broken gradients).
- [ ] **Settings** → Update Password: dark inset section readable.

---

## 12. Regression checklist (short)

Run before a release:

| Area | Pass |
|------|------|
| Build (target platform) | ☐ |
| Sign up + sign in + logout | ☐ |
| Two-user isolation | ☐ |
| Profile save / load | ☐ |
| Password change in Settings | ☐ |
| Add transaction + balance | ☐ |
| Savings CRUD | ☐ |
| Statistics month view | ☐ |

---

## 13. Troubleshooting

| Symptom | Things to check |
|---------|------------------|
| Old user’s data visible for new user | Log out fully; confirm `IUserSession` / DB `UserId` on rows; reinstall only as last resort (clears local DB). |
| Login fails after DB upgrade | Email normalization (lowercase); legacy account may need first-time password set per migration rules. |
| Build fails | `dotnet workload install maui`; correct TFM; Android SDK / JDK for Android. |
| UI not updating | `DataChanged` / navigation after auth; binding context on pages. |

---

## 14. Automated tests (future)

The solution may not include a unit test project yet. Recommended additions:

- **Unit tests** for `PasswordPolicy`, password hashing, and email normalization.
- **Integration tests** for `AuthService` + in-memory SQLite (EF Core).

Track these as separate tasks when introducing a test project.

---

## Document history

- Created for manual QA and GitHub documentation of the Spendy MAUI application.
