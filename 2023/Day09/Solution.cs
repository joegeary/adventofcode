namespace AdventOfCode.Y2023.Day09;

[ProblemName("Mirage Maintenance")]
internal class Solution : ISolver
{

    public object PartOne(string input)
    {
        return input.Split("\n")
            .Select(line => line.Split(' ').Select(long.Parse).ToArray())
            .Select(Predict)
            .Sum();
    }

    public object PartTwo(string input)
    {
        return input.Split("\n")
            .Select(line => line.Split(' ').Select(long.Parse).ToArray())
            .Select(PredictPrevious)
            .Sum();
    }

    private long Predict(long[] array)
    {
        return array.Any(x => x != 0)
            ? Predict(array.Zip(array.Skip(1), (a, b) => b - a).ToArray()) + array[^1]
            : 0;
    }

    private long PredictPrevious(long[] array)
    {
        return array.Any(x => x != 0)
            ? array[0] - PredictPrevious(array.Zip(array.Skip(1), (a, b) => b - a).ToArray())
            : 0;
    }
}
