namespace AdventOfCode.Y2023.Day17;

[ProblemName("Clumsy Crucible")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        return CityMap.Parse(input).FindShortestPath(1, 3);
    }

    public object PartTwo(string input)
    {
        return CityMap.Parse(input).FindShortestPath(4, 10);
    }

    public record CityMap(int[][] Blocks)
    {
        public static CityMap Parse(string input)
        {
            return new CityMap(
                input.Split('\n')
                    .Select(t => t.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray())
                    .ToArray()
                );
        }

        private readonly int[][] Directions = [[0, 1], [1, 0], [0, -1], [-1, 0]];

        private bool IsPositionInRange((int, int) position, int[][] array)
        {
            return position.Item1 >= 0 && position.Item1 < array.Length && position.Item2 >= 0 &&
                   position.Item2 < array[0].Length;
        }

        public int FindShortestPath(int minDistance, int maxDistance)
        {
            var queue = new List<(int, int, int, int)>
            {
                (0, 0, 0, -1)
            };

            var seen = new HashSet<(int, int, int)>();
            var costs = new Dictionary<(int, int, int), int>();

            while (queue.Count > 0)
            {
                queue.Sort((a, b) => a.Item1.CompareTo(b.Item1));
                var (cost, x, y, prevDirection) = queue[0];
                queue.RemoveAt(0);

                if (x == Blocks.Length - 1 && y == Blocks[0].Length - 1) return cost;

                if (seen.Contains((x, y, prevDirection))) continue;

                seen.Add((x, y, prevDirection));

                for (var direction = 0; direction < 4; direction++)
                {
                    var costIncrease = 0;

                    if (direction == prevDirection || (direction + 2) % 4 == prevDirection) continue;

                    for (var distance = 1; distance <= maxDistance; distance++)
                    {
                        var newX = x + (Directions[direction][0] * distance);
                        var newY = y + (Directions[direction][1] * distance);

                        if (IsPositionInRange((newX, newY), Blocks))
                        {
                            costIncrease += Blocks[newX][newY];

                            if (distance < minDistance) continue;

                            var newCost = cost + costIncrease;

                            if (costs.TryGetValue((newX, newY, direction), out var existingCost) &&
                                existingCost <= newCost)
                            {
                                continue;
                            }

                            costs[(newX, newY, direction)] = newCost;
                            queue.Add((newCost, newX, newY, direction));
                        }
                    }
                }
            }

            return -1;
        }
    }
}
