using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Storage;
using Spendy.Data.Entities;

namespace Spendy.Data;

/// <summary>Creates SQLite and seeds category metadata only — no demo transactions, goals, or sample user.</summary>
public sealed class SpendyDbInitializer(IDbContextFactory<SpendyDbContext> factory)
{
	const string LegacyFinancialPurgeKey = "SpendyLegacyFinancialPurge_v1";

	public async Task InitializeAsync(CancellationToken cancellationToken = default)
	{
		await using var db = await factory.CreateDbContextAsync(cancellationToken);
		await db.Database.EnsureCreatedAsync(cancellationToken);

		await PurgeLegacyFinancialDataOnceAsync(db, cancellationToken);

		if (await db.Categories.AnyAsync(cancellationToken))
			return;

		await SeedCategoriesOnlyAsync(db, cancellationToken);
	}

	/// <summary>
	/// One-time cleanup for devices that still have old seeded demo transactions/goals from earlier builds.
	/// Safety rule: never purge if a real user exists.
	/// This protects real data if Preferences are cleared or app is updated.
	/// </summary>
	static async Task PurgeLegacyFinancialDataOnceAsync(SpendyDbContext db, CancellationToken ct)
	{
		if (Preferences.Get(LegacyFinancialPurgeKey, false))
			return;

		// If a user exists, assume data is real and do not delete anything.
		if (await db.Users.AnyAsync(ct))
		{
			Preferences.Set(LegacyFinancialPurgeKey, true);
			return;
		}

		// Only purge when there are no users (legacy demo DB).
		await db.SavingTransactions.ExecuteDeleteAsync(ct);
		await db.SavingGoals.ExecuteDeleteAsync(ct);
		await db.Transactions.ExecuteDeleteAsync(ct);

		Preferences.Set(LegacyFinancialPurgeKey, true);
	}

	static async Task SeedCategoriesOnlyAsync(SpendyDbContext db, CancellationToken ct)
	{
		var expenseCategories = new[]
		{
			new CategoryEntity { Name = "Food", Icon = "🍔", Scope = CategoryScope.Expense },
			new CategoryEntity { Name = "Traffic", Icon = "🚗", Scope = CategoryScope.Expense },
			new CategoryEntity { Name = "Shopping", Icon = "🛍️", Scope = CategoryScope.Expense },
			new CategoryEntity { Name = "Grocery", Icon = "🛒", Scope = CategoryScope.Expense },
			new CategoryEntity { Name = "Notes", Icon = "📚", Scope = CategoryScope.Expense },
			new CategoryEntity { Name = "Health", Icon = "💊", Scope = CategoryScope.Expense },
			new CategoryEntity { Name = "Home", Icon = "🏠", Scope = CategoryScope.Expense },
			new CategoryEntity { Name = "Gift", Icon = "🎁", Scope = CategoryScope.Expense },
			new CategoryEntity { Name = "Digital", Icon = "💾", Scope = CategoryScope.Expense },
			new CategoryEntity { Name = "More", Icon = "•••", Scope = CategoryScope.Expense },
		};

		var incomeCategories = new[]
		{
			new CategoryEntity { Name = "Salary", Icon = "💼", Scope = CategoryScope.Income },
			new CategoryEntity { Name = "Business", Icon = "📈", Scope = CategoryScope.Income },
			new CategoryEntity { Name = "Allowance", Icon = "💳", Scope = CategoryScope.Income },
			new CategoryEntity { Name = "Savings", Icon = "🏦", Scope = CategoryScope.Income },
			new CategoryEntity { Name = "More", Icon = "•••", Scope = CategoryScope.Income },
		};

		db.Categories.AddRange(expenseCategories);
		db.Categories.AddRange(incomeCategories);
		await db.SaveChangesAsync(ct);
	}
}
