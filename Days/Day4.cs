namespace AdventOfCode2024.Days;

public sealed class Day4
{
    private static readonly char[][] Example;
    private static readonly char[][] Input;
    private static readonly char[] Target = ['X', 'M', 'A', 'S'];

    static Day4()
    {
        var inputExample = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Data/Day4.Example"));

        Example = inputExample.Select(column => column.ToArray()).ToArray();

        var input = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Data/Inputs/Day4.Input"));

        Input = input.Select(column => column.ToArray()).ToArray();
    }

    public static void Problem1(bool runOnExample)
    {
        var input = runOnExample ? Example : Input;

        var sum = 0;

        for (var row = 0; row < input.Length; row++)
        {
            for (var column = 0; column < input[row].Length; column++)
            {
                var me = input[row][column];

                if (me != 'X')
                    continue;

                sum += IsXmasStartingIn(row, column, input);
            }
        }

        Console.WriteLine($"Result of Day 4 Problem 1: {sum}. Running on example {runOnExample}");
    }

    private static int IsXmasStartingIn(int row, int column, char[][] input)
    {
        var isXmasIn = new[]
        {
            IsXmas(row, column, input, Direction.Right), IsXmas(row, column, input, Direction.Down), IsXmas(row, column, input, Direction.Up),
            IsXmas(row, column, input, Direction.Left), IsXmas(row, column, input, Direction.UpRight),
            IsXmas(row, column, input, Direction.DownRight), IsXmas(row, column, input, Direction.DownLeft),
            IsXmas(row, column, input, Direction.UpLeft)
        };

        return isXmasIn.Count(isXmas => isXmas);
    }

    private static bool IsXmas(int row, int column, char[][] input, Direction dir)
    {
        while (true)
        {
            var me = input[row][column];
            var neighborPosition = NeighborPosition(row, column, dir);
            var neighbor = TryGetChar(neighborPosition.Item1, neighborPosition.Item2, input);

            if (neighbor != NextXmas(me))
                return false;

            if (me == 'A' || neighbor == 'S')
                return true;

            row = neighborPosition.Item1;

            column = neighborPosition.Item2;
        }
    }

    private static char NextXmas(char me)
    {
        return me switch
        {
            'X' => 'M',
            'M' => 'A',
            'A' => 'S',
            _ => Convert.ToChar(" ")
        };
    }

    private static char? TryGetChar(int row, int column, char[][] input)
    {
        char neighbor;

        try
        {
            neighbor = input[row][column];
        }
        catch
        {
            return null;
        }

        return neighbor;
    }

    private static (int, int) NeighborPosition(int row, int column, Direction direction)
    {
        var candidatePosition = direction switch
        {
            Direction.Up => (row - 1, column),
            Direction.UpRight => (row - 1, column + 1),
            Direction.Right => (row, column + 1),
            Direction.DownRight => (row + 1, column + 1),
            Direction.Down => (row + 1, column),
            Direction.DownLeft => (row + 1, column - 1),
            Direction.Left => (row, column - 1),
            Direction.UpLeft => (row - 1, column - 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        return candidatePosition;
    }

    private enum Direction
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft
    }

    public static void Problem2(bool runOnExample)
    {
        var input = runOnExample ? Example : Input;

        var sum = 0;

        for (var row = 0; row < input.Length; row++)
        {
            for (var column = 0; column < input[row].Length; column++)
            {
                var me = input[row][column];

                if (me != 'M')
                    continue;

                sum += IsX_MasStartingIn(row, column, input);
            }
        }

        Console.WriteLine($"Result of Day 4 Problem 2: {sum/2}. Running on example {runOnExample}");
    }

    private static int IsX_MasStartingIn(int row, int column, char[][] input)
    {
        var isXmasIn = new[]
        {
            IsX_mas(row, column, input, Direction.UpRight), IsX_mas(row, column, input, Direction.DownRight),
            IsX_mas(row, column, input, Direction.DownLeft), IsX_mas(row, column, input, Direction.UpLeft)
        };

        return isXmasIn.Count(isXmas => isXmas);
    }

    private static bool IsX_mas(int row, int column, char[][] input, Direction dir)
    {
        while (true)
        {
            var me = input[row][column];
            var neighborPosition = NeighborPosition(row, column, dir);
            var neighbor = TryGetChar(neighborPosition.Item1, neighborPosition.Item2, input);

            if (neighbor != NextXmas(me))
                return false;

            if (me == 'A' && neighbor == 'S')
            {
                switch (dir)
                {
                    case Direction.UpRight:
                    case Direction.DownLeft:
                    {
                        var upLeftNeighborPosition = NeighborPosition(row, column, Direction.UpLeft);
                        var upLeftNeighbor = TryGetChar(upLeftNeighborPosition.Item1, upLeftNeighborPosition.Item2, input);

                        if (upLeftNeighbor != 'M' && upLeftNeighbor != 'S')
                            return false;

                        var downRightNeighborPosition = NeighborPosition(row, column, Direction.DownRight);
                        var downRightNeighbor = TryGetChar(downRightNeighborPosition.Item1, downRightNeighborPosition.Item2, input);

                        return downRightNeighbor == 'M' && upLeftNeighbor == 'S' || downRightNeighbor == 'S' && upLeftNeighbor == 'M';
                    }
                    case Direction.UpLeft:
                    case Direction.DownRight:
                    {
                        var upRightNeighborPosition = NeighborPosition(row, column, Direction.UpRight);
                        var upRightNeighbor = TryGetChar(upRightNeighborPosition.Item1, upRightNeighborPosition.Item2, input);

                        if (upRightNeighbor != 'M' && upRightNeighbor != 'S')
                            return false;

                        var downLeftNeighborPosition = NeighborPosition(row, column, Direction.DownLeft);
                        var downLeftNeighbor = TryGetChar(downLeftNeighborPosition.Item1, downLeftNeighborPosition.Item2, input);

                        return downLeftNeighbor == 'M' && upRightNeighbor == 'S' || downLeftNeighbor == 'S' && upRightNeighbor == 'M';
                    }
                }

                return false;
            }

            row = neighborPosition.Item1;

            column = neighborPosition.Item2;
        }
    }
}