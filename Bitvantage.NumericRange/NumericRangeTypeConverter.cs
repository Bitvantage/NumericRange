/*
   Bitvantage.NumericRange
   Copyright (C) 2024 Michael Crino
   
   Licensed under the MIT License. See LICENSE.md in the project root
   for the full license text.
*/

using System.ComponentModel;
using System.Globalization;

namespace Bitvantage;

/// <summary>
/// Converts a <see cref="NumericRange"/> to and from text such as <c>80,443,8000-8100</c>.
/// </summary>
/// <remarks>
/// <see cref="NumericRange"/> uses this converter automatically. It lets Microsoft.Extensions.Configuration bind a setting
/// such as <c>"AllowedPorts": "80,443"</c> to a <see cref="NumericRange"/> property, and lets designers such as a
/// WinForms property grid show and edit a set as text.
/// </remarks>
public sealed class NumericRangeTypeConverter : TypeConverter
{
    /// <summary>
    /// Returns whether this converter can create a <see cref="NumericRange"/> from <paramref name="sourceType"/>.
    /// </summary>
    /// <param name="context">The context of the conversion, which is not used.</param>
    /// <param name="sourceType">The type to convert from.</param>
    /// <returns><see langword="true"/> for <see cref="string"/>.</returns>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    /// <summary>
    /// Returns whether this converter can convert a <see cref="NumericRange"/> to <paramref name="destinationType"/>.
    /// </summary>
    /// <param name="context">The context of the conversion, which is not used.</param>
    /// <param name="destinationType">The type to convert to.</param>
    /// <returns><see langword="true"/> for <see cref="string"/>.</returns>
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
    }

    /// <summary>
    /// Parses text such as <c>80,443,8000-8100</c> into a <see cref="NumericRange"/>.
    /// </summary>
    /// <param name="context">The context of the conversion, which is not used.</param>
    /// <param name="culture">The culture, which is not used because the format does not depend on culture.</param>
    /// <param name="value">The text to parse.</param>
    /// <returns>The parsed set.</returns>
    /// <exception cref="FormatException">The text is not a valid set.</exception>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string text)
            return NumericRange.Parse(text);

        return base.ConvertFrom(context, culture, value);
    }

    /// <summary>
    /// Converts a <see cref="NumericRange"/> to text such as <c>80,443,8000-8100</c>.
    /// </summary>
    /// <param name="context">The context of the conversion, which is not used.</param>
    /// <param name="culture">The culture, which is not used because the format does not depend on culture.</param>
    /// <param name="value">The set to convert.</param>
    /// <param name="destinationType">The type to convert to.</param>
    /// <returns>The set as text.</returns>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is NumericRange numericRange)
            return numericRange.ToString();

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
