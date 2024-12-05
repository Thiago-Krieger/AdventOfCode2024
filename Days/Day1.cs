namespace AdventOfCode2024.Days;

public static class Day1
{
    private static readonly string[] Example;
    private static readonly string[] Input;
    
    static Day1()
    {
        Example = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Data/Day1.Example"));
        Input = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Data/Inputs/Day1.Input"));
    }
    
    public static void Problem1(bool runOnExample)
    {
        var (column1, column2) = ExtractColumnsList(runOnExample);

        var orderedFirstList = column1.Order().ToList();
        var orderedSecondList = column2.Order().ToList();

        var sum = 0;
        for (var i = 0; i < column1.Count; i++)
        {
            sum += Math.Abs(orderedFirstList[i] - orderedSecondList[i]);
        }
        
        Console.WriteLine($"Result of Day 1 Problem 1: {sum}. Running on example {runOnExample}");
    }

    public static void Problem2(bool runOnExample)
    {
        var sum = 0;

        var (column1, column2) = ExtractColumnsList(runOnExample);

        foreach (var id in column1)
        {
            var multiplier = column2.Count(elementOnSecondColumn => elementOnSecondColumn == id);

            sum += Math.Abs(id * multiplier);
        }
        
        Console.WriteLine($"Result of Day 1 Problem 2: {sum}. Running on example {runOnExample}");
    }

    private static (List<int> column1, List<int> column2) ExtractColumnsList(bool runOnExample)
    {
        var input = runOnExample ? Example : Input;

        var column1 = new List<int>();
        var column2 = new List<int>();
        
        foreach (var line in input)
        {
            var elements = line.Split("   ", StringSplitOptions.RemoveEmptyEntries);
            
            if(int.TryParse(elements[0], out var firstColumnElement))
                column1.Add(firstColumnElement);
            
            if(int.TryParse(elements[1], out var secondColumnElement))
                column2.Add(secondColumnElement);
        }

        return (column1, column2);
    }
}