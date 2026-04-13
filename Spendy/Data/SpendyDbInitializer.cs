using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Storage;
using Spendy.Data.Entities;

namespace Spendy.Data;

/// <summary>Creates SQLite, migrates legacy columns, seeds category metadata only.</summary>
public sealed class SpendyDbInitializer(IDbContextFactory<SpendyDbContext> factory)
{
	const string LegacyFinancialPurgeKey = "SpendyLegacyFinancialPurge_v1";

	public async Task InitializeAsync(CancellationToken cancellationToken = default)
	{
		await using var db = await factory.CreateDbContextAsync(cancellationToken);
		await db.Database.EnsureCreatedAsync(cancellationToken);

		await EnsureUserProfilePhotoPathColumnAsync(db, cancellationToken);
		await EnsureUserPasswordHashColumnAsync(db, cancellationToken);
		await EnsureTransactionsUserIdColumnAsync(db, cancellationToken);
		await EnsureSavingGoalsUserIdColumnAsync(db, cancellationToken);

		await PurgeLegacyFinancialDataOnceAsync(db, cancellationToken);

		if (await db.Categories.AnyAsync(cancellationToken))
			return;

		await SeedCategoriesOnlyAsync(db, cancellationToken);
	}

	static async Task EnsureUserProfilePhotoPathColumnAsync(SpendyDbContext db, CancellationToken ct)
	{
		try
		{
			await db.Database.ExecuteSqlRawAsync(
				"ALTER TABLE Users ADD COLUMN ProfilePhotoPath TEXT NULL;", ct);
		}
		catch (Exception ex) when (ex.Message.Contains("duplicate column", StringComparison.OrdinalIgnoreCase)
		                           || ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
		{
		}
	}

	static async Task EnsureUserPasswordHashColumnAsync(SpendyDbContext db, CancellationToken ct)
	{
		try
		{
			await db.Database.ExecuteSqlRawAsync(
				"ALTER TABLE Users ADD COLUMN PasswordHash TEXT NULL;", ct);
		}
		catch (Exception ex) when (ex.Message.Contains("duplicate column", StringComparison.OrdinalIgnoreCase)
		                           || ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
		{
		}
	}

	static async Task EnsureTransactionsUserIdColumnAsync(SpendyDbContext db, CancellationToken ct)
	{
		try
		{
			await db.Database.ExecuteSqlRawAsync(
				"ALTER TABLE Transactions ADD COLUMN UserId INTEGER NULL;", ct);
		}
		catch (Exception ex) when (ex.Message.Contains("duplicate column", StringComparison.OrdinalIgnoreCase)
		                           || ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
		{
		}

		await db.Database.ExecuteSqlRawAsync(
			"""
			UPDATE Transactions
			SET UserId = (SELECT MIN(Id) FROM Users)
			WHERE UserId IS NULL
			AND EXISTS (SELECT 1 FROM Users LIMIT 1);
			""", ct);

		await db.Database.ExecuteSqlRawAsync(
			"DELETE FROM Transactions WHERE UserId IS NULL;", ct);
	}

	static async Task EnsureSavingGoalsUserIdColumnAsync(SpendyDbContext db, CancellationToken ct)
	{
		try
		{
			await db.Database.ExecuteSqlRawAsync(
				"ALTER TABLE SavingGoals ADD COLUMN UserId INTEGER NULL;", ct);
		}
		catch (Exception ex) when (ex.Message.Contains("duplicate column", StringComparison.OrdinalIgnoreCase)
		                           || ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
		{
		}

		await db.Database.ExecuteSqlRawAsync(
			"""
			UPDATE SavingGoals
			SET UserId = (SELECT MIN(Id) FROM Users)
			WHERE UserId IS NULL
			AND EXISTS (SELECT 1 FROM Users LIMIT 1);
			""", ct);

		await db.Database.ExecuteSqlRawAsync(
			"DELETE FROM SavingGoals WHERE UserId IS NULL;", ct);

		await db.Database.ExecuteSqlRawAsync(
			"""
			DELETE FROM SavingTransactions
			WHERE SavingGoalId NOT IN (SELECT Id FROM SavingGoals);
			""", ct);
	}

	/// <summary>
	/// One-time cleanup for devices that still have old seeded demo transactions/goals from earlier builds.
	/// Safety rule: never purge if a real user exists.
	/// </summary>
	static async Task PurgeLegacyFinancialDataOnceAsync(SpendyDbContext db, CancellationToken ct)
	{
		if (Preferences.Get(LegacyFinancialPurgeKey, false))
			return;

		if (await db.Users.AnyAsync(ct))
		{
			Preferences.Set(LegacyFinancialPurgeKey, true);
			return;
		}

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
