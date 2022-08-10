namespace Fansi.Layout;

public class ConsoleRow : IPrintable
{
    private readonly int maxWidth;
    private readonly List<ConsoleOutput> segments = new();

    private int currentWidth = 0;

    public ConsoleRow(int maxWidth)
    {
        this.maxWidth = maxWidth;
    }

    public OutputFormat CommonFormat { get; set; } = new();

    public void Apply(OutputFormat format)
    {
        CommonFormat = CommonFormat.Apply(format);
    }

    public void AddSegment(string text, OutputFormat? format = null, double? widthRatio = null)
    {
        AddSegment(new(text, format), widthRatio);
    }

    public void AddSegment(ConsoleOutput segment, double? widthRatio = null)
    {
        if (widthRatio is not null)
        {
            if (widthRatio is < 0 or > 1)
            {
                throw new ArgumentException("Must be between 0 and 1.", nameof(widthRatio));
            }
            segment.Apply(new() { Width = (int)Math.Round(maxWidth * widthRatio.Value) });
        }

        var segmentWidth = segment.CalculateWidth();
        if ((segmentWidth + currentWidth) > maxWidth)
        {
            throw new InvalidOperationException("Cannot add segment. Max width will be violated.");
        }

        currentWidth += segmentWidth;
        segments.Add(segment);
    }

    public void Fill(string text = "", OutputFormat? format = null)
    {
        Fill(new(text, format));
    }

    public void Fill(ConsoleOutput row)
    {
        if (segments.Any())
        {
            throw new InvalidOperationException("Cannot fill non-empty row.");
        }

        currentWidth = maxWidth;
        row.Apply(new() { Width = maxWidth });

        segments.Add(row);
    }

    public void Print(OutputFormat? format = null)
    {
        var common = CommonFormat.Apply(format ?? new());
        foreach (var segment in segments)
        {
            segment.Print(common);
        }
        if (currentWidth < maxWidth)
        {
            var rest = common with { Width = maxWidth - currentWidth };
            rest.Print("");
        }
    }
}
