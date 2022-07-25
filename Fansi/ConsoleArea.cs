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

    public OutputFormat CommonFormat { get; set; } = new();

    public void Apply(OutputFormat format)
    {
        CommonFormat = CommonFormat.Apply(format);
    }

    public ConsoleRow this[int index] => lines[index];

    public void Print(OutputFormat? format = null)
    {
        var combined = CommonFormat.Apply(format ?? new());
        foreach (var line in lines)
        {
            line.Print(combined);
            Console.WriteLine();
        }
    }

    public void PrintRow(int index, OutputFormat? format = null)
    {
        var combined = CommonFormat.Apply(format ?? new());
        this[index].Print(combined);
    }
}
