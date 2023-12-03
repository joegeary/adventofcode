using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day02;

[ProblemName("Cube Conundrum")]
internal class Solution : ISolver
{

    public object PartOne(string input)
    {
        return input.Split('\n')
            .Select(ParseGame)
            .Where(g => g.Red <= 12 && g.Green <= 13 && g.Blue <= 14)
            .Select(x => x.Id)
            .Sum();
    }

    public object PartTwo(string input)
    {
        return input.Split('\n')
            .Select(ParseGame)
            .Select(x => x.Red * x.Green * x.Blue)
            .Sum();
    }

    private Game ParseGame(string line)
    {
        return new Game(
            ParseInts(line, @"Game (\d+)").First(),
            ParseInts(line, @"(\d+) red").Max(),
            ParseInts(line, @"(\d+) green").Max(),
            ParseInts(line, @"(\d+) blue").Max()
        );
    }

    private IEnumerable<int> ParseInts(string game, string regex)
    {
        return Regex.Matches(game, regex)
            .Select(x => int.Parse(x.Groups[1].Value));
    }

    private record Game(int Id, int Red, int Green, int Blue);
}
