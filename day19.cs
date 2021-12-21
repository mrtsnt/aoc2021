var unsolved = new Queue<Scanner>(File.ReadAllText("data/day19").Split("\n\n").Select(b => new Scanner(b.Split("\n")[1..])));
var solved = new List<Scanner>();
solved.Add(unsolved.Dequeue());

while (unsolved.Count != 0)
{
    var badScanner = unsolved.Dequeue();
    var foundMatch = false;

    foreach (var correctScanner in solved)
    {
        var aligned = correctScanner.TryAlign(badScanner);
        if (aligned is not null)
        {
            solved.Add(aligned);
            foundMatch = true;
            break;
        }
    }

    if (!foundMatch)
        unsolved.Enqueue(badScanner);
}

var uniquePoints = new HashSet<(int, int, int)>(solved.SelectMany(s => s.Points));
Console.WriteLine($"part1: {uniquePoints.Count}");

int Manhattan((int x, int y, int z) p, (int x, int y, int z) p2) => Math.Abs(p.x - p2.x) + Math.Abs(p.y - p2.y) + Math.Abs (p.z - p2.z);
var max = 0;
for (int i = 0; i < solved.Count; i++)
    for (int j = i + 1; j < solved.Count; j++)
        if (Manhattan(solved[i].Location, solved[j].Location) > max)
            max = Manhattan(solved[i].Location, solved[j].Location);
Console.WriteLine($"part2: {max}");

public class Scanner
{
    public (int x, int y, int z) Location { get; set; } = (0, 0, 0);
    public List<(int x, int y, int z)> Points { get; set; } = new();

    public Scanner(IEnumerable<string> rawPoints)
    {
        foreach (var p in rawPoints)
        {
            var split = p.Split(',');
            Points.Add((Int32.Parse(split[0]), Int32.Parse(split[1]), Int32.Parse(split[2])));
        }
    }

    public Scanner? TryAlign(Scanner other)
    {
        foreach(var thisP in Points)
        {
            var thisOffsets = Offsets(thisP, Points);
            foreach (var otherRotation in other.Rotations())
            {
                foreach (var otherP in otherRotation)
                {
                    var otherOffsets = Offsets(otherP, otherRotation);
                    if (thisOffsets.Intersect(otherOffsets).Count() >= 12)
                    {
                        var (xo, yo, zo) = (thisP.x - otherP.x, thisP.y - otherP.y, thisP.z - otherP.z);
                        other.Location = (xo, yo, zo);
                        other.Points = otherRotation.Select(co => (co.x + xo, co.y + yo, co.z + zo)).ToList();
                        return other;
                    }
                }
            }
        }

        return null;
    }

    public IEnumerable<IEnumerable<(int x, int y, int z)>> Rotations()
    {
        for (int i = 0; i < 24; i++)
            yield return Points.Select(p => Rotate(p, i));
    }

    private static IEnumerable<(int, int, int)> Offsets((int x, int y, int z) p, IEnumerable<(int, int, int)> points)
    {
        foreach(var (x, y, z) in points)
            yield return (p.x - x, p.y - y, p.z - z);
    }

    private (int, int, int) Rotate((int x, int y, int z) p, int rotation) =>
        rotation switch
        {
            0 => (p.x, p.y, p.z), 1 => (p.x, -p.z, p.y), 2 => (p.x, -p.y, -p.z),
            3 => (p.x, p.z, -p.y), 4 => (-p.y, p.x, p.z), 5 => (p.z, p.x, p.y),
            6 => (p.y, p.x, -p.z), 7 => (-p.z, p.x, -p.y), 8 => (-p.x, -p.y, p.z),
            9 => (-p.x, -p.z, -p.y), 10 => (-p.x, p.y, -p.z), 11 => (-p.x, p.z, p.y),
            12 => (p.y, -p.x, p.z), 13 => (p.z, -p.x, -p.y), 14 => (-p.y, -p.x, -p.z),
            15 => (-p.z, -p.x, p.y), 16 => (-p.z, p.y, p.x), 17 => (p.y, p.z, p.x),
            18 => (p.z, -p.y, p.x), 19 => (-p.y,-p.z, p.x), 20 => (-p.z, -p.y, -p.x),
            21 => (-p.y, p.z, -p.x), 22 => (p.z, p.y, -p.x), 23 => (p.y, -p.z, -p.x),
            _ => throw new Exception("unkown rotation")
        };
}