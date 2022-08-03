namespace Fansi;

public class ConsoleArea
{
    private readonly List<ConsoleRow> lines = new();
    private readonly int width;

    public ConsoleArea(int width, int? height = null)
    {
        this.width = width;

        for (int i = 0; i < height; i++)
        {
            lines.Add(new(width));
        }
    }

    public OutputFormat CommonFormat { get; set; } = new();

    public void Apply(OutputFormat format)
    {
        CommonFormat = CommonFormat.Apply(format);
    }

    public ConsoleRow AddNewRow()
    {
        var row = new ConsoleRow(width);
        lines.Add(row);
        return row;
    }

    public void AddRow(string text, OutputFormat? format = null)
    {
        AddRow(new(text, format));
    }

    public void AddRow(ConsoleOutput output)
    {
        AddNewRow().Fill(output);
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
