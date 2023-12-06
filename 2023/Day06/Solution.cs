using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day06;

[ProblemName("Wait For It")]
internal class Solution : ISolver
{

    public object PartOne(string input)
    {
        return ParseGames(input)
         .Select(CalcWinners)
         .Aggregate((acc, x) => acc * x);
    }

    public object PartTwo(string input)
    {
        return ParseGames(input.Replace(" ", ""))
            .Select(CalcWinners)
            .First();
    }

    private IEnumerable<Game> ParseGames(string input)
    {
        var lines = input.Split('\n');
        var times = Regex.Matches(lines[0], @"\d+").Select(g => int.Parse(g.Value));
        var distances = Regex.Matches(lines[1], @"\d+").Select(g => long.Parse(g.Value));

        return times.Select((t, i) => new Game(t, distances.ElementAt(i)));
    }

    private long CalcWinners(Game game)
    {
        long lowestChargeTime = 0;
        long highestChargeTime = game.Time / 2;
        long lastChargeTime = 0;

        while (true)
        {
            long chargeTime = lowestChargeTime + (highestChargeTime - lowestChargeTime) / 2;

            if (chargeTime == lastChargeTime)
                break;

            lastChargeTime = chargeTime;

            var distance = chargeTime * (game.Time - chargeTime);
            if (distance > game.Distance)
                highestChargeTime = chargeTime + 1;
            else if (distance < game.Distance)
                lowestChargeTime = chargeTime + 1;
        }

        return game.Time - lastChargeTime + 1 - lastChargeTime;
    }

    private record Game(int Time, long Distance);
}
