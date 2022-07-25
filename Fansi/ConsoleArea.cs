namespace Fansi;

public class ConsoleArea
{
    private readonly ConsoleRow[] lines;

    public ConsoleArea(int height, int width)
    {
        lines = new ConsoleRow[height];
        for (int i = 0; i < height; i++)
        {
            lines[i] = new(width);
        }
    }

    public OutputOptions Common { get; set; } = new();

    public void Apply(OutputOptions options)
    {
        Common = Common.Apply(options);
    }

    public ConsoleRow this[int index] => lines[index];

    public void Print(OutputOptions? options = null)
    {
        var combined = Common.Apply(options ?? new());
        foreach (var line in lines)
        {
            line.Print(combined);
            Console.WriteLine();
        }
    }

    public void PrintRow(int index, OutputOptions? options = null)
    {
        var combined = Common.Apply(options ?? new());
        this[index].Print(combined);
    }
}
