using Microsoft.EntityFrameworkCore;
using Spendy.Data;
using Spendy.Data.Entities;
using Spendy.Models;

namespace Spendy.Services;

public sealed class SpendyDataService(
	IDbContextFactory<SpendyDbContext> dbFactory,
	ICurrencyService currency) : ISpendyDataService
{
	public event EventHandler? DataChanged;

	public async Task<decimal> GetBalanceAsync(CancellationToken cancellationToken = default)
	{
		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var income = await db.Transactions.AsNoTracking()
			.Where(t => t.Type == TransactionKind.Income)
			.SumAsync(t => (decimal?)t.Amount, cancellationToken) ?? 0;
		var expense = await db.Transactions.AsNoTracking()
			.Where(t => t.Type == TransactionKind.Expense)
			.SumAsync(t => (decimal?)t.Amount, cancellationToken) ?? 0;
		return income - expense;
	}

	public async Task<bool> HasAnyIncomeAsync(CancellationToken cancellationToken = default)
	{
		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		return await db.Transactions.AsNoTracking()
			.AnyAsync(t => t.Type == TransactionKind.Income, cancellationToken);
	}

	public async Task<UserEntity?> GetCurrentUserAsync(CancellationToken cancellationToken = default)
	{
		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		return await db.Users.AsNoTracking().OrderBy(u => u.Id).FirstOrDefaultAsync(cancellationToken);
	}

	public async Task UpsertUserAsync(UserEntity user, CancellationToken cancellationToken = default)
	{
		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var existing = await db.Users.OrderBy(u => u.Id).FirstOrDefaultAsync(cancellationToken);
		if (existing is null)
		{
			db.Users.Add(user);
		}
		else
		{
			existing.Name = user.Name;
			existing.Email = user.Email;
			existing.Phone = user.Phone;
			existing.Birthday = user.Birthday;
			existing.Gender = user.Gender;
			existing.Address = user.Address;
			existing.Handle = user.Handle;
		}

		await db.SaveChangesAsync(cancellationToken);
		DataChanged?.Invoke(this, EventArgs.Empty);
	}

	public async Task<DashboardData> GetDashboardAsync(DateTime day, TransactionKind kind, CancellationToken cancellationToken = default)
	{
		var start = day.Date;
		var end = start.AddDays(1);

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var query = db.Transactions.AsNoTracking()
			.Include(t => t.Category)
			.Where(t => t.Date >= start && t.Date < end && t.Type == kind)
			.OrderBy(t => t.Date);

		var rows = await query.ToListAsync(cancellationToken);

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
		var start = new DateTime(year, month, 1);
		var end = start.AddMonths(1);

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

		var monthTx = await db.Transactions.AsNoTracking()
			.Include(t => t.Category)
			.Where(t => t.Date >= start && t.Date < end && t.Type == kind)
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

		var byCategory = monthTx
			.GroupBy(t => new { t.CategoryId, Name = t.Category!.Name, Icon = t.Category.Icon })
			.Select(g => new CategoryStat
			{
				Name = g.Key.Name,
				Icon = g.Key.Icon,
				Amount = g.Sum(x => x.Amount),
				CurrencySymbol = currency.Symbol,
				AmountColor = kind == TransactionKind.Expense
					? Color.FromArgb("#01143D")
					: Color.FromArgb("#00D4A5")
			})
			.OrderByDescending(c => c.Amount)
			.ToList();

		var barColor = kind == TransactionKind.Expense
			? Color.FromArgb("#022268")
			: Color.FromArgb("#00D4A5");

		var title = start.ToString("MMMM yyyy", currency.Culture);
		return new StatisticsData(title, points, byCategory, barColor);
	}

	public async Task<IReadOnlyList<SavingPlan>> GetSavingPlansAsync(bool endedOnly, CancellationToken cancellationToken = default)
	{
		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var goals = await db.SavingGoals.AsNoTracking()
			.Where(g => g.IsEnded == endedOnly)
			.OrderBy(g => g.TargetDate)
			.ToListAsync(cancellationToken);

		return goals.Select(MapPlan).ToList();
	}

	public async Task<SavingPlan?> GetSavingPlanAsync(int id, CancellationToken cancellationToken = default)
	{
		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var g = await db.SavingGoals.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
		return g is null ? null : MapPlan(g);
	}

	public async Task<IReadOnlyList<SavingHistoryLine>> GetSavingHistoryAsync(int goalId, CancellationToken cancellationToken = default)
	{
		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
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
		if (amount <= 0)
			throw new ArgumentOutOfRangeException(nameof(amount));

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		db.Transactions.Add(new TransactionEntity
		{
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
		if (incomeAmount <= 0)
			throw new ArgumentOutOfRangeException(nameof(incomeAmount));
		if (mandatorySavingsAmount <= 0)
			throw new ArgumentOutOfRangeException(nameof(mandatorySavingsAmount));

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

		db.Transactions.Add(new TransactionEntity
		{
			Amount = decimal.Round(incomeAmount, 2),
			Type = TransactionKind.Income,
			CategoryId = categoryId,
			Date = date,
			Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim(),
			CreatedAt = DateTime.UtcNow
		});

		var g = await db.SavingGoals.FirstOrDefaultAsync(x => x.Id == savingGoalId, cancellationToken)
			?? throw new InvalidOperationException("Saving goal not found.");
		if (g.IsEnded)
			throw new InvalidOperationException("Cannot allocate to an ended goal.");

		var dep = decimal.Round(mandatorySavingsAmount, 2);
		g.CurrentAmount = decimal.Round(g.CurrentAmount + dep, 2);

		db.SavingTransactions.Add(new SavingTransactionEntity
		{
			SavingGoalId = savingGoalId,
			Amount = dep,
			Type = SavingMovement.Save,
			Date = DateTime.Now,
			Notes = "Mandatory 2% income allocation"
		});

		await db.SaveChangesAsync(cancellationToken);
		DataChanged?.Invoke(this, EventArgs.Empty);
	}

	public async Task<int> CreateSavingGoalAsync(string name, decimal targetAmount, DateTime targetDate, CancellationToken cancellationToken = default)
	{
		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var g = new SavingGoalEntity
		{
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
		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var g = await db.SavingGoals.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
			?? throw new InvalidOperationException("Goal not found.");
		g.Name = name.Trim();
		g.TargetAmount = decimal.Round(targetAmount, 2);
		g.TargetDate = targetDate.Date;
		await db.SaveChangesAsync(cancellationToken);
		DataChanged?.Invoke(this, EventArgs.Empty);
	}

	public async Task SetSavingGoalEndedAsync(int id, bool isEnded, CancellationToken cancellationToken = default)
	{
		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var g = await db.SavingGoals.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
			?? throw new InvalidOperationException("Goal not found.");
		g.IsEnded = isEnded;
		await db.SaveChangesAsync(cancellationToken);
		DataChanged?.Invoke(this, EventArgs.Empty);
	}

	public async Task AddSavingMovementAsync(int goalId, decimal amount, SavingMovement movement, DateTime date, string? notes, CancellationToken cancellationToken = default)
	{
		if (amount <= 0)
			throw new ArgumentOutOfRangeException(nameof(amount));

		await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
		var g = await db.SavingGoals.FirstOrDefaultAsync(x => x.Id == goalId, cancellationToken)
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

		await db.SaveChangesAsync(cancellationToken);
		DataChanged?.Invoke(this, EventArgs.Empty);
	}
}
