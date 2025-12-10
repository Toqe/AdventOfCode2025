using System.Text;

public class Day10
{
    public void Run1()
    {
        var lines = DataHelper.GetLines("day10.txt")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        var sum = 0;

        foreach (var line in lines)
        {
            var machine = this.Parse(line);
            Console.WriteLine(machine);

            // Pressing a button twice negates its effect, so we only need to consider each button once.
            // We get every possible combination of buttons by counting in binary from 0 to 2^n - 1, where n is the number of buttons.

            var minCost = int.MaxValue;

            for (var i = 0; i < (1 << machine.Buttons.Count); i++)
            {
                var lights = new bool[machine.TargetIndicatorLights.Length];
                var cost = 0;

                for (var buttonIndex = 0; buttonIndex < machine.Buttons.Count; buttonIndex++)
                {
                    if ((i & (1 << buttonIndex)) != 0)
                    {
                        cost++;
                        var button = machine.Buttons[buttonIndex];

                        foreach (var lightIndex in button)
                        {
                            lights[lightIndex] = !lights[lightIndex];
                        }
                    }
                }

                if (lights.SequenceEqual(machine.TargetIndicatorLights))
                {
                    if (cost < minCost)
                    {
                        minCost = cost;
                        Console.WriteLine($"Found solution with cost {cost} with buttons {string.Join(" ", Enumerable.Range(0, machine.Buttons.Count).Where(bi => (i & (1 << bi)) != 0))}");
                    }
                }
            }

            sum += minCost;
        }

        Console.WriteLine("Sum: " + sum);
    }

    public void Run2()
    {
        var lines = DataHelper.GetLines("day10.txt")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        var sum = 0L;

        foreach (var line in lines)
        {
            var machine = this.Parse(line);
            Console.WriteLine(machine);
            Console.WriteLine($"buttons count: {machine.Buttons.Count}, joltage count: {machine.JoltageRequirements.Length}");

            // Hack: Use Z3 solver in python to find minimum of the linear equations.

            var pythonFile = "temp/temp.py";
            
            if (File.Exists(pythonFile))
            {
                File.Delete(pythonFile);
            }

            var sb = new StringBuilder();

            sb.AppendLine("from z3 import *");
            sb.AppendLine();

            for (var i = 0; i < machine.Buttons.Count; i++)
            {
                sb.AppendLine($"x{i + 1} = Int('x{i + 1}')");
            }

            sb.AppendLine("xs = [" + string.Join(", ", Enumerable.Range(1, machine.Buttons.Count).Select(i => $"x{i}")) + "]");

            sb.AppendLine();
            sb.AppendLine("opt = Optimize()");

            for (var i = 0; i < machine.Buttons.Count; i++)
            {
                sb.AppendLine($"opt.add(x{i + 1} >= 0)");
            }

            for (var j = 0; j < machine.JoltageRequirements.Length; j++)
            {
                var indices = Enumerable.Range(0, machine.Buttons.Count)
                    .Where(bi => machine.Buttons[bi].Any(x => x == j))
                    .Select(bi => $"x{bi + 1}")
                    .ToArray();

                sb.AppendLine($"opt.add({string.Join(" + ", indices)} == {machine.JoltageRequirements[j]})");
            }

            sb.AppendLine();
            sb.AppendLine($"h = opt.minimize({string.Join(" + ", Enumerable.Range(1, machine.Buttons.Count).Select(i => $"x{i}"))})");
            sb.AppendLine("opt.check()");
            sb.AppendLine("opt.model()");
            sb.AppendLine();
            sb.AppendLine("print (sum([opt.model()[v].as_long() for v in xs]))");

            File.WriteAllText(pythonFile, sb.ToString());

            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "python3";
            process.StartInfo.Arguments = pythonFile;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            var minCost = long.Parse(output.Trim());
            Console.WriteLine($"Min steps: {minCost}");

            sum += minCost;
        }

        Console.WriteLine("Sum: " + sum);
    }

    public void Run2TooSlow()
    {
        var lines = DataHelper.GetLines("day10.txt")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        var sum = 0;

        foreach (var line in lines)
        {
            var machine = this.Parse(line);
            Console.WriteLine(machine);
            Console.WriteLine($"buttons count: {machine.Buttons.Count}, joltage count: {machine.JoltageRequirements.Length}");

            var minCost = this.GetMinStepsTo(0, machine.JoltageRequirements, machine, new(new IntIntArrayTupleComparer()));
            Console.WriteLine($"Min steps: {minCost}");

            sum += minCost;
        }

        Console.WriteLine("Sum: " + sum);
    }

