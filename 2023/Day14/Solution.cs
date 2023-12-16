namespace AdventOfCode.Y2023.Day14;

[ProblemName("Parabolic Reflector Dish")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var platform = Platform.Parse(input);
        platform.TiltNorth();
        return platform.Score;
    }

    public object PartTwo(string input)
    {
        var platform = Platform.Parse(input);

        var cache = new Dictionary<string, int>();
        var cycle = 1;

        while (cycle <= 1_000_000_000)
        {
            platform.TiltNorth();
            platform.TiltWest();
            platform.TiltSouth();
            platform.TiltEast();

            var current = string.Join(string.Empty, platform.Map.SelectMany(c => c));

            if (cache.TryGetValue(current, out var cached))
            {
                var remaining = 1_000_000_000 - cycle - 1;
                var loop = cycle - cached;

                var loopRemaining = remaining % loop;
                cycle = 1_000_000_000 - loopRemaining - 1;
            }

            cache[current] = cycle++;
        }

        return platform.Score;
    }

    public record Platform(char[][] Map)
    {
        public static Platform Parse(string input)
        {
            var map = input.Split('\n').Select(s => s.ToCharArray()).ToArray();
            return new Platform(map);
        }

        public int Score
        {
            get
            {
                var score = 0;
                for (var i = Map.Length; i > 0; i--)
                {
                    score += Map[^i].Count(c => c == 'O') * i;
                }

                return score;
            }
        }
        public void TiltNorth()
        {
            for (var row = 1; row < Map.Length; row++)
            {
                for (var col = 0; col < Map[row].Length; col++)
                {
                    var c = Map[row][col];

                    if (c != 'O')
                    {
                        continue;
                    }

                    var previous = 1;
                    while (Map[row - previous][col] == '.')
                    {
                        Map[row - previous][col] = 'O';
                        Map[row - previous + 1][col] = '.';
                        previous++;

                        if (row - previous < 0)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void TiltSouth()
        {
            for (var row = Map.Length - 2; row >= 0; row--)
            {
                for (var col = 0; col < Map[row].Length; col++)
                {
                    var c = Map[row][col];

                    if (c != 'O')
                    {
                        continue;
                    }

                    var previous = 1;
                    while (Map[row + previous][col] == '.')
                    {
                        Map[row + previous][col] = 'O';
                        Map[row + previous - 1][col] = '.';
                        previous++;

                        if (row + previous >= Map.Length)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void TiltWest()
        {
            for (var row = 0; row < Map.Length; row++)
            {
                for (var col = 1; col < Map[row].Length; col++)
                {
                    var c = Map[row][col];

                    if (c != 'O')
                    {
                        continue;
                    }

                    var previous = 1;
                    while (Map[row][col - previous] == '.')
                    {
                        Map[row][col - previous] = 'O';
                        Map[row][col - previous + 1] = '.';
                        previous++;

                        if (col - previous < 0)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void TiltEast()
        {
            for (var row = 0; row < Map.Length; row++)
            {
                for (var col = Map[row].Length - 2; col >= 0; col--)
                {
                    var c = Map[row][col];

                    if (c != 'O')
                    {
                        continue;
                    }

                    var previous = 1;
                    while (Map[row][col + previous] == '.')
                    {
                        Map[row][col + previous] = 'O';
                        Map[row][col + previous - 1] = '.';
                        previous++;

                        if (col + previous >= Map[row].Length)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
