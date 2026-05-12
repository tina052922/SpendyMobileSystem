# Spendy – Budget Tracker  
## Feature & functionality test checklist

Use this list for manual QA before demos, thesis defense, or releases. Check each item on **Windows** and **Android** (or your target platforms).  

---

### 1. Launch & database

| # | Test | Expected |
|---|------|----------|
| 1.1 | Cold start app | Splash appears; navigation reaches Get Started / Sign In flow without crash |
| 1.2 | Second launch | Data from previous session still present (same device) |
| 1.3 | Debug / logs | Startup logs show SQLite path, file exists flag, and size for `spendy.db` |

---

### 2. Authentication

| # | Test | Expected |
|---|------|----------|
| 2.1 | Sign up new account | Success; lands in main app; user row in local DB |
| 2.2 | Sign in correct credentials | Success; session restored |
| 2.3 | Sign in wrong password | Clear error (e.g. incorrect password / no account on device) |
| 2.4 | Sign in unknown email | Message indicates no account on this device (offline model) |
| 2.5 | Sign out | Returns to sign-in stack; session cleared |
| 2.6 | Forgot / offline password reset | Flow completes per implemented screens |

---

### 3. Dashboard & calendar

| # | Test | Expected |
|---|------|----------|
| 3.1 | View summary / greeting | Reflects signed-in user |
| 3.2 | Open monthly / day views (if applicable) | Correct dates and totals |
| 3.3 | Toggle expense/income views | UI updates without crash |

---

### 4. Transactions (add & history)

| # | Test | Expected |
|---|------|----------|
| 4.1 | Add income | Amount appears in balance / history |
| 4.2 | Add expense (category) | Deducts from available balance; appears in list |
| 4.3 | Expense before any income | Blocked or warned per app rules |
| 4.4 | Add expense → **Savings goal** category | Action sheet lists goals; allocation saves to chosen goal |
| 4.5 | Mandatory savings on large income | Allocation flow opens and completes when applicable |

---

### 5. Statistics

| # | Test | Expected |
|---|------|----------|
| 5.1 | View chart / totals | Matches transactions for selected period |
| 5.2 | Calendar / month control | Changing month refreshes chart and category breakdown |

---

### 6. Savings goals & plans

| # | Test | Expected |
|---|------|----------|
| 6.1 | Add savings plan | Name, target amount, start/end dates; duration text updates |
| 6.2 | Date pickers | Start / target date overlays open, dark theme, Done closes overlay |
| 6.3 | Edit savings plan | Changes persist |
| 6.4 | Save money from plan detail | Amount updates goal balance (Draw/Save plan flow) |
| 6.5 | End / restore ended goals | Behaves as implemented |

---

### 7. Profile & settings

| # | Test | Expected |
|---|------|----------|
| 7.1 | Edit profile | Name/email/etc. save and reload |
| 7.2 | Currency (PHP/USD) | Amounts and symbols update |
| 7.3 | Update password | Current/new/confirm fields visible; validation works |
| 7.4 | Copy database path | Path copied; alert explains per-device storage |
| 7.5 | Share database backup | `spendy.db` share sheet opens when file exists |

---

### 8. Notifications

| # | Test | Expected |
|---|------|----------|
| 8.1 | Open notifications screen | Loads without crash; content consistent with goals/deadlines if seeded |

---

### 9. Regression & stability

| # | Test | Expected |
|---|------|----------|
| 9.1 | Rotate / resize window (desktop) | No overlapping broken layout on Settings cards |
| 9.2 | Back navigation | Returns without losing unsaved critical state incorrectly |
| 9.3 | Airplane mode | Core features still work (offline-first) |

---

### 10. Cross-platform note (evaluation)

| # | Test | Expected |
|---|------|----------|
| 10.1 | Same login on **two devices** without copying DB | **Not** expected: each device has its own `spendy.db`; document for thesis |
| 10.2 | Copy backup to second device | Manual transfer only; verify after documented procedure |

---

**Tester:** _______________  
**Date:** _______________  
**Build / commit:** _______________  
