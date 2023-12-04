using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day04;

[ProblemName("Scratchcards")]
internal class Solution : ISolver
{

    public object PartOne(string input)
    {
        return input.Split('\n')
            .Select(ParseCard)
            .Select(c => c.Score)
            .Sum();
    }

    public object PartTwo(string input)
    {
        var cards = input.Split('\n')
            .Select(ParseCard)
            .ToArray();

        var games = cards.ToDictionary(c => c.Id, _ => 1);

        for (var i = 0; i < games.Count; i++)
        {
            foreach (var copy in cards.Skip(i + 1).Take(cards[i].Matches.Length))
            {
                games[copy.Id] += games[cards[i].Id];
            }
        }

        return games.Sum(c => c.Value);
    }

    private Card ParseCard(string line)
    {
        var tokens = line.Split('|', ':');
        var id = int.Parse(Regex.Match(tokens[0], @"\d+").Value);
        var winners = Regex.Matches(tokens[1], @"\d+").Select(g => int.Parse(g.Value));
        var numbers = Regex.Matches(tokens[2], @"\d+").Select(g => int.Parse(g.Value));
        return new Card(id, winners.ToArray(), numbers.ToArray());
    }

    private record Card(int Id, int[] WinningNumbers, int[] Numbers)
    {
        public int[] Matches => WinningNumbers.Intersect(Numbers).ToArray();
        public int Score => Matches.Length > 0 ? (int)Math.Pow(2, Matches.Length - 1) : 0;
    };
}
