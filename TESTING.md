# Spendy – Budget Tracker  
## Feature & functionality test checklist

Use this list for manual QA before demos, thesis defense, or **final project evaluation**. Run checks on each target platform you claim (**Windows** and/or **Android**).

---

### 1. Launch & database

| # | Test | Expected |
|---|------|----------|
| 1.1 | Cold start | **Splash** runs; SQLite initializer completes (`EnsureCreated` / guarded migrations); no crash before **Get Started** |
| 1.2 | Continue flow | **Get Started** → **Sign In** (stack navigation); user must sign in explicitly (no auto-skip to main shell) |
| 1.3 | Second launch | Same device: existing data in `spendy.db` still visible after successful login |
| 1.4 | Debug diagnostics | When debugging: Output window may show `[Spendy.Database]` and SQLite path (see Settings → developer section on Debug builds) |

---

### 2. Authentication

| # | Test | Expected |
|---|------|----------|
| 2.1 | Sign up | Valid data + terms → account created → transitions to **main shell** (four tabs) |
| 2.2 | Sign in (correct) | Success toast → **AppShell** / **MainShellPage** with bottom tab bar |
| 2.3 | Sign in (wrong password / unknown email) | Clear error; offline model explains account is device-local |
| 2.4 | Remember me | When enabled: user id persisted + email may prefill next time; user still completes Splash → Get Started → Sign In |
| 2.5 | Logout (Settings) | Session cleared → full-screen **Sign In** stack (no tab bar) |
| 2.6 | Forgot password / offline reset | Request → confirm flow updates local password hash per implemented pages |

---

### 3. Home (Dashboard) & calendar

| # | Test | Expected |
|---|------|----------|
| 3.1 | Greeting / balance | Uses profile display name when set; available balance formatted with selected currency |
| 3.2 | Expense ↔ Income toggle | Summary, list, and calendar mode switch correctly |
| 3.3 | Transaction list (today) | Items match DB for selected kind |
| 3.4 | Open monthly calendar overlay | Month grid loads; prev/next month navigation works |
| 3.5 | Tap a day | Day detail shows total + category breakdown; **Back** returns to grid; closing overlay returns to dashboard |
| 3.6 | History button | Pushes **Transaction history** filtered by current mode (expense vs income), newest first |

---

### 4. Transactions (add & rules)

| # | Test | Expected |
|---|------|----------|
| 4.1 | Add income | Balance increases; appears in list/history |
| 4.2 | Add expense | Balance decreases; blocked/warned if **no income exists yet** |
| 4.3 | Expense → **Savings goal** category | Action sheet lists active goals; deposit respects available balance; goal **CurrentAmount** updates |
| 4.4 | Income ≥ **₱20,000** | **Mandatory savings** modal opens (2% rule); completing allocation saves income + mandatory movement |

---

### 5. Statistics tab

| # | Test | Expected |
|---|------|----------|
| 5.1 | Toggle expense/income | Chart and categories reflect selected kind |
| 5.2 | Month/year picker | Changing period refreshes chart and category breakdown |
| 5.3 | Totals vs DB | Spot-check sums against transactions for that month |

---

### 6. Savings tab

| # | Test | Expected |
|---|------|----------|
| 6.1 | Add savings plan | Name, target, **start date**, **target date** persist |
| 6.2 | Edit plan | Changes saved |
| 6.3 | Plan detail | Progress line correct; **save** / **withdraw** update balance when income prerequisite met |
| 6.4 | Ended link | **Ended savings** shows ended goals as implemented |
| 6.5 | Restore ended goal | **Show restore** behavior matches implementation |

---

### 7. Profile & settings

| # | Test | Expected |
|---|------|----------|
| 7.1 | Profile | Name, email, phone, photo path sync; edits persist in SQLite |
| 7.2 | Currency **PHP / USD** | Symbols and formatting update across Home, Statistics, Savings |
| 7.3 | Change password | Strength indicator; validation; success message |
| 7.4 | Copy database path (**Debug** only) | Path copied; alert explains Windows vs Android access to `spendy.db` |

---

### 8. Notifications

| # | Test | Expected |
|---|------|----------|
| 8.1 | Open from bell (Home / Savings / Settings) | Page loads; badge/count reflects derived alerts |
| 8.2 | Goal deadline (≤14 days) | Reminder appears when an active goal’s target date is soon |
| 8.3 | Balance alerts | Negative balance / low balance (below ₱500) messages when conditions met |
| 8.4 | Overspending | When monthly expenses exceed monthly income, informational alert appears |

---

### 9. Regression & stability

| # | Test | Expected |
|---|------|----------|
| 9.1 | Resize / rotate (desktop) | Main layouts usable; tab bar and overlays not clipped critically |
| 9.2 | Back / Pop | Pushed pages dismiss correctly; modal mandatory flow closes without corrupting stack |
| 9.3 | Airplane mode | Sign-in, CRUD, and reporting work (offline-first) |
| 9.4 | Rebuild while app running | Close **Spendy.exe** before build to avoid MSB3026 file locks |

---

### 10. Cross-platform / thesis notes

