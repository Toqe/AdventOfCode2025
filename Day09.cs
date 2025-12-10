public class Day09
{
    public void Run1()
    {
        var lines = DataHelper.GetLines("day09.txt")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        var coords = lines.Select(line =>
        {
            var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            return (long.Parse(parts[0]), long.Parse(parts[1]));
        })
        .ToList();

        var max = 0L;

        for (var i = 0; i < coords.Count - 1; i++)
        {
            var (x1, y1) = coords[i];

            for (var k = i + 1; k < coords.Count; k++)
            {
                var (x2, y2) = coords[k];

                var area = 1L * (Math.Abs(x2 - x1) + 1) * (Math.Abs(y2 - y1) + 1);

                if (area > max)
                {
                    max = area;
                }
            }
        }

        Console.WriteLine($"Max area: {max}");
    }

    public void Run2()
    {
        var lines = DataHelper.GetLines("day09.txt")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        var log = false;

        var coords = lines.Select(line =>
        {
            var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        })
        .ToList();

        var result = new List<(int i, int k, long area)>();

        for (var i = 0; i < coords.Count - 1; i++)
        {
            var (x1, y1) = coords[i];

            for (var k = i + 1; k < coords.Count; k++)
            {
                var (x2, y2) = coords[k];
                var area = 1L * (Math.Abs(x2 - x1) + 1) * (Math.Abs(y2 - y1) + 1);
                result.Add((i, k, area));
            }
        }

        result = result.OrderByDescending(x => x.area).ToList();

        foreach (var (i, k, area) in result)
        {
            var (x1, y1) = coords[i];
            var (x2, y2) = coords[k];
            var skip = false;

            if (log)
            {
                Console.WriteLine($"Checking area {area} between ({x1},{y1}) and ({x2},{y2})");
            }

            for (var m = 0; m < coords.Count; m++)
            {
                var (xm, ym) = coords[m];
                var (xn, yn) = coords[(m + 1) % coords.Count];

                if (xm > Math.Min(x1, x2) && xm < Math.Max(x1, x2) &&
                    ym > Math.Min(y1, y2) && ym < Math.Max(y1, y2))
                {
                    if (log)
                    {
                        Console.WriteLine($"Point ({xm},{ym}) is inside area");
                    }

                    skip = true;
                    break;
                }

                if (xn > Math.Min(x1, x2) && xn < Math.Max(x1, x2) &&
                    yn > Math.Min(y1, y2) && yn < Math.Max(y1, y2))
                {
                    if (log)
                    {
                        Console.WriteLine($"Point ({xn},{yn}) is inside area");
                    }

                    skip = true;
                    break;
                }

                var minXLine = Math.Min(xm, xn);
                var maxXLine = Math.Max(xm, xn);
                var minYLine = Math.Min(ym, yn);
                var maxYLine = Math.Max(ym, yn);

                var minXRect = Math.Min(x1, x2);
                var maxXRect = Math.Max(x1, x2);
                var minYRect = Math.Min(y1, y2);
                var maxYRect = Math.Max(y1, y2);

                var isLineHorizontal = minYLine == maxYLine;
                var isLineVertical = minXLine == maxXLine;

                if (isLineVertical
                    && minYLine == minYRect && maxYLine == maxYRect 
                    && minXLine > minXRect && minXLine < maxXRect)
                {
                    if (log)
                    {
                        Console.WriteLine($"Line ({xm},{ym})-({xn},{yn}) is crossing area");
                    }

                    skip = true;
                    break;
                }

                if (isLineHorizontal
                    && minXLine == minXRect && maxXLine == maxXRect 
                    && minYLine > minYRect && minYLine < maxYRect)
                {
                    if (log)
                    {
                        Console.WriteLine($"Line ({xm},{ym})-({xn},{yn}) is crossing area");
                    }

                    skip = true;
                    break;
                }

                if (isLineHorizontal && minYRect < minYLine && maxYRect > minYLine 
                    && minXLine < maxXRect && maxXLine > minXRect)
                {
                    if (log)
                    {
                        Console.WriteLine($"Line ({xm},{ym})-({xn},{yn}) is crossing area");
                    }

                    skip = true;
                    break;
                }

                if (isLineVertical && minXRect < minXLine && maxXRect > minXLine 
                    && minYLine < maxYRect && maxYLine > minYRect)
                {
                    if (log)
                    {
                        Console.WriteLine($"Line ({xm},{ym})-({xn},{yn}) is crossing area");
                    }

                    skip = true;
                    break;
                }
            }

            if (skip)
            {
                continue;
            }
            else
            {
                Console.WriteLine($"Max area: {area} between ({x1},{y1}) and ({x2},{y2})");
                break;
            }
        }
    }
}