    private int GetMinStepsTo(int previousSteps, int[] joltages, Machine machine, Dictionary<(int, int[]), int> minStepsCache)
    {
        if (minStepsCache.TryGetValue((previousSteps, joltages), out var cachedSteps))
        {
            return cachedSteps;
        }

        if (joltages.All(x => x == 0))
        {
            return previousSteps;
        }

        if (joltages.Any(x => x < 0))
        {
            return int.MaxValue;
        }

        var minJoltage = joltages.Where(x => x > 0).Min();
        var minIndex = Array.IndexOf(joltages, minJoltage);

        var buttons = machine.Buttons
            .Select((b, i) => (b, i))
            .Where(x => x.b.Any(x => x == minIndex))
            .ToArray();

        var min = int.MaxValue;

        foreach (var buttonPresses in GetButtonPresses(minJoltage, buttons, new int[machine.Buttons.Count]))
        {
            var newJoltages = new int[joltages.Length];
            System.Buffer.BlockCopy(joltages, 0, newJoltages, 0, joltages.Length * sizeof(int));
            var failed = false;

            for (var buttonIndex = 0; buttonIndex < machine.Buttons.Count; buttonIndex++)
            {
                var presses = buttonPresses[buttonIndex];

                if (presses == 0)
                {
                    continue;
                }

                foreach (var index in machine.Buttons[buttonIndex])
                {
                    newJoltages[index] -= presses;

                    if (newJoltages[index] < 0)
                    {
                        failed = true;
                        break;
                    }
                }

                if (failed)
                {
                    break;
                }
            }

            if (!failed)
            {
                var steps = this.GetMinStepsTo(previousSteps + minJoltage, newJoltages, machine, minStepsCache);

                if (steps < min)
                {
                    min = steps;
                }
            }
        }

        minStepsCache[(previousSteps, joltages)] = min;

        return min;
    }

    private IEnumerable<int[]> GetButtonPresses(int totalPresses, IEnumerable<(int[] b, int i)> buttons, int[] prefix)
    {
        if (buttons.Count() == 1 && totalPresses >= 0)
        {
            var result = new int[prefix.Length];
            System.Buffer.BlockCopy(prefix, 0, result, 0, prefix.Length * sizeof(int));
            result[buttons.First().i] = totalPresses;
            yield return result;
        }
        else
        {
            for (var i = totalPresses; i >= 0; i--)
            {
                var newPrefix = new int[prefix.Length];
                System.Buffer.BlockCopy(prefix, 0, newPrefix, 0, prefix.Length * sizeof(int));
                newPrefix[buttons.First().i] = i;

                foreach (var buttonPresses in GetButtonPresses(totalPresses - i, buttons.Skip(1), newPrefix))
                {
                    yield return buttonPresses;
                }
            }
        }
    }

    private Machine Parse(string line)
    {
        // [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
        var result = new Machine
        {
            TargetIndicatorLights = line
                .Substring(0, line.IndexOf(' '))
                .Trim('[', ']')
                .Select(c => c == '#')
                .ToArray(),
            JoltageRequirements = line
                .Substring(line.IndexOf('{') + 1, line.IndexOf('}') - line.IndexOf('{') - 1)
                .Split(',')
                .Select(x => int.Parse(x))
                .ToArray(),
            Buttons = line.Substring(line.IndexOf(' '), line.IndexOf('{') - line.IndexOf(' '))
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim(')', '('))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(y => int.Parse(y)).ToArray())
                .ToList(),
        };

        return result;
    }

    private class IntIntArrayTupleComparer : IEqualityComparer<(int, int[])>
    {
        public bool Equals((int, int[]) x, (int, int[]) y)
        {
            if (x.Item1 != y.Item1)
            {
                return false;
            }

            if (x.Item2.Length != y.Item2.Length)
            {
                return false;
            }

            for (var i = 0; i < x.Item2.Length; i++)
            {
                if (x.Item2[i] != y.Item2[i])
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode((int, int[]) obj)
        {
            var hash = obj.Item1.GetHashCode();

            foreach (var item in obj.Item2)
            {
                hash = (hash * 397) ^ item.GetHashCode();
            }

            return hash;
        }
    }

    private class Machine
    {
        public bool[] TargetIndicatorLights { get; init; }

        public List<int[]> Buttons { get; init; }

        public int[] JoltageRequirements { get; init; }

        public override string ToString()
        {
            return $"Lights: [{string.Join("", TargetIndicatorLights.Select(x => x ? '#' : '.'))}], " +
                   $"Buttons: {string.Join(" ", Buttons.Select(x => $"({string.Join(",", x)})"))}, " +
                   $"Joltage Requirements: {{{string.Join(",", JoltageRequirements)}}}";
        }
    }
}
