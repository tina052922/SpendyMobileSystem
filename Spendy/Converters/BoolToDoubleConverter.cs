using System.Globalization;

namespace Spendy.Converters;

/// <summary>Maps true → <see cref="TrueValue"/>, false → <see cref="FalseValue"/> (e.g. selection stroke).</summary>
public sealed class BoolToDoubleConverter : IValueConverter
{
	public double TrueValue { get; set; } = 3;
	public double FalseValue { get; set; } = 0;

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> value is true ? TrueValue : FalseValue;

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
		throw new NotSupportedException();
}
