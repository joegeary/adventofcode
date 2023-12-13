namespace AdventOfCode.Y2023.Day12;

[ProblemName("Hot Springs")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        return input.Split('\n')
            .Select(SpringRecord.Parse)
            .Sum(p => p.GetArrangements());
    }

    public object PartTwo(string input)
    {
        return input.Split('\n')
            .Select(RepeatLine)
            .Select(SpringRecord.Parse)
            .Sum(p => p.GetArrangements());
    }

    private string RepeatLine(string line)
    {
        var parts = line.Split(' ');
        return $"{parts[0]}?{parts[0]}?{parts[0]}?{parts[0]}?{parts[0]} {parts[1]},{parts[1]},{parts[1]},{parts[1]},{parts[1]}";
    }

    private record SpringRecord(string Row, int[] Groups, int Sum)
    {
        private readonly Dictionary<string, long> _cache = [];

        public static SpringRecord Parse(string line)
        {
            var parts = line.Split(' ');
            var row = parts[0].Trim('.');
            var groups = parts[1].Split(',').Select(int.Parse).ToArray();

            return new SpringRecord(row, groups, groups.Sum());
        }

        public long GetArrangements(string row = null, int[] groups = null, int? sum = null)
        {
            row ??= Row;
            groups ??= Groups;
            sum ??= Sum;

            var key = row + string.Join(',', groups);

            if (_cache.TryGetValue(key, out var answer))
            {
                return answer;
            }

            answer = CalculateArrangements(row, groups, sum.Value);
            _cache.Add(key, answer);
            return answer;
        }

        private long CalculateArrangements(string row, int[] groups, int sum)
        {
            row = row.TrimStart('.');

            var length = row.Length;

            var groupLength = groups.Length;

            if (length == 0)
            {
                return groupLength == 0 ? 1 : 0;
            }

            if (groupLength == 0)
            {
                for (var i = 0; i < length; i++)
                {
                    if (row[i] == '#')
                    {
                        return 0;
                    }
                }

                return 1;
            }

            var count = 0;

            for (var i = 0; i < length; i++)
            {
                if (row[i] != '.')
                {
                    count++;
                }
            }

            if (sum > count)
            {
                return 0;
            }

            if (row[0] == '#')
            {
                var group = groups[0];

                if (length < group)
                {
                    return 0;
                }

                for (var i = 0; i < group; i++)
                {
                    if (row[i] == '.')
                    {
                        return 0;
                    }
                }

                if (length == group)
                {
                    return groupLength == 1 ? 1 : 0;
                }

                if (row[group] == '#')
                {
                    return 0;
                }

                return GetArrangements(row[(group + 1)..], groups[1..], sum - groups[0]);
            }

            return GetArrangements($"#{row[1..]}", groups, sum) + GetArrangements(row[1..], groups, sum);
        }
    }
}
