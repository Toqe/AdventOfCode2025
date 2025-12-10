public class Day06
{
    public void Run1()
    {
        var lines = DataHelper.GetLines("day06.txt");

        var matrix = lines
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList())
            .ToList();

        var columns = matrix.First().Count;
        var sum = 0L;

        for (var columnIndex = 0; columnIndex < columns; columnIndex++)
        {
            var op = matrix.Last()[columnIndex];
            var columnResult = 0L;

            if (op == "+")
            {
                for (var rowIndex = 0; rowIndex < matrix.Count - 1; rowIndex++)
                {
                    columnResult += long.Parse(matrix[rowIndex][columnIndex]);
                }
            }
            else if (op == "*")
            {
                columnResult = 1L;

                for (var rowIndex = 0; rowIndex < matrix.Count - 1; rowIndex++)
                {
                    columnResult *= long.Parse(matrix[rowIndex][columnIndex]);
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown operation: " + op);
            }

            Console.WriteLine("Column " + columnIndex + " with operator " + op + " result: " + columnResult);

            sum += columnResult;
        }

        Console.WriteLine("Sum: " + sum);
    }

    public void Run2()
    {
        var lines = DataHelper.GetLines("day06.txt")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
        
        var opsLine = lines.Last();

        var ops = opsLine
            .Select((c, i) => (c,i))
            .Where(x => x.c != ' ')
            .ToList();

        var sum = 0L;

        for (var opIndex = 0; opIndex < ops.Count; opIndex++)
        {
            var op = ops[opIndex].c;
            var startIndex = ops[opIndex].i;
            var endIndex = opIndex == ops.Count - 1 ? opsLine.Length : ops[opIndex + 1].i;

            var numbers = new List<long>();

            for (var colIndex = startIndex; colIndex < endIndex; colIndex++)
            {
                var numberText = "";

                for (var rowIndex = 0; rowIndex < lines.Count - 1; rowIndex++)
                {
                    numberText += lines[rowIndex][colIndex];
                }

                numberText = numberText.Trim();

                if (string.IsNullOrWhiteSpace(numberText))
                {
                    continue;
                }

                numbers.Add(long.Parse(numberText));
            }

            var columnResult = 0L;

            if (op == '+')
            {
                foreach (var number in numbers)
                {
                    columnResult += number;
                }
            }
            else if (op == '*')
            {
                columnResult = 1L;

                foreach (var number in numbers)
                {
                    columnResult *= number;
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown operation: " + op);
            }

            Console.WriteLine("Column " + opIndex + " with operator " + op + " result: " + columnResult);

            sum += columnResult;
        }

        Console.WriteLine("Sum: " + sum);
    }
}
