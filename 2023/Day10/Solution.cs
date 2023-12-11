namespace AdventOfCode.Y2023.Day10;

[ProblemName("Pipe Maze")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var map = input.Split('\n').ToList();
        var loop = CalcLoop(map);
        return Math.Round((double)loop.Count / 2);
    }

    public object PartTwo(string input)
    {
        var map = input.Split('\n').ToList();
        var loop = CalcLoop(map);

        var enclosed = 0;

        for (var x = 0; x < map.Count; x++)
        {
            var isInside = false;

            for (var y = 0; y < map[x].Length; y++)
            {
                if (loop.Any(t => t.X == x && t.Y == y && (t.Pipe is '7' or 'F' or '|')))
                {
                    isInside = !isInside;
                }

                if (isInside && !loop.Any(t => t.X == x && t.Y == y))
                {
                    enclosed++;
                }
            }
        }

        return enclosed;
    }

    private List<Tile> CalcLoop(List<string> map)
    {
        var (startX, startY, dir) = GetStartCoord(map);
        var x = startX;
        var y = startY;

        var tiles = new List<Tile>();

        while (true)
        {
            (x, y, dir) = GetNextCoord(map, x, y, dir);
            tiles.Add(new Tile(x, y, map[x][y]));

            if (x == startX && y == startY)
                break;
        }

        return tiles;
    }

    private (int x, int y, Direction dir) GetStartCoord(List<string> map)
    {
        var x = map.FindIndex(m => m.Contains('S'));
        var y = map[x].IndexOf('S');

        var (ch, dir) = GetStartNextCoord(map, x, y);
        map[x] = map[x].Remove(y, 1).Insert(y, ch.ToString());

        return (x, y, dir);
    }

    private (int, int, Direction) GetNextCoord(List<string> map, int x, int y, Direction dir)
    {
        return map[x][y] switch
        {
            '|' => dir switch
            {
                Direction.Up => (x - 1, y, Direction.Up),
                Direction.Down => (x + 1, y, Direction.Down),
                _ => throw new InvalidOperationException("???")
            },
            '-' => dir switch
            {
                Direction.Left => (x, y - 1, Direction.Left),
                Direction.Right => (x, y + 1, Direction.Right),
                _ => throw new InvalidOperationException("???")
            },
            '7' => dir switch
            {
                Direction.Up => (x, y - 1, Direction.Left),
                Direction.Right => (x + 1, y, Direction.Down),
                _ => throw new InvalidOperationException("???")
            },
            'F' => dir switch
            {
                Direction.Up => (x, y + 1, Direction.Right),
                Direction.Left => (x + 1, y, Direction.Down),
                _ => throw new InvalidOperationException("???")
            },
            'L' => dir switch
            {
                Direction.Down => (x, y + 1, Direction.Right),
                Direction.Left => (x - 1, y, Direction.Up),
                _ => throw new InvalidOperationException("???")
            },
            'J' => dir switch
            {
                Direction.Down => (x, y - 1, Direction.Left),
                Direction.Right => (x - 1, y, Direction.Up),
                _ => throw new InvalidOperationException("???")
            },
            _ => throw new InvalidOperationException("???")
        };
    }

    private (char, Direction) GetStartNextCoord(List<string> map, int x, int y)
    {
        var directions = new List<Direction>();

        if (x > 0 && map[x - 1][y] is '|' or '7' or 'F')
        {
            directions.Add(Direction.Up);
        }
        if (x < map.Count - 1 && map[x + 1][y] is '|' or 'J' or 'L')
        {
            directions.Add(Direction.Down);
        }
        if (y > 0 && map[x][y - 1] is '-' or 'L' or 'F')
        {
            directions.Add(Direction.Left);
        }
        if (y < map[x].Length - 1 && map[x][y + 1] is '-' or '7' or 'J')
        {
            directions.Add(Direction.Right);
        }

        if (directions.Contains(Direction.Up) && directions.Contains(Direction.Down))
        {
            return ('|', Direction.Up);
        }
        if (directions.Contains(Direction.Left) && directions.Contains(Direction.Right))
        {
            return ('-', Direction.Right);
        }
        if (directions.Contains(Direction.Up) && directions.Contains(Direction.Left))
        {
            return ('J', Direction.Down);
        }
        if (directions.Contains(Direction.Up) && directions.Contains(Direction.Right))
        {
            return ('L', Direction.Down);
        }
        if (directions.Contains(Direction.Down) && directions.Contains(Direction.Left))
        {
            return ('7', Direction.Up);
        }
        if (directions.Contains(Direction.Down) && directions.Contains(Direction.Right))
        {
            return ('F', Direction.Left);
        }

        throw new InvalidOperationException("???");
    }

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }

    private record Tile(int X, int Y, char Pipe);
}
