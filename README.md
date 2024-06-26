# Bitvantage.NumericRange
Parse and manulipulate huge quantities of numeric ranges such as 40-70,90,95,300-700,5000.

## Installing via NuGet Package Manager
```
PM> NuGet\Install-Package Bitvantage.NumericRange
```

## Quick Start
```csharp
var range = NumericRange.Parse("40-70,90,95,300-700,702,5000");
range.Add(701);							// 40-70,90,95,300-702,5000
range.Add(1000,2000);					// 40-70,90,95,300-702,1000-2000,5000
range.Remove(80,100);					// 40-70,300-702,1000-2000,5000
range.Remove(1500);						// 40-70,300-702,1000-1499,1501-2000,5000

range.Contains(500);					// true;
range.Contains(800);					// false;

Console.WriteLine(range.ToString());	// 40-70,300-702,1000-1499,1501-2000,5000
```

## Performance
Internally ranges are stored in a red black tree, the search time of red black trees is O(log n).