| # | Test | Expected |
|---|------|----------|
| 10.1 | Same login on two devices without copying DB | **Not** expected: each device has its own SQLite file |
| 10.2 | Manual DB transfer | Only via adb / file explorer / documented export—verify integrity after copy |

---

**Tester:** _______________  
**Date:** _______________  
**Build / commit:** _______________  

---

## Appendix — Final Project Application Evaluation Form (draft)

Copy the following blocks into your Word evaluation form and adjust wording if your adviser requires a shorter version.

### Application Name  
**Spendy – Budget Tracker**

### Project Overview (for context)  
Spendy is an offline-first personal finance mobile application built with **.NET MAUI**. It enables users to record income and expenses, inspect daily and monthly activity through an interactive calendar and statistics views, and manage **savings goals** with deposits and withdrawals—all persisted locally via **SQLite** and **Entity Framework Core**, without reliance on cloud authentication or synchronization services.

---

### 1. List of Modules  

| Module | Description |
|--------|-------------|
| **Splash & onboarding** | Splash screen with database initialization; **Get Started** landing; transitions into authentication stack |
| **Authentication** | **Sign In**, **Sign Up**, **Forgot password** (offline reset flow) |
| **Main shell (four-tab UI)** | Custom tab bar hosting **Home**, **Statistics**, **Savings**, and **Settings** |
| **Home (Dashboard)** | Daily summary, expense/income mode, transaction list, calendar overlay with per-day breakdown, link to all-time history |
| **Add transaction** | Category selection, amount, notes, day-of-month; mandatory savings modal for large incomes; savings-goal allocation expense |
| **Transaction history** | Chronological list filtered by transaction kind |
| **Statistics** | Monthly chart and category breakdown with month/year picker |
| **Savings** | Savings plans list, add/edit plan with date range, plan detail (movements, progress), ended goals view |
| **Notifications** | In-app alerts derived from goal deadlines and balance heuristics |
| **Profile** | User demographic and profile photo integration |
| **Settings** | Currency preference (PHP/USD), password change, logout; developer utilities on Debug builds |

---

### 2. List of Functions  

**User & security (CRUD / operations)**  
- **Create / Read / Update** local user accounts (registration, profile editing)  
- **Authenticate** with hashed passwords; **session** persistence controlled by “Remember me”  
- **Change password** with policy validation; **offline password reset** using local token storage  

**Transactions**  
- **Create** income and expense records with category, amount, date, and notes  
- **Read** transactions for dashboard, calendar day breakdown, history, and statistics aggregates  
- **Enforce business rule:** expenses disabled until first income exists  
- **Create** compound operation for **mandatory savings** on qualifying income (income + forced allocation to a goal)  
- **Create** goal deposits via dedicated expense category or savings detail screens  

**Categories**  
- **Read** seeded and user-facing categories; scope separation (income vs expense)  

**Savings goals**  
- **Create / Read / Update** saving plans (name, amounts, start/target dates)  
- **Update** goal balance through save/withdraw movements  
- **Update** ended/restored state per application logic  

**Reporting & UX**  
- **Read** monthly statistics and chart series; **read** day-level breakdown for calendar  
- **Read** computed **available balance** reflecting ledger rules  

**Settings & localization**  
- **Update** currency preference affecting formatting  
- **Delete** session on logout (local only)  

---

### 3. List of Unique Features  

- **Offline-first architecture:** All core features operate without network connectivity; data remains on-device in SQLite.  
- **Hybrid navigation model:** Pre-auth **NavigationPage** stack for onboarding and login; post-auth **Shell** with a **custom four-tab bar** and pushed modal/pages for depth—demonstrates intentional UX separation between guest and signed-in states.  
- **Calendar-based transaction viewer:** Month grid with per-day totals and drill-down to category breakdown for the selected income/expense mode—focused on **auditability** rather than generic charts only.  
- **Mandatory savings policy:** Configurable threshold income triggers a **modal allocation** workflow (e.g., 2% rule), integrating budgeting discipline with the savings subsystem.  
- **Unified savings ledger:** Expenses routed through a **“Savings goal”** category tie directly to goal balances, keeping **available balance**, transactions, and goals consistent.  
- **In-app intelligence layer:** Notifications synthesize **deadline proximity**, **balance risk**, and **monthly overspending** without push infrastructure—appropriate for an offline academic prototype.  
- **MVVM discipline:** ViewModels encapsulate state and commands; services abstract data access—supporting testability and maintainability expected in capstone evaluation.  
- **Security posture:** Passwords stored as **hashes**; offline reset avoids exposing plaintext secrets; session cleared on logout.  
- **Visual design:** Cohesive **navy / accent** palette, rounded cards, and custom navigation artwork supporting a polished **dark-themed** financial UI suitable for demonstration and defense panels.  

---

### 4. Additional Notes / Comments  

Spendy intentionally scopes **synchronization and multi-device merge** out of scope: each installation owns its database file, which simplifies consistency guarantees and aligns with the **offline-first** thesis. Performance considerations include **debounced refresh** after data changes and **database initialization on splash** to avoid race conditions during first login. For evaluation, recommend exercising the checklist in **TESTING.md** on each declared platform and capturing screenshots of the four main tabs, calendar drill-down, mandatory savings modal, and savings plan detail to evidence functional completeness.
