namespace AdventOfCode.Y2023.Day08;

[ProblemName("Haunted Wasteland")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var lines = input.Split('\n');
        var instructions = lines[0];
        var nodes = ParseNodes(lines.Skip(2));

        var currNode = "AAA";
        var steps = 0;

        while (currNode != "ZZZ")
        {
            var direction = instructions[steps % instructions.Length];
            currNode = nodes[currNode][direction];
            steps++;
        }

        return steps;
    }

    public object PartTwo(string input)
    {
        var lines = input.Split('\n');
        var instructions = lines[0];
        var nodes = ParseNodes(lines.Skip(2));

        var walkLengths = new List<long>();
        foreach (var n in nodes.Keys.Where(k => k.EndsWith('A')))
        {
            var steps = 0L;
            var currNode = n;

            while (!currNode.EndsWith('Z'))
            {
                var direction = instructions[(int)steps % instructions.Length];
                currNode = nodes[currNode][direction];
                steps++;
            }

            walkLengths.Add(steps);
        }

        return walkLengths.Aggregate(1L, FindLCM);
    }

    public static long FindLCM(long a, long b) => a * b / FindGCD(a, b);

    public static long FindGCD(long a, long b)
    {
        if (a == 0 || b == 0) return Math.Max(a, b);
        return (a % b == 0) ? b : FindGCD(b, a % b);
    }

    private Dictionary<string, Dictionary<char, string>> ParseNodes(IEnumerable<string> lines)
    {
        var nodes = new Dictionary<string, Dictionary<char, string>>();
        foreach (var line in lines)
        {
            nodes.Add(line[..3], new Dictionary<char, string>()
            {
                ['L'] = line.Substring(7, 3),
                ['R'] = line.Substring(12, 3)
            });
        }
        return nodes;
    }
}
