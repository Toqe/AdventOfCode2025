public class Day07
{
    public void Run1()
    {
        var lines = DataHelper.GetLines("day07.txt")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        var beams = new HashSet<int>();
        var splits = 0;
        var isFirstLine = true;

        foreach (var line in lines)
        {
            if (isFirstLine)
            {
                isFirstLine = false;
                var startIndex = line.IndexOf('S');
                beams.Add(startIndex);
                continue;
            }

            var newBeams = new HashSet<int>();

            foreach (var beam in beams.ToList())
            {
                if (line[beam] == '^')
                {
                    newBeams.Add(beam - 1);
                    newBeams.Add(beam + 1);
                    splits++;
                }
                else
                {
                    newBeams.Add(beam);
                }
            }

            beams = newBeams;
        }

        Console.WriteLine("Splits: " + splits);
    }

    public void Run2()
    {
        var lines = DataHelper.GetLines("day07.txt")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        var beams = new HashSet<int>();
        var beamPossibilities = new Dictionary<(int lineIndex, int beamIndex), long>();
        var isFirstLine = true;

        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];

            if (isFirstLine)
            {
                isFirstLine = false;
                var startIndex = line.IndexOf('S');
                beams.Add(startIndex);
                beamPossibilities[(0, startIndex)] = 1;
                continue;
            }

            var newBeams = new HashSet<int>();

            foreach (var beam in beams.ToList())
            {
                if (line[beam] == '^')
                {
                    newBeams.Add(beam - 1);
                    newBeams.Add(beam + 1);
                    var count = beamPossibilities[(i - 1, beam)];
                    beamPossibilities[(i, beam - 1)] = beamPossibilities.GetValueOrDefault((i, beam - 1), 0) + count;
                    beamPossibilities[(i, beam + 1)] = beamPossibilities.GetValueOrDefault((i, beam + 1), 0) + count;
                }
                else
                {
                    newBeams.Add(beam);
                    var count = beamPossibilities[(i - 1, beam)];
                    beamPossibilities[(i, beam)] = beamPossibilities.GetValueOrDefault((i, beam), 0) + count;
                }
            }

            beams = newBeams;
        }

        // for (var lineIndex = 0; lineIndex < lines.Count; lineIndex++)
        // {
        //     var line = lines[lineIndex];

        //     for (var beamIndex = 0; beamIndex < line.Length; beamIndex++)
        //     {
        //         if (!beamPossibilities.ContainsKey((lineIndex, beamIndex)))
        //         {
        //             Console.Write("    ");
        //         } 
        //         else
        //         {
        //             Console.Write(beamPossibilities[(lineIndex, beamIndex)].ToString().PadLeft(4));
        //         }
        //     }

        //     Console.WriteLine();
        // }

        var sum = beamPossibilities
            .Where(kvp => kvp.Key.lineIndex == lines.Count - 1)
            .Sum(kvp => kvp.Value);

        Console.WriteLine("Possibilities: " + sum);
    }
}
