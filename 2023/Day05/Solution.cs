namespace AdventOfCode.Y2023.Day05;

[ProblemName("If You Give A Seed A Fertilizer")]
internal class Solution : ISolver
{

    public object PartOne(string input)
    {
        var lines = input.Split("\n\n");
        var seeds = lines[0].Split(": ")[1].Split(' ').Select(long.Parse);
        var maps = lines.Skip(1).Select(ParseMap);

        return seeds.Select(s => SeedToLocation(s, maps)).Min();
    }

    public object PartTwo(string input)
    {
        var lines = input.Split("\n\n");
        var seeds = ParseSeeds(lines[0]);
        var maps = lines.Skip(1).Select(ParseMap).Reverse();

        // what is time but a construct?

        var location = 0;
        while (true)
        {
            var seed = LocationToSeed(location, maps);
            if (seeds.Any(s => s.ContainsKey(seed)))
            {
                return location;
            }

            location++;
        }
    }

    private List<Range> ParseSeeds(string seedStr)
    {
        var numbers = seedStr.Split(": ")[1].Split(' ').Select(long.Parse);
        var seeds = new List<Range>();
        for (var i = 0; i < numbers.Count(); i += 2)
        {
            seeds.Add(new Range(0, numbers.ElementAt(i), numbers.ElementAt(i + 1)));
        }
        return seeds;
    }

    private IEnumerable<Range> ParseMap(string mapStr)
    {
        return mapStr.Split('\n')
            .Skip(1)
            .Select(l =>
            {
                var tokens = l.Split(' ').Select(long.Parse).ToArray();
                return new Range(tokens[0], tokens[1], tokens[2]);
            });
    }

    private long SeedToLocation(long seed, IEnumerable<IEnumerable<Range>> maps)
    {
        var location = seed;
        foreach (var map in maps)
        {
            var range = map.Where(r => r.ContainsKey(location));
            location = !range.Any() ? location : range.ElementAt(0).GetValue(location);
        }
        return location;
    }

    private long LocationToSeed(long location, IEnumerable<IEnumerable<Range>> maps)
    {
        var seed = location;
        foreach (var map in maps)
        {
            var range = map.Where(r => r.ContainsValue(seed));
            seed = !range.Any() ? seed : range.ElementAt(0).GetKey(seed);
        }
        return seed;
    }

    private record Range(long Dest, long Src, long Length)
    {
        public bool ContainsKey(long key)
        {
            return key >= Src && key <= Src + Length;
        }

        public bool ContainsValue(long value)
        {
            return value >= Dest && value <= Dest + Length;
        }

        public long GetKey(long value)
        {
            return Src + (value - Dest);
        }

        public long GetValue(long key)
        {
            return Dest + (key - Src);
        }
    }

}
