namespace AdventOfCode.Y2023.Day15;

[ProblemName("Lens Library")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        return input.Split(',')
            .Select(CalculateHashValue)
            .Sum();
    }

    public object PartTwo(string input)
    {
        Dictionary<int, Box> boxes = [];
        foreach (var instruction in input.Split(','))
        {
            var indexOfOperation = instruction.IndexOfAny(['=', '-']);
            var boxNr = CalculateHashValue(instruction[..indexOfOperation]);
            var box = boxes.TryGetValue(boxNr, out var receivedBox) ? receivedBox : boxes[boxNr] = new Box() { Id = boxNr };

            switch (instruction[indexOfOperation])
            {
                case '=':
                    var lens = box.FirstOrDefault(l => instruction.AsSpan(0, indexOfOperation).Equals(l.Label, StringComparison.Ordinal));
                    if (lens != null)
                    {
                        lens.FocalStrength = long.Parse(instruction.AsSpan(indexOfOperation + 1));
                    }
                    else
                    {
                        box.Add(new Lens() { Label = string.Join("", instruction.Take(indexOfOperation)), FocalStrength = long.Parse(instruction.AsSpan(indexOfOperation + 1)) });
                    }
                    break;
                case '-':
                    var lensIdx = box.FindIndex(l => instruction.AsSpan(0, indexOfOperation).Equals(l.Label, StringComparison.Ordinal));
                    if (lensIdx != -1)
                    {
                        box.RemoveAt(lensIdx);
                    }
                    break;
                default:
                    throw new InvalidOperationException("Invalid operator");
            }
        }

        return boxes.Values.Sum(b => b.FocusingPower);
    }

    private int CalculateHashValue(string hash)
    {
        return hash
            .ToCharArray()
            .Aggregate(0, (acc, ch) => (acc + ch) * 17 % 256);
    }

    private class Lens
    {
        public string Label { get; set; }
        public long FocalStrength { get; set; }
    }

    private class Box : List<Lens>
    {
        public int Id { get; set; }
        public long FocusingPower
        {
            get
            {
                long result = 0;
                var idx = 1;
                foreach (var l in this)
                {
                    result += idx++ * l.FocalStrength;
                }
                return result * (((long)Id) + 1);
            }
        }
    }
}
