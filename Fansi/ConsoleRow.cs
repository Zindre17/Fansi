namespace Fansi;

public class ConsoleRow
{
    private readonly int maxWidth;
    private readonly List<ConsoleOutput> segments = new();

    private int currentWidth = 0;

    public ConsoleRow(int maxWidth)
    {
        this.maxWidth = maxWidth;
    }

    public ConsoleOutput Common { get; set; } = new();

    public void AddSegment(ConsoleOutput segment, double? widthRatio = null)
    {
        if (widthRatio is not null)
        {
            if (widthRatio is < 0 or > 1)
            {
                throw new ArgumentException("Must be between 0 and 1.", nameof(widthRatio));
            }
            segment = segment with { Width = (int)Math.Round(maxWidth * widthRatio.Value) };
        }

        var segmentWidth = segment.ActualWidth;
        if ((segmentWidth + currentWidth) > maxWidth)
        {
            throw new InvalidOperationException("Cannot add segment. Max width will be violated.");
        }

        currentWidth += segmentWidth;
        segments.Add(segment);
    }

    public void Fill(ConsoleOutput row)
    {
        if (segments.Any())
        {
            throw new InvalidOperationException("Cannot fill non-empty row.");
        }

        currentWidth = maxWidth;
        row = row with { Width = maxWidth };

        segments.Add(row);
    }

    public void Print()
    {
        foreach (var segment in segments)
        {
            segment.Apply(Common).Print();
        }
        if (currentWidth < maxWidth)
        {
            var rest = Common with { Width = maxWidth - currentWidth };
            rest.Print();
        }
    }
}
