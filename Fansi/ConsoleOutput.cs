namespace Fansi;

public class ConsoleOutput
{
    public ConsoleOutput(string? text = null, OutputOptions? options = null)
    {
        Text = text ?? string.Empty;
        Options = options ?? new();
    }

    public string Text { get; set; }

    public OutputOptions Options { get; set; }

    public int ActualWidth => Options.Width ?? (Text.Length + PaddingWhenWidthIsNotSet);

    public int PaddingWhenWidthIsNotSet => Options.Alignment switch
    {
        null
        or TextAlignment.Left => Options.Padding ?? Options.PaddingLeft ?? 0,
        TextAlignment.Right => Options.Padding ?? Options.PaddingRight ?? 0,
        TextAlignment.Center => 0,
        _ => throw new Exception("Invalid alignment.")
    };

    public void Apply(OutputOptions options)
    {
        Options = Options.Apply(options);
    }

    public void Print(OutputOptions? options = null)
    {
        if (options is null)
        {
            Options.Print(Text);
        }
        else
        {
            Options.Apply(options).Print(Text);
        }
    }

    public override string ToString() => Options.ApplyToText(Text);
}
