public static class DataHelper
{
    public static string[] GetLines(string filename)
    {
        var dir = new DirectoryInfo(".");

        while (dir.EnumerateFiles("aoc-2025.sln").Any() != true)
        {
            dir = dir.Parent;
        }

        return File.ReadAllLines(Path.Combine(dir.FullName, "data", filename));
    }
}
