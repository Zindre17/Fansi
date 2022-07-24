namespace Fansi;

public class ConsoleArea
{
    private readonly ConsoleRow[] lines;
    private ConsoleOutput common = new();

    public ConsoleArea(int height, int width)
    {
        lines = new ConsoleRow[height];
        for (int i = 0; i < height; i++)
        {
            lines[i] = new(width);
        }
    }

    public ConsoleOutput Common
    {
        get => common;
        set
        {
            foreach (var line in lines)
            {
                line.Common = line.Common.Apply(value);
            }
            common = value;
        }
    }

    public ConsoleRow this[int index] => lines[index];

    public void Print()
    {
        foreach (var line in lines)
        {
            line.Print();
            Console.WriteLine();
        }
    }

    public void PrintRow(int index)
    {
        this[index].Print();
    }
}
