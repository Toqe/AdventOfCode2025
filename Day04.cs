public class Day04
{
    public void Run1()
    {
        var lines = DataHelper.GetLines("day04.txt").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        var result = this.Iterate(lines);
    }

    public void Run2()
    {
        var lines = DataHelper.GetLines("day04.txt").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        int toRemoveCount;
        var sum = 0;

        do
        {
            var result = this.Iterate(lines);
            lines = result.afterRemoval;
            toRemoveCount = result.toRemoveCount;
            sum += toRemoveCount;
        } while (toRemoveCount > 0);

        Console.WriteLine("Total sum: " + sum);
    }

    private (int toRemoveCount, string[] afterRemoval) Iterate(string[] lines)
    {   
        var adjacentCounts = new int[lines.Length, lines.First().Length];

        void AddAdjcentCount(int y, int x)
        {
            if (y >= 0 && y < lines.Length && x >= 0 && x < lines[y].Length)
            {
                adjacentCounts[y, x]++;
            }
        }

        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];

            for (var x = 0; x < line.Length; x++)
            {
                var currentChar = line[x];

                if (currentChar == '@')
                {
                    AddAdjcentCount(y - 1, x - 1);
                    AddAdjcentCount(y - 1, x);
                    AddAdjcentCount(y - 1, x + 1);
                    AddAdjcentCount(y, x - 1);
                    AddAdjcentCount(y, x + 1);
                    AddAdjcentCount(y + 1, x - 1);
                    AddAdjcentCount(y + 1, x);
                    AddAdjcentCount(y + 1, x + 1);
                }
            }
        }

        var sum = 0;
        var afterRemoval = new string[lines.Length];

        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            var lineAfterRemoval = new char[line.Length];

            for (var x = 0; x < line.Length; x++)
            {
                var currentChar = line[x];
                lineAfterRemoval[x] = currentChar;

                if (currentChar == '@' && adjacentCounts[y, x] < 4)
                {
                    sum++;
                    lineAfterRemoval[x] = '.';
                }
            }

            afterRemoval[y] = new string(lineAfterRemoval);
        }

        Console.WriteLine("Sum: " + sum);
        return (sum, afterRemoval);
    }
}
