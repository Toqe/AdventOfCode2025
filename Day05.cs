public class Day05
{
    public void Run1()
    {
        var lines = DataHelper.GetLines("day05.txt");
        var emptyLineIndex = lines.ToList().FindIndex(string.IsNullOrWhiteSpace);

        var freshIdRanges = lines
            .Take(emptyLineIndex)
            .Select(line =>
            {
                var parts = line.Split('-');
                return (long.Parse(parts[0]), long.Parse(parts[1]));
            })
            .ToList();
        
        var availableIds = lines.Skip(emptyLineIndex + 1)
            .Select(line => long.Parse(line))
            .ToList();

        var count = 0;

        foreach (var id in availableIds)
        {
            foreach (var range in freshIdRanges)
            {
                if (id >= range.Item1 && id <= range.Item2)
                {
                    count++;
                    break;
                }
            }
        }

        Console.WriteLine("Count: " + count);
    }

    public void Run2()
    {
        var lines = DataHelper.GetLines("day05.txt");

        var emptyLineIndex = lines.ToList().FindIndex(string.IsNullOrWhiteSpace);

        var freshIdRanges = lines
            .Take(emptyLineIndex)
            .Select(line =>
            {
                var parts = line.Split('-');
                return (long.Parse(parts[0]), long.Parse(parts[1]));
            })
            .OrderBy(x => x.Item1)
            .ToList();

        var i = 0;
        
        while (i < freshIdRanges.Count)
        {
            if (i > 0 && DoRangesOverlap(freshIdRanges[i - 1], freshIdRanges[i]))
            {
                var newRange = (
                    Math.Min(freshIdRanges[i - 1].Item1, freshIdRanges[i].Item1),
                    Math.Max(freshIdRanges[i - 1].Item2, freshIdRanges[i].Item2)
                );

                freshIdRanges[i - 1] = newRange;
                freshIdRanges.RemoveAt(i);
                i--;
                continue;
            }
            
            if (i < freshIdRanges.Count - 1 && DoRangesOverlap(freshIdRanges[i], freshIdRanges[i + 1]))
            {
                var newRange = (
                    Math.Min(freshIdRanges[i].Item1, freshIdRanges[i + 1].Item1),
                    Math.Max(freshIdRanges[i].Item2, freshIdRanges[i + 1].Item2)
                );

                freshIdRanges[i] = newRange;
                freshIdRanges.RemoveAt(i + 1);
                continue;
            }
            
            i++;
        }

        var sum = 0L;

        foreach (var range in freshIdRanges)
        {
            sum += range.Item2 - range.Item1 + 1;
            Console.WriteLine($"Range: {range.Item1}-{range.Item2}");
        }
        
        Console.WriteLine("Sum: " + sum);
    }

    private bool DoRangesOverlap((long, long) range1, (long, long) range2)
    {
        return range1.Item1 <= range2.Item2 && range2.Item1 <= range1.Item2;
    }
}
