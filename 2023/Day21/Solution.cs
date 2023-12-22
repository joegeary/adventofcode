namespace AdventOfCode.Y2023.Day21;

[ProblemName("Step Counter")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var map = Map.Parse(input);
        var positions = new HashSet<Point>()
        {
            map.Start
        };

        for (var i = 0; i < 64; i++)
        {
            positions = map.Walk(positions);
        }

        return positions.Count;
    }

    public object PartTwo(string input)
    {
        var map = Map.Parse(input);

        var positions = new HashSet<Point>
        {
            map.Start
        };
        var regression = new List<Point>();
        var count = 26501365L;
        var cycles = count / map.Width;
        var reminder = count % map.Width;
        var steps = 0;

        for (var i = 0; i < 3; i++)
        {
            while (steps < (i * map.Width) + reminder)
            {
                positions = map.Walk(positions);
                steps++;
            }

            regression.Add(new Point(i, positions.Count));
        }

        return CalculateQuadraticCurve(cycles, regression);
    }

    private long CalculateQuadraticCurve(long x, List<Point> regressionPoints)
    {
        long g(long x)
        {
            double x1 = regressionPoints[0].X;
            double y1 = regressionPoints[0].Y;
            double x2 = regressionPoints[1].X;
            double y2 = regressionPoints[1].Y;
            double x3 = regressionPoints[2].X;
            double y3 = regressionPoints[2].Y;
            return (long)(((x - x2) * (x - x3) / ((x1 - x2) * (x1 - x3)) * y1) +
                    ((x - x1) * (x - x3) / ((x2 - x1) * (x2 - x3)) * y2) +
                    ((x - x1) * (x - x2) / ((x3 - x1) * (x3 - x2)) * y3));
        }

        return g(x);
    }

    public record Map(char[,] Input, Point Start)
    {
        public int Width => Input.GetLength(0);
        public int Height = Input.GetLength(1);

        public static Map Parse(string input)
        {
            var lines = input.Split('\n');
            var y = 0;

            var map = new char[lines[0].Length, lines.Length];
            Point start = null;

            foreach (var line in lines)
            {
                for (var x = 0; x < line.Length; x++)
                {
                    map[x, y] = line[x];

                    if (line[x] == 'S')
                    {
                        start = new Point(x, y);
                        map[x, y] = '.';
                    }
                }

                y++;
            }

            return new Map(map, start);
        }

        public HashSet<Point> Walk(HashSet<Point> positions)
        {
            return positions.SelectMany(p => new[] { new Point(p.X, p.Y - 1), new Point(p.X, p.Y + 1), new Point(p.X - 1, p.Y), new Point(p.X + 1, p.Y) })
                  .Where(IsValid)
                  .ToHashSet();
        }

        private bool IsValid(Point p)
        {
            var x = ((p.X % Width) + Width) % Width;
            var y = ((p.Y % Height) + Height) % Height;
            return Input[x, y] != '#';
        }
    }

    public record Point(int X, int Y);
}
