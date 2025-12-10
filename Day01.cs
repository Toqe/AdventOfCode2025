public class Day01 
{
    public void Run1()
    {
        var data = DataHelper.GetLines("day01.txt");

        var pos = 50;
        var count = 0;

        foreach (var item in data)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                continue;
            }

            var sign = 1;

            if (item[0] == 'L')
            {
                sign = -1;
            }

            var dist = int.Parse(item.Substring(1));

            pos = (pos + sign * dist + 100) % 100;

            Console.WriteLine(item + " -> " + pos);

            if (pos == 0)
            {
                count++;
            }
        }

        Console.WriteLine("count: " + count);
    }

    public void Run2()
    {
        var data = DataHelper.GetLines("day01.txt");

        var pos = 50;
        var count = 0;

        foreach (var item in data)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                continue;
            }

            var sign = 1;

            if (item[0] == 'L')
            {
                sign = -1;
            }

            var dist = int.Parse(item.Substring(1)) * sign;

            while (dist != 0)
            {
                pos += Math.Sign(dist);
                pos = (pos + 100) % 100;
                dist -= Math.Sign(dist);

                if (pos == 0)
                {
                    count++;
                }
            }

            Console.WriteLine(item + " -> " + pos);
        }

        Console.WriteLine("count: " + count);
    }
}
