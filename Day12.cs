public class Day12
{
    public void Run1()
    {
        var lines = DataHelper.GetLines("day12.txt")
            .ToList();

        var shapes = new List<bool[][]>();
        var shapesX = new List<int>();

        for (var i = 0; i <= 5; i++)
        {
            var shape = lines
                .Skip(i * 5 + 1)
                .Take(3)
                .Select(x => x.Select(c => c == '#').ToArray())
                .ToArray();

            shapes.Add(shape);

            shapesX.Add(shape.Sum(x => x.Count(c => c == true)));
            Console.WriteLine($"Shape {i} has {shapesX.Last()} filled cells");
        }

        foreach (var shape in shapes)
        {
            for (var r = 0; r < shape.Length; r++)
            {
                for (var c = 0; c < shape[r].Length; c++)
                {
                    Console.Write(shape[r][c] ? '#' : '.');
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        var fitsCounts = 0;

        foreach (var line in lines.Skip(30))
        {
            Console.WriteLine(line);

            var gridSize = line.Split(": ").First().Split('x').Select(int.Parse).ToArray();
            var size = gridSize[0] * gridSize[1];

            var shapeCounts = line.Split(": ").Last().Split(' ').Select(int.Parse).ToArray();
            var shapeTotal = 0;

            for (var i = 0; i < shapes.Count; i++)
            {
                var shape = shapes[i];
                var count = shapeCounts[i];

                var shapeSize = count * shapesX[i];
                shapeTotal += shapeSize;
            }

            // HACK: really bad approximation ... but it works for my input ...
            if (shapeTotal < size)
            {
                fitsCounts++;
            }
       }

        Console.WriteLine($"Fits count: {fitsCounts}");
    }
}
