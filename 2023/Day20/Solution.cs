namespace AdventOfCode.Y2023.Day20;

[ProblemName("Pulse Propagation")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var modules = Module.Parse(input);
        var (lows, highs) = Enumerable.Range(0, 1000)
            .Select(_ => SendPulses(modules))
            .Aggregate((0, 0), (acc, cur) => (acc.Item1 + cur.lows, acc.Item2 + cur.highs));
        return lows * highs;
    }

    public object PartTwo(string input)
    {
        var modules = Module.Parse(input);
        var iterations = new List<long>();

        foreach (var conjunction in GetAllConjuctions(modules))
        {
            modules = Module.Parse(input);

            var presses = 0;
            while (true)
            {
                presses++;

                var result = SendPulses(modules, conjunction);
                if (result == (0, 0))
                {
                    iterations.Add(presses);
                    break;
                }
            }
        }

        return Lcm(iterations);
    }

    public (int lows, int highs) SendPulses(Dictionary<string, Module> modules, string checkModule = null)
    {
        var lows = 0;
        var highs = 0;
        var queue = new Queue<(string Source, bool Pulse, string Target)>();

        queue.Enqueue(("button", false, "broadcaster"));

        while (queue.TryDequeue(out var pulse))
        {
            if (checkModule != null && !pulse.Pulse && pulse.Target == checkModule)
            {
                return (0, 0);
            }

            if (pulse.Pulse)
            {
                highs++;
            }
            else
            {
                lows++;
            }

            if (!modules.TryGetValue(pulse.Target, out var module))
            {
                continue;
            }

            switch (module.Type)
            {
                case Type.Broadcast:
                    module.State = pulse.Pulse;
                    break;

                case Type.FlipFlop:
                    if (!pulse.Pulse)
                    {
                        module.State = !module.State;
                    }
                    break;

                case Type.Conjunction:
                    module.ReceivedPulses[pulse.Source] = pulse.Pulse;
                    break;
            }

            foreach (var target in module.Targets)
            {
                if (pulse.Pulse && module.Type == Type.FlipFlop)
                {
                    continue;
                }

                var output = false;
                switch (module.Type)
                {
                    case Type.Broadcast:
                        output = module.State;
                        break;

                    case Type.FlipFlop:
                        if (!pulse.Pulse)
                        {
                            output = module.State;
                        }
                        break;

                    case Type.Conjunction:
                        output = !module.ReceivedPulses.All(r => r.Value);
                        break;
                }

                queue.Enqueue((pulse.Target, output, target));
            }
        }

        return (lows, highs);
    }

    public IEnumerable<string> GetAllConjuctions(Dictionary<string, Module> modules)
    {
        var list = new List<string>();

        foreach (var resets in modules.Where(m => m.Value.Targets.Contains("rx")))
        {
            foreach (var m in modules)
            {
                if (m.Value.Targets.Contains(resets.Key))
                {
                    list.Add(m.Key);
                }
            }
        }

        return list;
    }

    private static long Lcm(IEnumerable<long> numbers) => numbers.Select(Convert.ToInt64).Aggregate((a, b) => a * b / Gcd(a, b));
    private static long Gcd(long a, long b) => b == 0 ? a : Gcd(b, a % b);

    public enum Type
    {
        Broadcast = 0,
        FlipFlop = 1,
        Conjunction = 2
    }

    public class Module
    {
        public Type Type { get; set; }
        public bool State { get; set; }
        public IList<string> Targets { get; set; }
        public IDictionary<string, bool> ReceivedPulses { get; set; }

        public static Dictionary<string, Module> Parse(string input)
        {
            var modules = new Dictionary<string, Module>();

            foreach (var line in input.Split('\n'))
            {
                var parts = line.Split("->", StringSplitOptions.TrimEntries);

                Module module;
                string name;

                switch (parts[0][0])
                {
                    case 'b':
                        module = new Module
                        {
                            Type = Type.Broadcast
                        };
                        name = parts[0];
                        break;

                    case '%':
                        module = new Module
                        {
                            Type = Type.FlipFlop
                        };
                        name = parts[0][1..];
                        break;

                    default:
                        module = new Module
                        {
                            Type = Type.Conjunction
                        };
                        name = parts[0][1..];
                        break;
                }

                module.Targets = parts[1].Split(',', StringSplitOptions.TrimEntries).ToList();

                if (module.Type == Type.Conjunction)
                {
                    module.ReceivedPulses = new Dictionary<string, bool>();
                }

                modules.Add(name, module);
            }

            foreach (var module in modules)
            {
                foreach (var target in module.Value.Targets)
                {
                    if (modules.TryGetValue(target, out var targetModule) && targetModule.Type == Type.Conjunction)
                    {
                        targetModule.ReceivedPulses[module.Key] = false;
                    }
                }
            }

            return modules;
        }
    }
}
