using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day01;

[ProblemName("Trebuchet?!")]
internal class Solution : ISolver
{

    public object PartOne(string input) =>
        Solve(input, @"\d");

    public object PartTwo(string input) =>
        Solve(input, @"\d|one|two|three|four|five|six|seven|eight|nine");

    private int Solve(string input, string rx) => (
        from line in input.Split("\n")
        let first = Regex.Match(line, rx)
        let last = Regex.Match(line, rx, RegexOptions.RightToLeft)
        select (ParseMatch(first.Value) * 10) + ParseMatch(last.Value)
    ).Sum();

    private int ParseMatch(string input) =>
        input switch
        {
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            var d => int.Parse(d)
        };
}
