using Microsoft.EntityFrameworkCore;
using Spendy.Data.Entities;

namespace Spendy.Data;

public sealed class SpendyDbContext : DbContext
{
	public SpendyDbContext(DbContextOptions<SpendyDbContext> options)
		: base(options)
	{
	}

	public DbSet<UserEntity> Users => Set<UserEntity>();
	public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
	public DbSet<TransactionEntity> Transactions => Set<TransactionEntity>();
	public DbSet<SavingGoalEntity> SavingGoals => Set<SavingGoalEntity>();
	public DbSet<SavingTransactionEntity> SavingTransactions => Set<SavingTransactionEntity>();
	public DbSet<PasswordResetTokenEntity> PasswordResetTokens => Set<PasswordResetTokenEntity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UserEntity>(e =>
		{
			e.HasKey(x => x.Id);
			e.Property(x => x.Name).HasMaxLength(200);
			e.Property(x => x.Email).HasMaxLength(320);
			e.Property(x => x.Phone).HasMaxLength(64);
			e.Property(x => x.Birthday).HasMaxLength(64);
			e.Property(x => x.Gender).HasMaxLength(64);
			e.Property(x => x.Address).HasMaxLength(500);
			e.Property(x => x.Handle).HasMaxLength(100);
			e.Property(x => x.ProfilePhotoPath).HasMaxLength(2048);
			e.Property(x => x.PasswordHash).HasMaxLength(256);
			e.HasIndex(x => x.Email).IsUnique();
		});

		modelBuilder.Entity<CategoryEntity>(e =>
		{
			e.HasKey(x => x.Id);
			e.Property(x => x.Name).HasMaxLength(120);
			e.Property(x => x.Icon).HasMaxLength(32);
			e.Property(x => x.Scope).HasConversion<int>();
			e.HasIndex(x => new { x.Name, x.Scope }).IsUnique();
		});

		modelBuilder.Entity<TransactionEntity>(e =>
		{
			e.HasKey(x => x.Id);
			e.Property(x => x.Type).HasConversion<int>();
			e.Property(x => x.Amount).HasPrecision(18, 2);
			e.Property(x => x.Notes).HasMaxLength(2000);
			e.Property(x => x.CreatedAt);
			e.HasOne(x => x.Category)
				.WithMany()
				.HasForeignKey(x => x.CategoryId)
				.OnDelete(DeleteBehavior.Restrict);
			e.HasIndex(x => x.Date);
			e.HasIndex(x => x.UserId);
			e.HasOne(x => x.User)
				.WithMany()
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<SavingGoalEntity>(e =>
		{
			e.HasKey(x => x.Id);
			e.Property(x => x.Name).HasMaxLength(200);
			e.Property(x => x.TargetAmount).HasPrecision(18, 2);
			e.Property(x => x.CurrentAmount).HasPrecision(18, 2);
			e.HasIndex(x => x.UserId);
			e.HasOne(x => x.User)
				.WithMany()
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Cascade);
			e.HasMany(x => x.Transactions)
				.WithOne(x => x.SavingGoal)
				.HasForeignKey(x => x.SavingGoalId)
				.OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<SavingTransactionEntity>(e =>
		{
			e.HasKey(x => x.Id);
			e.Property(x => x.Type).HasConversion<int>();
			e.Property(x => x.Amount).HasPrecision(18, 2);
			e.Property(x => x.Notes).HasMaxLength(2000);
			e.HasIndex(x => x.Date);
		});

		modelBuilder.Entity<PasswordResetTokenEntity>(e =>
		{
			e.HasKey(x => x.Id);
			e.Property(x => x.TokenHash).HasMaxLength(128);
			e.HasIndex(x => x.UserId);
			e.HasIndex(x => x.ExpiresAtUtc);
			e.HasOne(x => x.User)
				.WithMany()
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		});
	}
}
