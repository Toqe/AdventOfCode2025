public class Day08
{
    public void Run1()
    {
        var topK = 1000;

        var lines = DataHelper.GetLines("day08.txt")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        var coords = lines
            .Select(line => line.Split(',').Select(long.Parse).ToArray())
            .ToArray();

        var distances = new Dictionary<(int, int), double>();

        for (var i = 0; i < coords.Length - 1; i++)
        {
            for (var k = i + 1; k < coords.Length; k++)
            {
                var a = coords[i];
                var b = coords[k];
                var distance = this.CalculateDistance(a, b);
                distances.Add((i, k), distance);
            }
        }

        var closest = distances
            .OrderBy(x => x.Value)
            .Take(topK)
            .ToList();

        var clusters = new Dictionary<int, HashSet<int>>();

        foreach (var pair in closest)
        {
            var (i, k) = pair.Key;

            clusters.TryGetValue(i, out var clusterI);
            clusters.TryGetValue(k, out var clusterK);

            clusterI ??= new HashSet<int> { i };
            clusterK ??= new HashSet<int> { k };

            clusterI.UnionWith(clusterK);

            foreach (var item in clusterI)
            {
                clusters[item] = clusterI;
            }
        }
        
        var top3largestClusters = clusters
            .Select(x => x.Value)
            .Distinct()
            .OrderByDescending(x => x.Count)
            .Take(3)
            .ToArray();

        var result = 1L 
            * top3largestClusters[0].Count
            * top3largestClusters[1].Count
            * top3largestClusters[2].Count;

        Console.WriteLine($"Result: {result}");
    }

    public void Run2()
    {
        var lines = DataHelper.GetLines("day08.txt")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        var coords = lines
            .Select(line => line.Split(',').Select(long.Parse).ToArray())
            .ToArray();

        var distances = new Dictionary<(int, int), double>();

        for (var i = 0; i < coords.Length - 1; i++)
        {
            for (var k = i + 1; k < coords.Length; k++)
            {
                var a = coords[i];
                var b = coords[k];
                var distance = this.CalculateDistance(a, b);
                distances.Add((i, k), distance);
            }
        }

        var closest = distances
            .OrderBy(x => x.Value)
            .ToList();

        var clusters = new Dictionary<int, HashSet<int>>();

        foreach (var pair in closest)
        {
            var (i, k) = pair.Key;

            clusters.TryGetValue(i, out var clusterI);
            clusters.TryGetValue(k, out var clusterK);

            clusterI ??= new HashSet<int> { i };
            clusterK ??= new HashSet<int> { k };

            clusterI.UnionWith(clusterK);

            if (clusterI.Count == coords.Length)
            {
                Console.WriteLine(coords[i][0] + " " + coords[k][0] + " -> " + (1L * coords[i][0] * coords[k][0]));
                break;
            }

            foreach (var item in clusterI)
            {
                clusters[item] = clusterI;
            }
        }
    }

    private double CalculateDistance(long[] a, long[] b)
    {
        return Math.Sqrt(Math.Pow(b[0] - a[0], 2) + Math.Pow(b[1] - a[1], 2) + Math.Pow(b[2] - a[2], 2));
    }
}
