namespace AdventOfCode.Generators;

internal class ProjectReadmeGenerator
{
    public string Generate(int firstYear, int lastYear)
    {
        return $"""
                # Advent of Code ({firstYear}-{lastYear})
                C# solutions to the Advent of Code problems.
                Check out https://adventofcode.com

                <a href="https://adventofcode.com"><img src="{lastYear}/calendar.svg" width="80%" /></a>
                """;
    }
}
