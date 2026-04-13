using Microsoft.EntityFrameworkCore;
using Spendy.Data;
using Spendy.Data.Entities;
using Spendy.Models;

namespace Spendy.Services;

public sealed class SpendyDataService(
	IDbContextFactory<SpendyDbContext> dbFactory,
	ICurrencyService currency,
	IUserSession session) : ISpendyDataService
{
	public event EventHandler? DataChanged;

	public void NotifyDataChanged() =>
		DataChanged?.Invoke(this, EventArgs.Empty);

	int? CurrentUserIdOrNull() => session.CurrentUserId;

	public async Task<decimal> GetBalanceAsync(CancellationToken cancellationToken = default)
	{
		var uid = CurrentUserIdOrNull();
		if (uid is null)
			return 0;

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var income = await db.Transactions.AsNoTracking()
			.Where(t => t.UserId == uid && t.Type == TransactionKind.Income)
			.SumAsync(t => (decimal?)t.Amount, cancellationToken) ?? 0;
		var expense = await db.Transactions.AsNoTracking()
			.Where(t => t.UserId == uid && t.Type == TransactionKind.Expense)
			.SumAsync(t => (decimal?)t.Amount, cancellationToken) ?? 0;
		return income - expense;
	}

	public async Task<bool> HasAnyIncomeAsync(CancellationToken cancellationToken = default)
	{
		var uid = CurrentUserIdOrNull();
		if (uid is null)
			return false;

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		return await db.Transactions.AsNoTracking()
			.AnyAsync(t => t.UserId == uid && t.Type == TransactionKind.Income, cancellationToken);
	}

	public async Task<UserEntity?> GetCurrentUserAsync(CancellationToken cancellationToken = default)
	{
		var uid = CurrentUserIdOrNull();
		if (uid is null)
			return null;

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		return await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == uid, cancellationToken);
	}

	public async Task<string?> GetUserDisplayNameAsync(CancellationToken cancellationToken = default)
	{
		var u = await GetCurrentUserAsync(cancellationToken);
		return string.IsNullOrWhiteSpace(u?.Name) ? null : u.Name.Trim();
	}

	public async Task UpsertUserAsync(UserEntity user, CancellationToken cancellationToken = default)
	{
		var uid = CurrentUserIdOrNull()
			?? throw new InvalidOperationException("Not signed in.");

		var email = (user.Email ?? string.Empty).Trim().ToLowerInvariant();
		if (string.IsNullOrWhiteSpace(email) || !email.Contains('@', StringComparison.Ordinal))
			throw new InvalidOperationException("Valid email is required.");

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

		if (await db.Users.AnyAsync(
			    u => u.Id != uid && u.Email == email,
			    cancellationToken))
			throw new InvalidOperationException("This email is already used by another account.");

		var existing = await db.Users.FirstOrDefaultAsync(u => u.Id == uid, cancellationToken)
			?? throw new InvalidOperationException("User not found.");

		existing.Name = (user.Name ?? string.Empty).Trim();
		existing.Email = email;
		existing.Phone = (user.Phone ?? string.Empty).Trim();
		existing.Birthday = user.Birthday ?? string.Empty;
		existing.Gender = user.Gender ?? string.Empty;
		existing.Address = (user.Address ?? string.Empty).Trim();
		existing.Handle = string.IsNullOrWhiteSpace(user.Handle) ? null : user.Handle.Trim();
		existing.ProfilePhotoPath = string.IsNullOrWhiteSpace(user.ProfilePhotoPath)
			? null
			: user.ProfilePhotoPath.Trim();

		await db.SaveChangesAsync(cancellationToken);
		DataChanged?.Invoke(this, EventArgs.Empty);
	}

	public async Task<DashboardData> GetDashboardAsync(DateTime day, TransactionKind kind, CancellationToken cancellationToken = default)
	{
		var uid = CurrentUserIdOrNull();
		var start = day.Date;
		var end = start.AddDays(1);

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		List<TransactionEntity> rows;
		if (uid is null)
			rows = [];
		else
			rows = await db.Transactions.AsNoTracking()
				.Include(t => t.Category)
				.Where(t => t.UserId == uid && t.Date >= start && t.Date < end && t.Type == kind)
				.OrderBy(t => t.Date)
				.ToListAsync(cancellationToken);

		decimal total = rows.Sum(t => t.Amount);
		var summaryLabel = kind == TransactionKind.Expense ? "Total Expenditure" : "Total Income";
		var summaryAmount = kind == TransactionKind.Expense
			? currency.Format(total, decimals: 0)
			: currency.Format(total, decimals: 2);
		var summaryColor = kind == TransactionKind.Expense
			? Color.FromArgb("#FF0000")
			: Color.FromArgb("#00D4A5");

		var items = rows.Select(t => new TransactionItem
		{
			Category = t.Category?.Name ?? "?",
			Icon = t.Category?.Icon ?? "•",
			CurrencySymbol = currency.Symbol,
			Amount = kind == TransactionKind.Expense ? -t.Amount : t.Amount,
			Time = t.Date.ToString("HH:mm", currency.Culture)
		}).ToList();

		var dateLabel = start.ToString("ddd, d MMMM", currency.Culture);

		return new DashboardData(dateLabel, summaryLabel, summaryAmount, summaryColor, items);
	}

	public async Task<StatisticsData> GetStatisticsAsync(int year, int month, TransactionKind kind, CancellationToken cancellationToken = default)
	{
		var uid = CurrentUserIdOrNull();
		var start = new DateTime(year, month, 1);
		var end = start.AddMonths(1);

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

		var monthTx = uid is null
			? []
			: await db.Transactions.AsNoTracking()
				.Include(t => t.Category)
				.Where(t => t.UserId == uid && t.Date >= start && t.Date < end && t.Type == kind)
				.ToListAsync(cancellationToken);

		var byDay = monthTx
			.GroupBy(t => t.Date.Day)
			.ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

		var daysInMonth = DateTime.DaysInMonth(year, month);
		var points = new List<ChartPoint>();
		for (var d = 1; d <= daysInMonth; d++)
		{
			var amt = byDay.TryGetValue(d, out var v) ? v : 0;
			points.Add(new ChartPoint { Day = d, Amount = amt });
		}

		var max = points.Count == 0 ? 1 : points.Max(p => p.Amount);
		if (max <= 0)
			max = 1;

		var grouped = monthTx
			.GroupBy(t => new { t.CategoryId, Name = t.Category!.Name, Icon = t.Category.Icon })
			.OrderByDescending(g => g.Sum(x => x.Amount))
			.ToList();

		var byCategory = new List<CategoryStat>();
		for (var i = 0; i < grouped.Count; i++)
		{
			var g = grouped[i];
			byCategory.Add(new CategoryStat
			{
				Name = g.Key.Name,
				Icon = g.Key.Icon,
				Amount = g.Sum(x => x.Amount),
				CurrencySymbol = currency.Symbol,
				AmountColor = kind == TransactionKind.Expense
					? Color.FromArgb("#01143D")
					: Color.FromArgb("#00D4A5"),
				IsTopCategory = i == 0 && g.Sum(x => x.Amount) > 0
			});
		}

		var barColor = kind == TransactionKind.Expense
			? Color.FromArgb("#022268")
			: Color.FromArgb("#00D4A5");

		var title = start.ToString("MMMM yyyy", currency.Culture);
		return new StatisticsData(title, points, byCategory, barColor);
	}

	public async Task<IReadOnlyList<SavingPlan>> GetSavingPlansAsync(bool endedOnly, CancellationToken cancellationToken = default)
	{
		var uid = CurrentUserIdOrNull();
		if (uid is null)
			return [];

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var goals = await db.SavingGoals.AsNoTracking()
			.Where(g => g.UserId == uid && g.IsEnded == endedOnly)
			.OrderBy(g => g.TargetDate)
			.ToListAsync(cancellationToken);

		return goals.Select(MapPlan).ToList();
	}

	public async Task<SavingPlan?> GetSavingPlanAsync(int id, CancellationToken cancellationToken = default)
	{
		var uid = CurrentUserIdOrNull();
		if (uid is null)
			return null;

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var g = await db.SavingGoals.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid, cancellationToken);
		return g is null ? null : MapPlan(g);
	}

	public async Task<IReadOnlyList<SavingHistoryLine>> GetSavingHistoryAsync(int goalId, CancellationToken cancellationToken = default)
	{
		var uid = CurrentUserIdOrNull();
		if (uid is null)
			return [];

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var goalOk = await db.SavingGoals.AsNoTracking()
			.AnyAsync(g => g.Id == goalId && g.UserId == uid, cancellationToken);
		if (!goalOk)
			return [];

		var rows = await db.SavingTransactions.AsNoTracking()
			.Where(s => s.SavingGoalId == goalId)
			.OrderByDescending(s => s.Date)
			.ThenByDescending(s => s.Id)
			.ToListAsync(cancellationToken);

		var saveGreen = Color.FromArgb("#05B325");
		var withdrawRed = Color.FromArgb("#BB0000");

		return rows.Select(r => new SavingHistoryLine
		{
			DateText = r.Date.ToString("yyyy-MM-dd", currency.Culture),
			AmountText = r.Type == SavingMovement.Save
				? currency.Format(r.Amount, decimals: 0)
				: $"-{currency.Format(r.Amount, decimals: 0)}",
			AmountColor = r.Type == SavingMovement.Save ? saveGreen : withdrawRed
		}).ToList();
	}

	SavingPlan MapPlan(SavingGoalEntity g)
	{
		var finished = g.TargetAmount > 0 && g.CurrentAmount >= g.TargetAmount;
		return new SavingPlan
		{
			Id = g.Id,
			Name = g.Name,
			Current = g.CurrentAmount,
			Target = g.TargetAmount,
			TargetDate = g.TargetDate.ToString("MMM d, yyyy", currency.Culture),
			TargetDateValue = g.TargetDate.Date,
			IsEnded = g.IsEnded,
			IsFinished = finished,
			CurrencySymbol = currency.Symbol
		};
	}

	public async Task<IReadOnlyList<(int Id, string Icon, string Name)>> GetCategoriesForPickerAsync(TransactionKind kind, CancellationToken cancellationToken = default)
	{
		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var scope = kind == TransactionKind.Expense ? CategoryScope.Expense : CategoryScope.Income;
		var rows = await db.Categories.AsNoTracking()
			.Where(c => c.Scope == scope || c.Scope == CategoryScope.Both)
			.OrderBy(c => c.Id)
			.Select(c => new ValueTuple<int, string, string>(c.Id, c.Icon, c.Name))
			.ToListAsync(cancellationToken);
		return rows;
	}

	public async Task AddTransactionAsync(decimal amount, TransactionKind kind, int categoryId, DateTime date, string? notes, CancellationToken cancellationToken = default)
	{
		var uid = CurrentUserIdOrNull()
			?? throw new InvalidOperationException("Not signed in.");

		if (amount <= 0)
			throw new ArgumentOutOfRangeException(nameof(amount));

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		db.Transactions.Add(new TransactionEntity
		{
			UserId = uid,
			Amount = decimal.Round(amount, 2),
			Type = kind,
			CategoryId = categoryId,
			Date = date,
			Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim(),
			CreatedAt = DateTime.UtcNow
		});
		await db.SaveChangesAsync(cancellationToken);
		DataChanged?.Invoke(this, EventArgs.Empty);
	}

	public async Task AddIncomeWithMandatorySavingsAsync(
		decimal incomeAmount,
		int categoryId,
		DateTime date,
		string? notes,
		int savingGoalId,
		decimal mandatorySavingsAmount,
		CancellationToken cancellationToken = default)
	{
		var uid = CurrentUserIdOrNull()
			?? throw new InvalidOperationException("Not signed in.");

		if (incomeAmount <= 0)
			throw new ArgumentOutOfRangeException(nameof(incomeAmount));
		if (mandatorySavingsAmount <= 0)
			throw new ArgumentOutOfRangeException(nameof(mandatorySavingsAmount));

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

		var incomeRounded = decimal.Round(incomeAmount, 2);
		var dep = decimal.Round(mandatorySavingsAmount, 2);
		if (dep > incomeRounded)
			throw new ArgumentOutOfRangeException(
				nameof(mandatorySavingsAmount),
				"Mandatory savings cannot exceed the income amount.");

		db.Transactions.Add(new TransactionEntity
		{
			UserId = uid,
			Amount = incomeRounded,
			Type = TransactionKind.Income,
			CategoryId = categoryId,
			Date = date,
			Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim(),
			CreatedAt = DateTime.UtcNow
		});

		var g = await db.SavingGoals.FirstOrDefaultAsync(
			         x => x.Id == savingGoalId && x.UserId == uid, cancellationToken)
			?? throw new InvalidOperationException("Saving goal not found.");
		if (g.IsEnded)
			throw new InvalidOperationException("Cannot allocate to an ended goal.");

		var expenseCat = await GetOrCreateCategoryAsync(db, "Savings goal", "🎯", CategoryScope.Expense, cancellationToken);
		db.Transactions.Add(new TransactionEntity
		{
			UserId = uid,
			Amount = dep,
			Type = TransactionKind.Expense,
			CategoryId = expenseCat.Id,
			Date = date,
			Notes = $"Mandatory allocation to {g.Name}",
			CreatedAt = DateTime.UtcNow
		});

		g.CurrentAmount = decimal.Round(g.CurrentAmount + dep, 2);

		db.SavingTransactions.Add(new SavingTransactionEntity
		{
			SavingGoalId = savingGoalId,
			Amount = dep,
			Type = SavingMovement.Save,
			Date = date,
			Notes = "Mandatory 2% income allocation"
		});

		await db.SaveChangesAsync(cancellationToken);
		DataChanged?.Invoke(this, EventArgs.Empty);
	}

	public async Task<int> CreateSavingGoalAsync(string name, decimal targetAmount, DateTime targetDate, CancellationToken cancellationToken = default)
	{
		var uid = CurrentUserIdOrNull()
			?? throw new InvalidOperationException("Not signed in.");

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var g = new SavingGoalEntity
		{
			UserId = uid,
			Name = name.Trim(),
			TargetAmount = decimal.Round(targetAmount, 2),
			CurrentAmount = 0,
			TargetDate = targetDate.Date,
			IsEnded = false
		};
		db.SavingGoals.Add(g);
		await db.SaveChangesAsync(cancellationToken);
		DataChanged?.Invoke(this, EventArgs.Empty);
		return g.Id;
	}

	public async Task UpdateSavingGoalAsync(int id, string name, decimal targetAmount, DateTime targetDate, CancellationToken cancellationToken = default)
	{
		var uid = CurrentUserIdOrNull()
			?? throw new InvalidOperationException("Not signed in.");

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var g = await db.SavingGoals.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid, cancellationToken)
			?? throw new InvalidOperationException("Goal not found.");
		g.Name = name.Trim();
		g.TargetAmount = decimal.Round(targetAmount, 2);
		g.TargetDate = targetDate.Date;
		await db.SaveChangesAsync(cancellationToken);
		DataChanged?.Invoke(this, EventArgs.Empty);
	}

	public async Task SetSavingGoalEndedAsync(int id, bool isEnded, CancellationToken cancellationToken = default)
	{
		var uid = CurrentUserIdOrNull()
			?? throw new InvalidOperationException("Not signed in.");

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var g = await db.SavingGoals.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid, cancellationToken)
			?? throw new InvalidOperationException("Goal not found.");
		g.IsEnded = isEnded;
		await db.SaveChangesAsync(cancellationToken);
		DataChanged?.Invoke(this, EventArgs.Empty);
	}

	public async Task AddSavingMovementAsync(int goalId, decimal amount, SavingMovement movement, DateTime date, string? notes, CancellationToken cancellationToken = default)
	{
		var uid = CurrentUserIdOrNull()
			?? throw new InvalidOperationException("Not signed in.");

		if (amount <= 0)
			throw new ArgumentOutOfRangeException(nameof(amount));

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var g = await db.SavingGoals.FirstOrDefaultAsync(x => x.Id == goalId && x.UserId == uid, cancellationToken)
			?? throw new InvalidOperationException("Goal not found.");

		var delta = movement == SavingMovement.Save ? amount : -amount;
		g.CurrentAmount = decimal.Round(g.CurrentAmount + delta, 2);
		if (g.CurrentAmount < 0)
			g.CurrentAmount = 0;

		db.SavingTransactions.Add(new SavingTransactionEntity
		{
			SavingGoalId = goalId,
			Amount = decimal.Round(amount, 2),
			Type = movement,
			Date = date,
			Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim()
		});

		var expenseCat = await GetOrCreateCategoryAsync(db, "Savings goal", "🎯", CategoryScope.Expense, cancellationToken);
		var incomeSavingsCat = await db.Categories.FirstAsync(
			c => c.Scope == CategoryScope.Income && c.Name == "Savings", cancellationToken);

		if (movement == SavingMovement.Save)
		{
			db.Transactions.Add(new TransactionEntity
			{
				UserId = uid,
				Amount = decimal.Round(amount, 2),
				Type = TransactionKind.Expense,
				CategoryId = expenseCat.Id,
				Date = date,
				Notes = $"Saved to {g.Name}",
				CreatedAt = DateTime.UtcNow
			});
		}
		else
		{
			db.Transactions.Add(new TransactionEntity
			{
				UserId = uid,
				Amount = decimal.Round(amount, 2),
				Type = TransactionKind.Income,
				CategoryId = incomeSavingsCat.Id,
				Date = date,
				Notes = $"Withdrawn from {g.Name}",
				CreatedAt = DateTime.UtcNow
			});
		}

		await db.SaveChangesAsync(cancellationToken);
		DataChanged?.Invoke(this, EventArgs.Empty);
	}

	static async Task<CategoryEntity> GetOrCreateCategoryAsync(
		SpendyDbContext db,
		string name,
		string icon,
		CategoryScope scope,
		CancellationToken ct)
	{
		var existing = await db.Categories.FirstOrDefaultAsync(c => c.Name == name && c.Scope == scope, ct);
		if (existing is not null)
			return existing;

		var created = new CategoryEntity { Name = name, Icon = icon, Scope = scope };
		db.Categories.Add(created);
		await db.SaveChangesAsync(ct);
		return created;
	}
}
