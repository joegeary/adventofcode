using AdventOfCode.Models;

namespace AdventOfCode.Generators;

internal class YearReadmeGenerator
{
    public string Generate(Calendar calendar)
    {
        return $"""
                # Advent of Code ({calendar.Year})
                Check out https://adventofcode.com/{calendar.Year}.

                <a href="https://adventofcode.com/{calendar.Year}"><img src="calendar.svg" width="80%" /></a>
                """;
    }
}
