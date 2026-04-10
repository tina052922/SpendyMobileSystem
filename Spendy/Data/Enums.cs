namespace Spendy.Data;

public enum TransactionKind
{
	Expense = 0,
	Income = 1
}

public enum SavingMovement
{
	Save = 0,
	Withdraw = 1
}

/// <summary>Whether a category appears for expense, income, or both pickers.</summary>
public enum CategoryScope
{
	Expense = 0,
	Income = 1,
	Both = 2
}
