using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Storage;
using Spendy.Data.Entities;
using Spendy.Services;

namespace Spendy.Data;

/// <summary>Creates SQLite, migrates legacy columns, seeds category metadata only.</summary>
public sealed class SpendyDbInitializer(IDbContextFactory<SpendyDbContext> factory)
{
	const string LegacyFinancialPurgeKey = "SpendyLegacyFinancialPurge_v1";

	public async Task InitializeAsync(CancellationToken cancellationToken = default)
	{
		await using var db = await factory.CreateDbContextAsync(cancellationToken);
		// Creates schema only when spendy.db does not exist yet; never deletes an existing database file.
		await db.Database.EnsureCreatedAsync(cancellationToken);

		await EnsureUserProfilePhotoPathColumnAsync(db, cancellationToken);
		await EnsureUserPasswordHashColumnAsync(db, cancellationToken);
		await EnsureTransactionsUserIdColumnAsync(db, cancellationToken);
		await EnsureSavingGoalsUserIdColumnAsync(db, cancellationToken);
		await EnsureSavingGoalsStartDateColumnAsync(db, cancellationToken);
		await EnsurePasswordResetTokensTableAsync(db, cancellationToken);
		await EnsureIndexesAsync(db, cancellationToken);

		if (await db.Categories.AnyAsync(cancellationToken))
		{
			System.Diagnostics.Debug.WriteLine(
				$"[Spendy.Database] Ready (categories exist). SQLite: {SpendyDatabasePaths.SqliteDatabasePath}");
			return;
		}

		await SeedCategoriesOnlyAsync(db, cancellationToken);
		System.Diagnostics.Debug.WriteLine(
			$"[Spendy.Database] Ready (seeded categories). SQLite: {SpendyDatabasePaths.SqliteDatabasePath}");
	}

	static async Task EnsurePasswordResetTokensTableAsync(SpendyDbContext db, CancellationToken ct)
	{
		// EnsureCreated doesn't add new tables on existing DBs; create this one explicitly.
		await db.Database.ExecuteSqlRawAsync(
			"""
			CREATE TABLE IF NOT EXISTS PasswordResetTokens (
				Id INTEGER PRIMARY KEY AUTOINCREMENT,
				UserId INTEGER NOT NULL,
				TokenHash TEXT NOT NULL,
				CreatedAtUtc TEXT NOT NULL,
				ExpiresAtUtc TEXT NOT NULL,
				UsedAtUtc TEXT NULL,
				FOREIGN KEY(UserId) REFERENCES Users(Id) ON DELETE CASCADE
			);
			""", ct);

		await db.Database.ExecuteSqlRawAsync(
			"CREATE INDEX IF NOT EXISTS IX_PasswordResetTokens_UserId ON PasswordResetTokens(UserId);", ct);
		await db.Database.ExecuteSqlRawAsync(
			"CREATE INDEX IF NOT EXISTS IX_PasswordResetTokens_ExpiresAtUtc ON PasswordResetTokens(ExpiresAtUtc);", ct);
	}

	static async Task EnsureIndexesAsync(SpendyDbContext db, CancellationToken ct)
	{
		// SQLite perf: ensure our common filter+sort patterns have supporting indexes,
		// especially on existing DBs where EnsureCreated won't apply model changes.
		await db.Database.ExecuteSqlRawAsync(
			"CREATE INDEX IF NOT EXISTS IX_Transactions_UserId_Date_Type ON Transactions(UserId, Date, Type);", ct);
		await db.Database.ExecuteSqlRawAsync(
			"CREATE INDEX IF NOT EXISTS IX_SavingGoals_UserId_IsEnded_TargetDate ON SavingGoals(UserId, IsEnded, TargetDate);", ct);
		await db.Database.ExecuteSqlRawAsync(
			"CREATE INDEX IF NOT EXISTS IX_SavingTransactions_SavingGoalId_Date ON SavingTransactions(SavingGoalId, Date);", ct);
	}

	/// <summary>Avoids redundant ALTERs so EF Core does not log failed commands when columns already exist.</summary>
	static async Task<bool> SqliteColumnExistsAsync(SpendyDbContext db, string table, string column, CancellationToken ct)
	{
		if (!IsSafeSqlIdentifier(table) || !IsSafeSqlIdentifier(column))
			return false;

		var connection = db.Database.GetDbConnection();
		var shouldClose = connection.State != System.Data.ConnectionState.Open;
		if (shouldClose)
			await connection.OpenAsync(ct);
		try
		{
			await using var cmd = connection.CreateCommand();
			cmd.CommandText =
				$"SELECT COUNT(*) FROM pragma_table_info('{table}') WHERE name = '{column}';";
			var result = await cmd.ExecuteScalarAsync(ct);
			return result is not null && Convert.ToInt64(result) > 0;
		}
		finally
		{
			if (shouldClose)
				await connection.CloseAsync();
		}
	}

	static bool IsSafeSqlIdentifier(string name) =>
		name.Length > 0 && name.All(c => char.IsLetterOrDigit(c) || c == '_');

	static async Task EnsureUserProfilePhotoPathColumnAsync(SpendyDbContext db, CancellationToken ct)
	{
		if (await SqliteColumnExistsAsync(db, "Users", "ProfilePhotoPath", ct))
			return;

		await db.Database.ExecuteSqlRawAsync(
			"ALTER TABLE Users ADD COLUMN ProfilePhotoPath TEXT NULL;", ct);
	}

	static async Task EnsureUserPasswordHashColumnAsync(SpendyDbContext db, CancellationToken ct)
	{
		if (await SqliteColumnExistsAsync(db, "Users", "PasswordHash", ct))
			return;

		await db.Database.ExecuteSqlRawAsync(
			"ALTER TABLE Users ADD COLUMN PasswordHash TEXT NULL;", ct);
	}

	static async Task EnsureTransactionsUserIdColumnAsync(SpendyDbContext db, CancellationToken ct)
	{
		if (!await SqliteColumnExistsAsync(db, "Transactions", "UserId", ct))
		{
			await db.Database.ExecuteSqlRawAsync(
				"ALTER TABLE Transactions ADD COLUMN UserId INTEGER NULL;", ct);
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
		if (!await SqliteColumnExistsAsync(db, "SavingGoals", "UserId", ct))
		{
			await db.Database.ExecuteSqlRawAsync(
				"ALTER TABLE SavingGoals ADD COLUMN UserId INTEGER NULL;", ct);
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

	static async Task EnsureSavingGoalsStartDateColumnAsync(SpendyDbContext db, CancellationToken ct)
	{
		// Older DBs may lack StartDate; newer schemas expect it (NOT NULL on some deployments).
		if (!await SqliteColumnExistsAsync(db, "SavingGoals", "StartDate", ct))
		{
			await db.Database.ExecuteSqlRawAsync(
				"ALTER TABLE SavingGoals ADD COLUMN StartDate TEXT NULL;", ct);
		}

		await db.Database.ExecuteSqlRawAsync(
			"""
			UPDATE SavingGoals
			SET StartDate = TargetDate
			WHERE StartDate IS NULL;
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
