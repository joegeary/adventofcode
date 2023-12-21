namespace AdventOfCode.Y2023.Day18;

[ProblemName("Lavaduct Lagoon")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var instructions = input.Split('\n')
                .Select(l =>
                {
                    var d = l.Split(' ');
                    return new Dig(d[0], long.Parse(d[1]));
                });

        return Solve(instructions);
    }

    public object PartTwo(string input)
    {
        var instructions = input.Split('\n')
                .Select(l =>
                {
                    var d = l.Split(' ');
                    var hex = d[2][2..^1];
                    return new Dig(DirectionLookup[hex.Last()], long.Parse(hex[0..^1], System.Globalization.NumberStyles.HexNumber));
                });

        return Solve(instructions);
    }

    private readonly Dictionary<char, string> DirectionLookup = new()
    {
        { '0', "R" },
        { '1', "D" },
        { '2', "L" },
        { '3', "U" },
    };

    public double Solve(IEnumerable<Dig> instructions)
    {
        var polygons = new List<(long, long)>();
        (long row, long col) pos = (0, 0);
        var circumference = 0L;

        foreach (var instruction in instructions)
        {
            polygons.Add(pos);
            circumference += instruction.Distance;

            pos = instruction.Direction switch
            {
                "R" => (pos.row, pos.col + instruction.Distance),
                "D" => (pos.row + instruction.Distance, pos.col),
                "L" => (pos.row, pos.col - instruction.Distance),
                "U" => (pos.row - instruction.Distance, pos.col),
                _ => throw new InvalidOperationException("???")
            };
        }

        return Area(polygons) + (circumference / 2) + 1;
    }

    public double Area(List<(long row, long col)> polygons)
    {
        var n = polygons.Count;
        var result = 0.0;
        for (var i = 0; i < n - 1; i++)
        {
            result += (polygons[i].row * polygons[i + 1].col) - (polygons[i + 1].row * polygons[i].col);
        }

        return Math.Abs(result + (polygons[n - 1].row * polygons[0].col) - (polygons[0].row * polygons[n - 1].col)) / 2.0;
    }

    public record Dig(string Direction, long Distance);
}
