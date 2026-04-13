using System.Text.RegularExpressions;

namespace Spendy.Services;

public static class PasswordPolicy
{
	static readonly Regex HasUpper = new("[A-Z]", RegexOptions.Compiled);
	static readonly Regex HasDigit = new("[0-9]", RegexOptions.Compiled);
	static readonly Regex HasSpecial = new("[^a-zA-Z0-9]", RegexOptions.Compiled);

	public const int MinLength = 8;

	public static bool TryValidate(string password, out string? error)
	{
		error = null;
		if (string.IsNullOrWhiteSpace(password))
		{
			error = "Password is required.";
			return false;
		}

		if (password.Length < MinLength)
		{
			error = $"Password must be at least {MinLength} characters.";
			return false;
		}

		if (!HasUpper.IsMatch(password))
		{
			error = "Password must contain at least one uppercase letter.";
			return false;
		}

		if (!HasDigit.IsMatch(password))
		{
			error = "Password must contain at least one number.";
			return false;
		}

		if (!HasSpecial.IsMatch(password))
		{
			error = "Password must contain at least one special character.";
			return false;
		}

		return true;
	}

	/// <summary>0 = empty, 1–4 increasing strength (for UI).</summary>
	public static int StrengthScore(string password)
	{
		if (string.IsNullOrEmpty(password))
			return 0;
		var score = 1;
		if (password.Length >= MinLength)
			score++;
		if (HasUpper.IsMatch(password) && HasDigit.IsMatch(password))
			score++;
		if (HasSpecial.IsMatch(password) && password.Length >= 12)
			score++;
		return Math.Min(score, 4);
	}

	public static string StrengthLabel(int score) => score switch
	{
		0 => "",
		1 => "Weak",
		2 => "Fair",
		3 => "Good",
		_ => "Strong"
	};
}
