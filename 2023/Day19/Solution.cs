namespace AdventOfCode.Y2023.Day19;

[ProblemName("Aplenty")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var tokens = input.Split("\n\n");
        var workflows = tokens[0].Split('\n').Select(Workflow.Parse);
        var parts = tokens[1].Split('\n').Select(Part.Parse);

        return parts.Sum(p => p.Process(workflows));
    }

    public object PartTwo(string input)
    {
        var tokens = input.Split("\n\n");
        var workflows = tokens[0].Split('\n').Select(Workflow.Parse);
        var parts = tokens[1].Split('\n').Select(Part.Parse);

        var candidates = new Dictionary<char, (int min, int max)>
        {
            ['x'] = (1, 4000),
            ['m'] = (1, 4000),
            ['a'] = (1, 4000),
            ['s'] = (1, 4000),
        };

        return ProcessRanges("in", candidates, workflows);
    }

    public long ProcessRanges(string pos, Dictionary<char, (int min, int max)> ranges, IEnumerable<Workflow> workflows)
    {
        switch (pos)
        {
            case "A":
                return ranges.Values
                    .Aggregate<(int Min, int Max), long>(1, (current, range) => current * (range.Max - range.Min + 1));
            case "R":
                return 0;
        }

        long result = 0;
        var workflow = workflows.First(w => w.Name == pos);

        foreach (var rule in workflow.Rules)
        {
            var (min, max) = rule.Property == '\0' ? (0, 0) : ranges[rule.Property];

            switch (rule.Condition)
            {
                case '<':
                    if (max < rule.Value)
                    {
                        result += ProcessRanges(rule.Destination, ranges, workflows);
                        return result;
                    }

                    if (min < rule.Value)
                    {
                        var newRanges = new Dictionary<char, (int Min, int Max)>(ranges)
                        {
                            [rule.Property] = (min, rule.Value - 1)
                        };
                        result += ProcessRanges(rule.Destination, newRanges, workflows);

                        ranges[rule.Property] = (rule.Value, max);
                    }
                    break;
                case '>':
                    if (min > rule.Value)
                    {
                        result += ProcessRanges(rule.Destination, ranges, workflows);
                        return result;
                    }

                    if (max > rule.Value)
                    {
                        var newRanges = new Dictionary<char, (int Min, int Max)>(ranges)
                        {
                            [rule.Property] = (rule.Value + 1, max)
                        };
                        result += ProcessRanges(rule.Destination, newRanges, workflows);

                        ranges[rule.Property] = (min, rule.Value);
                    }
                    break;
                default:
                    result += ProcessRanges(rule.Destination, ranges, workflows);
                    break;
            }
        }

        return result;
    }

    public record Workflow(string Name, IEnumerable<Rule> Rules)
    {
        public static Workflow Parse(string input)
        {
            var tokens = input.Split('{');
            var rules = tokens[1][0..^1].Split(',');

            return new Workflow(tokens[0], rules.Select(Rule.Parse));
        }
    }

    public record Rule(string Destination, char Property = '\0', char Condition = '\0', int Value = 0)
    {
        public static Rule Parse(string input)
        {
            var tokens = input.Split('<', '>');

            if (tokens.Length != 2)
            {
                return new Rule(input);
            }

            var tokens2 = tokens[1].Split(':');
            return new Rule(tokens2[1], input[0], input[1], int.Parse(tokens2[0]));
        }
    }

    public record Part(int X, int M, int A, int S)
    {
        public static Part Parse(string input)
        {
            var tokens = input[1..^1].Split(',');
            return new Part(int.Parse(tokens[0].Split('=')[1]), int.Parse(tokens[1].Split('=')[1]), int.Parse(tokens[2].Split('=')[1]), int.Parse(tokens[3].Split('=')[1]));
        }

        public int Process(IEnumerable<Workflow> workflows)
        {
            var name = "in";

            while (name is not ("A" or "R"))
            {
                var workflow = workflows.First(w => w.Name == name);

                foreach (var rule in workflow.Rules)
                {
                    if (rule.Condition == '\0')
                    {
                        name = rule.Destination;
                        break;
                    }

                    var property = rule.Property switch
                    {
                        'x' => X,
                        'm' => M,
                        'a' => A,
                        _ => S
                    };

                    var pass = rule.Condition == '<' ? property < rule.Value : property > rule.Value;
                    if (pass)
                    {
                        name = rule.Destination;
                        break;
                    }
                }
            }

            return name == "A" ? X + M + A + S : 0;
        }
    }
}
