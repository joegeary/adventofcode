using System.Text;

namespace AdventOfCode.Y2023.Day03;

[ProblemName("Gear Ratios")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var lines = input.Split('\n');
        var results = new List<int>();

        foreach (var p in FindSymbols(lines))
            results.AddRange(FindAdjacentNumbers(lines, p));

        return results.Sum();
    }

    public object PartTwo(string input)
    {
        var lines = input.Split('\n');
        var results = new List<int>();
        checkd = [];

        foreach (var p in FindGears(lines))
        {
            var numbers = FindAdjacentNumbers(lines, p);
            if (numbers.Count == 2)
            {
                results.Add(numbers[0] * numbers[1]);
            }
        }

        return results.Sum();
    }

    private static IEnumerable<Point> FindSymbols(string[] input)
    {
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] == '.' || char.IsDigit(input[y][x])) continue;
                yield return new Point(x, y);
            }
        }
    }

    private static IEnumerable<Point> FindGears(string[] input)
    {
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] == '*')
                    yield return new Point(x, y);
            }
        }
    }

    static List<Point> checkd = new List<Point>();
    private static List<int> FindAdjacentNumbers(string[] input, Point symbol)
    {
        var results = new List<int>();
        foreach (var d in directions)
        {
            var newPoint = symbol + d;
            if (checkd.Contains(newPoint)) continue;

            if (char.IsDigit(input[newPoint.Y][newPoint.X]))
            {
                checkd.Add(newPoint);
                var sb = new StringBuilder();
                sb.Append(input[newPoint.Y][newPoint.X]);
                var newX = newPoint.X + 1;
                while (newX < input[newPoint.Y].Length && char.IsDigit(input[newPoint.Y][newX]))
                {
                    if (checkd.Contains(new Point(newX, newPoint.Y))) break;
                    checkd.Add(new Point(newX, newPoint.Y));
                    sb.Append(input[newPoint.Y][newX]);
                    newX++;
                }
                newX = newPoint.X - 1;
                while (newX >= 0 && char.IsDigit(input[newPoint.Y][newX]))
                {
                    if (checkd.Contains(new Point(newX, newPoint.Y))) break;
                    checkd.Add(new Point(newX, newPoint.Y));
                    sb.Insert(0, input[newPoint.Y][newX]);
                    newX--;
                }
                results.Add(int.Parse(sb.ToString()));
            }
        }
        return results;
    }

    private static readonly Point[] directions =
    [
        new(-1, -1),
        new(-1, 0),
        new(-1, 1),
        new(0, -1),
        new(0, 1),
        new(1, -1),
        new(1, 0),
        new(1, 1),
    ];

    private record Point(int X, int Y)
    {
        public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
    }
}
