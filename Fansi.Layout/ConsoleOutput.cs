namespace Fansi.Layout;

public class ConsoleOutput : IPrintable
{
    public ConsoleOutput(string? text = null, OutputFormat? format = null)
    {
        Text = text ?? string.Empty;
        Format = format ?? new();
    }

    public string Text { get; set; }

    public OutputFormat Format { get; set; }

    public void EnrichWith(OutputFormat format)
    {
        Format = Format.EnrichWith(format);
    }

    public int CalculateWidth()
    {
        return Format.CalculateWidth(Text);
    }

    public void Print(OutputFormat? format = null)
    {
        if (format is null)
        {
            Format.Print(Text);
        }
        else
        {
            Format.EnrichWith(format).Print(Text);
        }
    }

    public override string ToString() => Format.ApplyToText(Text);
}
