namespace AdventOfCode.Y2023.Day13;

[ProblemName("Point of Incidence")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        return input.Split("\n\n")
            .Select(Pattern.FromString)
            .Select(p => p.ReflectionNumber(0))
            .Sum();
    }

    public object PartTwo(string input)
    {
        return input.Split("\n\n")
            .Select(Pattern.FromString)
            .Select(p => p.ReflectionNumber(1))
            .Sum();
    }

    public record Pattern(char[][] Input)
    {
        public int Width => Input[0].Length;
        public int Height => Input.Length;

        public List<char> Row(int index) => Input[index].ToList();
        public IEnumerable<List<char>> Rows() => Input.Select(row => row.ToList());
        public List<char> Column(int index) => Input.Select(row => row[index]).ToList();
        public IEnumerable<List<char>> Columns() => Enumerable.Range(0, Width).Select(Column);

        public static Pattern FromString(string input)
        {
            return new Pattern(
                        input
                            .Split('\n')
                            .Select(l => l.ToCharArray()).ToArray()
                    );
        }

        public int IsReflected(List<char> symbols, int index)
        {
            return Enumerable.Reverse(symbols[0..index])
                .Zip(symbols[index..^0])
                .Count(item => item.First != item.Second);
        }

        public long ReflectionNumber(int target = 0)
        {
            var result = Enumerable.Range(1, Width - 1)
                .Where(index => Rows()
                    .Select(row => IsReflected(row, index))
                    .Sum() == target
                )
                .FirstOrDefault(0);

            if (result > 0)
                return result;

            return 100 * Enumerable.Range(1, Height - 1)
                .Where(index => Columns()
                    .Select(col => IsReflected(col, index))
                    .Sum() == target
                )
                .FirstOrDefault(0);
        }
    }
}
