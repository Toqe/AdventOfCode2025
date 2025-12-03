public class Day3
{
    public void Run1()
    {
        var lines = DataHelper.GetLines("day3.txt");
        var sum = 0L;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var lineDigits = line.Select(c => int.Parse(c.ToString())).ToArray();
            var minIndices = new int?[10];
            var maxIndices = new int?[10];

            for (var i = 0; i < lineDigits.Length; i++)
            {
                var digit = lineDigits[i];

                if (minIndices[digit] == null)
                {
                    minIndices[digit] = i;
                }

                maxIndices[digit] = i;
            }

            var found = false;

            for (var x1 = 9; x1 >= 0; x1--)
            {
                if (found)
                {
                    break;
                }

                if (minIndices[x1] == null)
                {
                    continue;
                }

                for (var x2 = 9; x2 >= 0; x2--)
                {
                    if (minIndices[x2] == null)
                    {
                        continue;
                    }

                    if (minIndices[x1] < maxIndices[x2])
                    {
                        var max = x1 * 10 + x2;
                        Console.WriteLine(line + " -> " + max);
                        sum += max;
                        found = true;
                        break;
                    }
                }
            }
        }

        Console.WriteLine("sum: " + sum);
    }

    public void Run2()
    {
        var lines = DataHelper.GetLines("day3.txt");
        var sum = 0L;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var lineDigits = line.Select(c => int.Parse(c.ToString())).ToArray();
            var max = new MaxJoltageFinder(lineDigits).GetBiggestJoltage(0, 12);
            Console.WriteLine(line + " -> " + max);
            sum += max;
        }

        Console.WriteLine("sum: " + sum);
    }

    private class MaxJoltageFinder(
        int[] digits)
    {
        private readonly Dictionary<(int start, int length), long> cache = new();

        public long GetBiggestJoltage(int start, int length)
        {
            if (cache.TryGetValue((start, length), out var cached))
            {
                return cached;
            }
            
            if ((digits.Length < start + length) || length <= 0)
            {
                return 0;
            }

            if (length == 1)
            {
                return digits.Where((x, i) => i >= start).Max();
            }

            var result = Math.Max(
                digits[start] * (long)Math.Pow(10, length - 1) + GetBiggestJoltage(start + 1, length - 1),
                GetBiggestJoltage(start + 1, length));

            cache[(start, length)] = result;
            return result;
        }
    }
}
