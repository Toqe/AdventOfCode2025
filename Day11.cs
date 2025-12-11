public class Day11
{
    public void Run1()
    {
        var lines = DataHelper.GetLines("day11.txt")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        var deviceConnections = new Dictionary<string, List<string>>();

        foreach (var line in lines)
        {
            var parts = line.Split(":", StringSplitOptions.TrimEntries);
            deviceConnections[parts[0]] = parts[1].Split(" ", StringSplitOptions.TrimEntries).ToList();
        }

        var start = "you";
        var end = "out";
        var count = 0;

        void Visit(string node, HashSet<string> visited)
        {
            if (node == end)
            {
                count++;
                return;
            }

            if (visited.Contains(node))
            {
                return;
            }

            visited.Add(node);

            if (deviceConnections.ContainsKey(node))
            {
                foreach (var neighbor in deviceConnections[node])
                {
                    Visit(neighbor, new HashSet<string>(visited));
                }
            }
        }

        Visit(start, new HashSet<string>());

        Console.WriteLine($"Total paths: {count}");
    }

    public void Run2()
    {
        var lines = DataHelper.GetLines("day11.txt")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        var deviceConnections = new Dictionary<string, List<string>>();

        foreach (var line in lines)
        {
            var parts = line.Split(":", StringSplitOptions.TrimEntries);
            deviceConnections[parts[0]] = parts[1].Split(" ", StringSplitOptions.TrimEntries).ToList();
        }

        Dictionary<(string, string), long> memo = new();

        long GetPathsCount(string start, string end)
        {
            if (memo.ContainsKey((start, end)))
            {
                return memo[(start, end)];
            }

            long paths = 0;

            foreach (var neighbor in deviceConnections.GetValueOrDefault(start, new List<string>()))
            {
                if (neighbor == end)
                {
                    paths++;
                }
                else
                {
                    paths += GetPathsCount(neighbor, end);
                }
            }

            memo[(start, end)] = paths;
            return paths;
        }

        var paths = 
            GetPathsCount("svr", "fft") *
            GetPathsCount("fft", "dac") *
            GetPathsCount("dac", "out");

        Console.WriteLine($"Total paths: {paths}");

        paths = 
            GetPathsCount("svr", "dac") *
            GetPathsCount("dac", "fft") *
            GetPathsCount("fft", "out");

        Console.WriteLine($"Total paths: {paths}");
    }
}
