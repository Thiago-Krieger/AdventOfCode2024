using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

public static partial class Day3
{
    private static readonly string Example;
    private static readonly string Input;
    private static readonly Regex NumberPattern = MyRegex();

    static Day3()
    {
        Example = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Data/Day3.Example"));
        Input = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Data/Inputs/Day3.Input"));
    }

    public static void Problem1(bool runOnExample)
    {
        var input = runOnExample ? Example : Input;

        var sum = DoTheMulRecursive(input);

        Console.WriteLine($"Result of Day 3 Problem 1: {sum}. Running on example {runOnExample}");
    }

    private static int DoTheMulRecursive(string input, bool ignoreDont = true)
    {
        if (MultiplyInput(input, out var result))
            return result;

        var mulIndexes = new Stack<int>();
        var isEnabled = true;
        var sum = 0;

        for (var i = 0; i < input.Length; i++)
        {
            if (i + 7 <= input.Length && input.Substring(i, 7) == "don't()")
            {
                i += 6;

                isEnabled = false;
                continue;
            }

            if (!isEnabled && !ignoreDont)
            {
                if (i + 4 <= input.Length && input.Substring(i, 4) != "do()")
                    continue;

                i += 3;
                isEnabled = true;

                continue;
            }

            i = CheckForMul(input, i, mulIndexes, ref sum);
        }

        return sum;
    }

    private static int CheckForMul(string input, int i, Stack<int> mulIndexes, ref int sum)
    {
        if (i + 4 <= input.Length && input.Substring(i, 4) == "mul(")
        {
            i += 3;
            mulIndexes.Push(i);

            return i;
        }

        if (input[i] != ')')
            return i;

        if (mulIndexes.Count == 0)
            return i;

        var startIndex = mulIndexes.Pop();
        var length = i - startIndex + 1;
        var subInput = input.Substring(startIndex, length);

        if (MultiplyInput(subInput, out var internalResult))
            sum += internalResult;

        return i;
    }

    private static bool MultiplyInput(string input, out int multiply)
    {
        multiply = 0;

        if (!NumberPattern.IsMatch(input))
            return false;

        var operators = input.Replace("(", "")
            .Replace(")", "")
            .Split(',')
            .Select(
                factorString =>
                {
                    int.TryParse(factorString, out var factor);

                    return factor;
                })
            .ToArray();

        multiply = operators[0] * operators[1];

        return true;
    }

    public static void Problem2(bool runOnExample)
    {
        var input = runOnExample ? Example : Input;

        var sum = DoTheMulRecursive(input, false);

        Console.WriteLine($"Result of Day 3 Problem 2: {sum}. Running on example {runOnExample}");
    }

    [GeneratedRegex(@"^\(\d{1,3},\d{1,3}\)")]
    private static partial Regex MyRegex();
}