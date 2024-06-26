/*
   Bitvantage.NumericRange
   Copyright (C) 2024 Michael Crino
   
   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU Affero General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.
   
   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU Affero General Public License for more details.
   
   You should have received a copy of the GNU Affero General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.Text;
using System.Text.RegularExpressions;

namespace Bitvantage;

public class NumericRange
{
    private static readonly Regex RangeRegex = new(@"^((((?<start>\d+)((-(?<end>\d+))|(?<end>)))(,|$))*)$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    private readonly SortedSet<ValueRange> _values = new();

    public void Add(ulong value)
    {
        Add(value, value);
    }

    // TODO: overwrite add, and subtract?
    public void Add(ulong startValue, ulong endValue)
    {
        var removeList = new List<ValueRange>();


        // 10-20, 25, 27-30, 33-53

        // merge any partial values into the beginning
        // for example adding 21-23 should become 10-23, 25 ,27-30, 33-53
        var previousValues = _values.GetViewBetween(new ValueRange(0), startValue > 0 ? new ValueRange(startValue - 1) : ValueRange.Min);
        var previousValuesMax = previousValues.Max;


        if (previousValuesMax.IsSet && startValue <= previousValuesMax.EndValue + 1 && previousValuesMax.StartValue < endValue)
        {
            if (previousValuesMax.StartValue < startValue)
                startValue = previousValuesMax.StartValue;

            if (previousValuesMax.EndValue > endValue)
                endValue = previousValuesMax.EndValue;

            removeList.Add(previousValuesMax);
        }


        var nextValues = _values.GetViewBetween(startValue > 0 ? new ValueRange(startValue - 1) : ValueRange.Min, ValueRange.Max);

        // consolidate overlapped values
        // for example adding 25-40 should become 10-20, 25-53
        var enumerator = nextValues.GetEnumerator();
        while (enumerator.MoveNext() && enumerator.Current.StartValue <= endValue)
        {
            if (enumerator.Current.EndValue > endValue)
                endValue = enumerator.Current.EndValue;

            removeList.Add(enumerator.Current);
        }

        enumerator.Dispose();


        foreach (var valueRange in removeList)
            _values.Remove(valueRange);

        _values.Add(new ValueRange(startValue, endValue));
    }

    public void Add(NumericRange numericRange)
    {
        foreach (var range in numericRange._values)
            Add(range.StartValue, range.EndValue);
    }

    public void Remove(NumericRange numericRange)
    {
        foreach (var range in numericRange._values)
            Remove(range.StartValue, range.EndValue);
    }


    public void Clear()
    {
        _values.Clear();
    }

    public bool Contains(ulong value)
    {
        var previousValues = _values.GetViewBetween(new ValueRange(0), value > 0 ? new ValueRange(value) : ValueRange.Min);

        var previousValueMax = previousValues.Max;

        if (!previousValueMax.IsSet)
            return false;

        return previousValueMax.StartValue <= value && value <= previousValueMax.EndValue;
    }

    public static NumericRange operator +(NumericRange numericRange, ulong value)
    {
        numericRange.Add(value);
        return numericRange;
    }

    public static NumericRange Parse(string value)
    {
        var match = RangeRegex.Match(value);

        if (!match.Success)
            throw new ArgumentException("Could not parse string");

        var range = new NumericRange();

        for (var i = 0; i < match.Groups["start"].Captures.Count; i++)
        {
            var startValue = ulong.Parse(match.Groups["start"].Captures[i].Value);

            ulong endValue;
            var endValueString = match.Groups["end"].Captures[i].Value;
            if (endValueString == string.Empty)
                endValue = startValue;
            else
                endValue = ulong.Parse(endValueString);

            range.Add(startValue, endValue);
        }

        return range;
    }

    public void Remove(ulong value)
    {
        Remove(value, value);
    }

    public void Remove(ulong startValue, ulong endValue)
    {
        var removeList = new List<ValueRange>();
        var addList = new List<ValueRange>();

        // get the ValueRange range at or before this one starts
        // Example: 10-100, 110-120
        // if we want to remove 115, we need to get the value range 110-120
        var lowerMaxValueRange = _values.GetViewBetween(ValueRange.Min, new ValueRange(startValue)).Max;

        var upperValueRanges = _values.GetViewBetween(new ValueRange(startValue), ValueRange.Max);

        ValueRange[] previousValueRange;
        if (lowerMaxValueRange.IsSet && lowerMaxValueRange != upperValueRanges.Min && lowerMaxValueRange.EndValue >= startValue)
            previousValueRange = new[] { lowerMaxValueRange };
        else
            previousValueRange = new ValueRange[] { };

        foreach (var currentRange in previousValueRange.Concat(upperValueRanges))
        {
            // the current range is completely contained in the removal range, remove the current range 
            // Range:  100-150,1000
            // Remove: 0-999
            // Result: 1000
            if (currentRange.StartValue >= startValue && currentRange.EndValue <= endValue)
                removeList.Add(currentRange);

            // the removal range is in between the current range, remove the current range and add two shoulder ranges
            // Range:  100-150
            // Remove: 125
            // Result: 100-124,126-150
            else if (currentRange.StartValue > startValue && currentRange.EndValue < endValue)
            {
                removeList.Add(currentRange);
                addList.Add(new ValueRange(currentRange.StartValue, startValue - 1));
                addList.Add(new ValueRange(endValue + 1, currentRange.EndValue));
            }

            // the removal range overlaps the left side of the current range and is contained by the right side, remove the current range and add two shoulder ranges
            // Range:  100-150
            // Remove: 90-120
            // Result: 121-150
            else if (currentRange.StartValue < startValue && currentRange.EndValue >= startValue)
            {
                removeList.Add(currentRange);
                addList.Add(new ValueRange(currentRange.StartValue, endValue - 1));
                addList.Add(new ValueRange(endValue + 1, currentRange.EndValue));
            }

            // the removal range is contained by the left side and overlaps the right side of the current range , remove the current range and add two shoulder ranges
            // Range:  100-150
            // Remove: 120-200
            // Result: 100-119
            else if (currentRange.StartValue >= startValue && currentRange.EndValue < endValue)
            {
                removeList.Add(currentRange);
                addList.Add(new ValueRange(currentRange.EndValue + 1, endValue));
            }
            else
                break;
        }

        foreach (var range in removeList)
            _values.Remove(range);

        foreach (var range in addList)
            _values.Add(range);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var value in _values)
        {
            if (sb.Length > 0)
                sb.Append(',');

            sb.Append(value);
        }

        return sb.ToString();
    }

    //public static NumericRange operator -(NumericRange numericRange, ulong value)
    //{
    //    numericRange.Remove(value);
    //    return numericRange;
    //}

    public IEnumerable<ulong> Values()
    {
        foreach (var value in _values)
            for (var i = value.StartValue; i <= value.EndValue; i++)
                yield return i;
    }

    public IEnumerable<string> Ranges()
    {
        return _values.Select(value => value.ToString());
    }

    private readonly struct ValueRange : IComparable<ValueRange>
    {
        public readonly ulong StartValue;
        public readonly ulong EndValue;

        public static ValueRange Max => new(ulong.MaxValue);
        public static ValueRange Min => new(ulong.MinValue);

        public readonly bool IsSet;

        public ValueRange(ulong startValue, ulong endValue)
        {
            StartValue = startValue;
            EndValue = endValue;
            IsSet = true;
        }


        public ValueRange(ulong startValue)
        {
            StartValue = startValue;
            EndValue = startValue;
        }

        public override string ToString()
        {
            if (StartValue == EndValue)
                return StartValue.ToString();

            return $"{StartValue}-{EndValue}";
        }

        public static bool operator ==(ValueRange first, ValueRange second)
        {
            return first.StartValue == second.StartValue && first.EndValue == second.EndValue && first.IsSet == second.IsSet;
        }

        public static bool operator !=(ValueRange first, ValueRange second)
        {
            return !(first == second);
        }

        public int CompareTo(ValueRange other)
        {
            return StartValue.CompareTo(other.StartValue);
            //var startValueComparison = StartValue.CompareTo(other.StartValue);
            //if (startValueComparison != 0) 
            //    return startValueComparison;

            //return EndValue.CompareTo(other.EndValue);
        }
    }
}
