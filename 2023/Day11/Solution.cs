namespace AdventOfCode.Y2023.Day11;

[ProblemName("Cosmic Expansion")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var map = new Map(input.Split('\n').Select(l => l.ToCharArray()).ToArray());
        return map.Galaxies
            .Pairs()
            .Select(p => map.Distance(p.Item1, p.Item2))
            .Sum();
    }

    public object PartTwo(string input)
    {
        var map = new Map(input.Split('\n').Select(l => l.ToCharArray()).ToArray());
        return map.Galaxies
            .Pairs()
            .Select(p => map.Distance(p.Item1, p.Item2, 1000000))
            .Sum();
    }

    private record Map(char[][] Input)
    {
        public int Width => Input[0].Length;
        public int Height => Input.Length;

        public IEnumerable<char> Row(int rowIndex) => Input[rowIndex];
        public IEnumerable<char> Column(int colIndex) => Input.Select(r => r[colIndex]);

        private HashSet<Point> _galaxies;
        public HashSet<Point> Galaxies => _galaxies ??= new HashSet<Point>(
                                    Input.SelectMany((r, x) => r.Select((c, y) => (x, y, c)))
                                        .Where(rc => rc.c == '#')
                                        .Select(rc => new Point(rc.x, rc.y))
                                );

        public long Distance(Point p1, Point p2, long dilation = 2)
        {
            var dx = Math.Sign(p2.X - p1.X);
            var dy = Math.Sign(p2.Y - p1.Y);

            var x = p1.X;
            long distance = 0;
            while (x != p2.X)
            {
                if (!Row(x).Contains('#'))
                {
                    distance += dilation;
                }
                else
                {
                    distance++;
                }
                x += dx;
            }

            var y = p1.Y;
            while (y != p2.Y)
            {
                if (!Column(y).Contains('#'))
                {
                    distance += dilation;
                }
                else
                {
                    distance++;
                }
                y += dy;
            }

            return distance;
        }
    }

    private record Point(int X, int Y);
}

public static class Extensions
{
    public static IEnumerable<(T, T)> Pairs<T>(this IEnumerable<T> source)
    {
        var asList = source.ToList();
        foreach (var i in Enumerable.Range(0, asList.Count))
        {
            foreach (var j in Enumerable.Range(i + 1, asList.Count - i - 1))
            {
                yield return (asList[i], asList[j]);
            }
        }
    }
}
