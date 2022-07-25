namespace Fansi;

public class ConsoleOutput
{
    public ConsoleOutput(string? text = null, OutputFormat? format = null)
    {
        Text = text ?? string.Empty;
        Format = format ?? new();
    }

    public string Text { get; set; }

    public OutputFormat Format { get; set; }

    public int ActualWidth => Format.Width ?? (Text.Length + PaddingWhenWidthIsNotSet);

    public int PaddingWhenWidthIsNotSet => Format.Alignment switch
    {
        null
        or TextAlignment.Left => Format.Padding ?? Format.PaddingLeft ?? 0,
        TextAlignment.Right => Format.Padding ?? Format.PaddingRight ?? 0,
        TextAlignment.Center => 0,
        _ => throw new Exception("Invalid alignment.")
    };

    public void Apply(OutputFormat format)
    {
        Format = Format.Apply(format);
    }

    public void Print(OutputFormat? format = null)
    {
        if (format is null)
        {
            Format.Print(Text);
        }
        else
        {
            Format.Apply(format).Print(Text);
        }
    }

    public override string ToString() => Format.ApplyToText(Text);
}
