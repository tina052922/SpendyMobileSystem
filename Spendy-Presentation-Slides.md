# Spendy – Budget Tracker  
## PowerPoint slide content (copy each block into one slide)

Use **Design ideas** or your school template. Add **screenshots** from Windows/Android where noted.

---

### Slide 1 — Title  
**Spendy – Budget Tracker**  
Offline-first personal finance mobile application  
.NET MAUI • SQLite • MVVM  
*[Your name(s)] • [Course / term] • [Year]*

---

### Slide 2 — Problem & motivation  
- Many finance apps need **constant internet** or **cloud accounts**  
- Users want **privacy** and **simplicity** for daily income, expenses, and savings  
- **Goal:** one app that works **fully offline** with **local data only**

---

### Slide 3 — Project objectives  
- Track **income** and **expenses** with categories  
- Show **available balance** and **daily / monthly** views  
- Support **savings goals** (save, withdraw, progress)  
- **No** mandatory cloud sync — data stays on the device  

---

### Slide 4 — Tech stack  
| Layer | Technology |
|--------|------------|
| UI | .NET MAUI (cross-platform) |
| Pattern | MVVM (CommunityToolkit.Mvvm) |
| Data | SQLite + Entity Framework Core |
| Auth | Local accounts, hashed passwords |

---

### Slide 5 — Architecture (high level)  
- **Views / XAML** → bind to **ViewModels**  
- **Services** (`SpendyDataService`, auth, currency, profile photo)  
- **SQLite** single database file (`spendy.db`) per install  
- **Startup:** splash initializes DB before main navigation  

*[Optional: simple diagram — UI → VM → Service → EF/SQLite]*

---

### Slide 6 — Navigation & structure  
- **Pre-login:** Splash → Get Started → Sign In / Sign Up / Forgot password  
- **After login:** **Shell** with **four tabs:** Home, Statistics, Savings, Settings  
- Secondary pages pushed on stack (add transaction, history, plan detail, profile)  

*[Screenshot: main tabs or splash → sign-in]*

---

### Slide 7 — Core feature: Dashboard (Home)  
- **Available balance** and **expense vs income** mode  
- **Today’s transactions** with categories and amounts  
- **Calendar overlay:** month grid → tap a day → **category breakdown**  
- **History:** all-time list filtered by current mode  

*[Screenshot: dashboard + calendar if possible]*

---

### Slide 8 — Transactions & business rules  
- Add income/expense: **category, amount, date, notes**  
- **Rule:** expenses blocked until **first income** is recorded  
- **Large income:** mandatory savings allocation (threshold / %)  
- **Savings goal** as expense category → pick goal and deposit  

---

### Slide 9 — Statistics  
- Monthly **chart** and **category breakdown**  
- Toggle **expense / income**  
- **Month/year picker** for any period  

*[Screenshot: Statistics tab]*

---

### Slide 10 — Savings goals  
- Create/edit plans: **name, target, start & target dates**  
- **Plan detail:** Save Money / Withdraw (updates goal + ledger)  
- **Ended plans** list and restore flow  
- Balance and transactions stay **consistent** across dashboard and goals  

*[Screenshot: Savings tab + plan detail]*

---

### Slide 11 — Security & privacy  
- Passwords stored as **hashes** (not plaintext)  
- **Offline** password reset flow (local tokens)  
- **Remember me** optional — session stored on device  
- **No** cloud sync in scope — each device has its own DB  

---

### Slide 12 — UI / UX highlights  
- Navy **brand palette**, cards, custom **tab bar**  
- Consistent headers, notifications bell, currency **PHP / USD**  
- Debounced refresh for smoother performance  
- In-app **notifications** (deadlines, balance hints — not push)  

*[Screenshot collage: 2–3 screens]*

---

### Slide 13 — Testing & quality  
- Manual checklist in **`TESTING.md`** (platform smoke tests)  
- **Offline** verification (airplane mode)  
- Build targets: **Windows** / **Android** as configured  

---

### Slide 14 — Limitations & future work  
- **No multi-device sync** (by design for this version)  
- Optional future: backup/export, cloud sync, push notifications  
- Further **performance** tuning on low-end devices  

---

### Slide 15 — Conclusion  
Spendy delivers a **complete offline** budgeting experience: **ledger + calendar + statistics + savings goals** with **local control** and **clear UX**.  
**Thank you / Questions**

---

### Speaker notes (optional — Notes pane in PowerPoint)  
- **Demo path (2 min):** Sign in → Home → add income → add expense → open calendar → Savings → open one goal → Save Money → Settings → currency.  
- **Backup line:** “All financial data lives in SQLite on this device; reinstall or new device starts fresh unless the user copies `spendy.db` manually.”
