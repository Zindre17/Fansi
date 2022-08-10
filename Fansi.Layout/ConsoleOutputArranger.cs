namespace Fansi.Layout;

public class ConsoleOutputArranger : IPrintable
{
    private readonly List<PositionedConsoleArea> areas = new();

    public bool NewLineOnWrap { get; set; } = true;

    public void AddBlock(ConsoleArea area)
    {
        areas.Add(new(area, true));
    }

    public void Append(ConsoleArea area)
    {
        areas.Add(new(area, false));
    }

    public void Print(OutputFormat? format = null)
    {
        var currentHeight = 0;
        var currentWidth = 0;
        var currentAreas = new List<ConsoleArea>();
        foreach (var (area, standalone) in areas)
        {
            // Triggers for printing current
            if (standalone || (currentWidth + area.Width) > Console.WindowWidth)
            {
                PrintLines(currentHeight, currentAreas);
                if (NewLineOnWrap)
                {
                    Console.WriteLine();
                }

                currentAreas.Clear();
                currentHeight = 0;
                currentWidth = 0;
            }

            if (standalone)
            {
                area.Print();
            }

            // accumulate current
            currentAreas.Add(area);
            currentWidth += area.Width;
            if (area.Height > currentHeight)
            {
                currentHeight = area.Height;
            }
        }

        PrintLines(currentHeight, currentAreas);
    }

    private static void PrintLines(int lineCount, List<ConsoleArea> areasToPrint)
    {
        for (int i = 0; i < lineCount; i++)
        {
            foreach (var currentArea in areasToPrint.Where(a => a.Height > i))
            {
                currentArea.PrintRow(i);
            }
            Console.WriteLine();
        }
    }

    private record PositionedConsoleArea(ConsoleArea Area, bool Standalone);
}
