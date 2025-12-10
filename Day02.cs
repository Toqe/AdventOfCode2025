public class Day02
{
    public void Run1()
    {
        var lines = DataHelper.GetLines("day02.txt");
        var ranges = lines[0].Split(",");
        var sum = 0L;
        
        foreach (var range in ranges)
        {
            var bounds = range.Split("-").Select(long.Parse).ToArray();

            for (var i = bounds[0]; i <= bounds[1]; i++)
            {
                var str = i.ToString();

                if (str.Length % 2 == 1)
                {
                    continue;
                }

                if (str.Substring(0, str.Length / 2) != str.Substring(str.Length / 2))
                {
                    continue;
                }

                Console.WriteLine(i);
                
                sum += i;
            }
        }

        Console.WriteLine("sum: " + sum);
    }

    public void Run2()
    {
        var lines = DataHelper.GetLines("day02.txt");
        var ranges = lines[0].Split(",");
        var sum = 0L;
        
        foreach (var range in ranges)
        {
            var bounds = range.Split("-").Select(long.Parse).ToArray();

            for (var i = bounds[0]; i <= bounds[1]; i++)
            {
                var str = i.ToString();

                for (var len = 1; len <= str.Length / 2; len++)
                {               
                    if (!IsRepetitionOf(str, str.Substring(0, len)))
                    {
                        continue;
                    }

                    Console.WriteLine(i);
                    sum += i;
                    break;
                }
            }
        }

        Console.WriteLine("sum: " + sum);
    }

    private bool IsRepetitionOf(string str, string part)
    {
        var len = part.Length;

        if (len * (str.Length / len) != str.Length)
        {
            return false;
        }

        for (var i = 0; i < str.Length / len; i++)
        {
            if (str.Substring(i * len, len) != part)
            {
                return false;
            }
        }

        return true;
    }
}