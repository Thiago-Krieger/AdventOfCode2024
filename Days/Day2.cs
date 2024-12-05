namespace AdventOfCode2024.Days;

public sealed class Day2
{
    private static readonly List<List<int>> Example;
    private static readonly List<List<int>> Input;

    static Day2()
    {
        var inputExample = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Data/Day2.Example"));

        Example = inputExample.Select(
                row =>
                {
                    return row.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(
                            value =>
                            {
                                int.TryParse(value, out var intValue);

                                return intValue;
                            })
                        .ToList();
                })
            .ToList();

        var input = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Data/Inputs/Day2.Input"));

        Input = input.Select(
                row =>
                {
                    return row.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(
                            value =>
                            {
                                int.TryParse(value, out var intValue);

                                return intValue;
                            })
                        .ToList();
                })
            .ToList();
    }

    public static void Problem1(bool runOnExample)
    {
        var input = runOnExample ? Example : Input;

        var sum = 0;

        foreach (var row in input)
        {
            var safe = false;

            for (var i = 1; i < row.Count - 1; i++)
            {
                var backwardDifference = row[i - 1] - row[i];
                var difference = row[i] - row[i + 1];

                safe =
                    Math.Abs(difference) <= 3
                    && Math.Abs(backwardDifference) <= 3 // max value for the difference
                    && Math.Sign(difference) == Math.Sign(backwardDifference); // the sequence is increasing or decreasing;

                if (!safe)
                    break;
            }

            if (safe)
                sum += 1;
        }

        Console.WriteLine($"Result of Day 2 Problem 2: {sum}. Running on example {runOnExample}");
    }

    public static void Problem2(bool runOnExample)
    {
        var input = runOnExample ? Example : Input;

        var sum  = ValidateCollection(input, runOnExample);
        
        Console.WriteLine($"Result of Day 1 Problem 2: {sum}. Running on example {runOnExample}");
    }

    private static int ValidateCollection(List<List<int>> input, bool verbose)
    {
        var safeReports = 0;

        foreach (var row in input)
        {
            var printableRows = string.Join(' ', row);
            var isSafe = IsSafe(row);

            if (isSafe)
            {
                if(verbose)
                    Console.WriteLine($"Report {printableRows}: Safe without removing any level");
                safeReports++;
                continue;
            }

            var canBeMadeValidResult = CanBeSafeByRemovingOneLevel(row);

            if (canBeMadeValidResult.Item1)
            {
                if(verbose)
                    Console.WriteLine(
                    $"Report {printableRows}: Safe by removing the level {canBeMadeValidResult.Item2?.Item1} at {canBeMadeValidResult.Item2?.Item2} position");
                
                safeReports++;

                continue;
            }

            if(verbose)
                Console.WriteLine($"Report {printableRows}: Unsafe regardless of which level is removed.");
        }

        if(verbose)
            Console.WriteLine($"Total Safe Reports: {safeReports}");
        
        return safeReports;
    }

    private static bool IsSafe(List<int> row)
    {
        for (var i = 1; i < row.Count; i++)
        {
            var difference = row[i] - row[i - 1];

            if (Math.Abs(difference) > 3 || difference == 0)
                return false;

            // Ensure all differences have the same sign (increasing or decreasing)
            if (i > 1 && Math.Sign(difference) != Math.Sign(row[i - 1] - row[i - 2]))
                return false;
        }

        return true;
    }

    private static (bool, Tuple<int, int>?) CanBeSafeByRemovingOneLevel(List<int> row)
    {
        if (IsSafe(row.Skip(1).ToList()))
            return (true, new Tuple<int, int>(row[0], 0));
        
        for (var i = 1; i < row.Count; i++)
        {
            var difference = row[i] - row[i - 1];

            if (Math.Abs(difference) <= 3 && difference != 0 && (i <= 1 || Math.Sign(difference) == Math.Sign(row[i - 1] - row[i - 2])))
                continue;

            // Test removing the current invalid level or the next
            if (IsSafe(row.Where((_, index) => index != i - 1).ToList()))
                return (true, new Tuple<int, int>(row[i - 1], i - 1));

            return IsSafe(row.Where((_, index) => index != i).ToList()) ? (true, new Tuple<int, int>(row[i], i)) : (false, null);
        }

        return (false, null);
    }
}