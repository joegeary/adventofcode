namespace AdventOfCode.Y2023.Day16;

[ProblemName("The Floor Will Be Lava")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        return Contraption.Parse(input).Energize(0, -1, Direction.Right);
    }

    public object PartTwo(string input)
    {
        var contraption = Contraption.Parse(input);
        var result = 0;

        for (var j = 0; j < contraption.Width; j++)
        {
            result = Math.Max(contraption.Energize(-1, j, Direction.Down), result);
            result = Math.Max(contraption.Energize(contraption.Height, j, Direction.Up), result);
        }

        for (var i = 0; i < contraption.Height; i++)
        {
            result = Math.Max(contraption.Energize(i, -1, Direction.Right), result);
            result = Math.Max(contraption.Energize(i, contraption.Width, Direction.Left), result);
        }

        return result;
    }

    public enum Direction
    {
        Down = 'D',
        Left = 'L',
        Right = 'R',
        Up = 'U',
    }

    public static readonly Dictionary<Direction, (int x, int y)> offsets = new()
    {
        { Direction.Up, (-1, 0) },
        { Direction.Down, (1, 0) },
        { Direction.Left, (0, -1) },
        { Direction.Right, (0, 1) },
    };

    public static readonly Dictionary<Direction, Dictionary<char, Direction>> reflections = new()
    {
        { Direction.Up, new () {{'/', Direction.Right}, {'\\', Direction.Left}} },
        { Direction.Down, new () {{'/', Direction.Left}, {'\\', Direction.Right}} },
        { Direction.Left, new () {{'/', Direction.Down}, {'\\', Direction.Up}} },
        { Direction.Right, new () {{'/', Direction.Up}, {'\\', Direction.Down}} },
    };

    public record Contraption(char[][] Tiles)
    {
        public int Height => Tiles.Length;
        public int Width => Tiles[0].Length;

        public static Contraption Parse(string input)
        {
            return new Contraption(
                input.Split('\n')
                    .Select(l => l.ToCharArray()).ToArray()
            );
        }

        public int Energize(int x, int y, Direction direction)
        {
            var beams = new List<(int x, int y, Direction direction)> { (x, y, direction) };
            var energized = new HashSet<(int x, int y)>();
            var seen = new HashSet<(int x, int y, Direction direction)>();

            while (beams.Count > 0)
            {
                var beam = beams[^1];
                beams.RemoveAt(beams.Count - 1);

                var i = beam.x + offsets[beam.direction].x;
                var j = beam.y + offsets[beam.direction].y;
                if (i >= Height || j >= Width || i < 0 || j < 0 || seen.Contains(beam)) continue;

                seen.Add(beam);
                energized.Add((i, j));

                if (Tiles[i][j] == '.')
                {
                    beam = (i, j, beam.direction);
                    beams.Add(beam);
                    continue;
                }

                foreach (var d in DeflectBeam(beam, Tiles[i][j]))
                {
                    beam = (i, j, d);
                    beams.Add(beam);
                }
            }

            return energized.Count;
        }

        private List<Direction> DeflectBeam((int x, int y, Direction direction) beam, char ch)
        {
            var beams = new List<Direction>();

            if (ch == '-')
            {
                beams.Add(beam.direction is Direction.Right or Direction.Left ? beam.direction : Direction.Right);
                beams.Add(beam.direction is Direction.Right or Direction.Left ? beam.direction : Direction.Left);
            }
            else if (ch == '|')
            {
                beams.Add(beam.direction is Direction.Up or Direction.Down ? beam.direction : Direction.Up);
                beams.Add(beam.direction is Direction.Up or Direction.Down ? beam.direction : Direction.Down);
            }
            else
            {
                beams.Add(reflections[beam.direction][ch]);
            }

            return beams;
        }
    }
}
