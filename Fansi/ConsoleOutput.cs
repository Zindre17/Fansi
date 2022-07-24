namespace Monline;

public record ConsoleOutput
{
    public ConsoleOutput(string? text = null)
    {
        Text = text ?? string.Empty;
    }

    public string Text { get; init; }

    public bool Bold { get; init; }
    public bool Italics { get; init; }
    public bool Inverse { get; init; }
    public bool Underline { get; init; }
    public bool StrikeThrough { get; init; }
    public bool Blinking { get; init; }

    public int? Width { get; init; }
    public TextAlignment? Alignment { get; init; }

    public int? Padding { get; init; }
    public int? PaddingLeft { get; init; }
    public int? PaddingRight { get; init; }

    public int? SimpleForeground { get; init; }
    public int? SimpleBackground { get; init; }

    public Color? Foreground { get; init; }
    public Color? Background { get; init; }

    public bool ResetColorAfter { get; init; } = true;

    public int ActualWidth => Width ?? (Text.Length + ActualPadding);

    public int ActualPadding => Alignment switch
    {
        null
        or TextAlignment.Left => Padding ?? PaddingLeft ?? 0,
        TextAlignment.Right => Padding ?? PaddingRight ?? 0,
        TextAlignment.Center => 0,
        _ => throw new Exception("Invalid alignment.")
    };

    public void Print() => Console.Write(this);

    public override string ToString()
    {
        var ansi = GetAnsi();
        var alignedText = GetAlignedText();
        var reset = ResetColorAfter ? $"{AnsiStart}{ResetAllArg}{AnsiStop}" : "";

        return $"{ansi}{alignedText}{reset}";
    }

    public ConsoleOutput Apply(ConsoleOutput other) => this with
    {
        Alignment = Alignment ?? other.Alignment,
        Background = Background ?? other.Background,
        Blinking = Blinking || other.Blinking,
        Bold = Bold || other.Bold,
        Foreground = Foreground ?? other.Foreground,
        Inverse = Inverse || other.Inverse,
        Italics = Italics || other.Italics,
        Padding = Padding ?? other.Padding,
        PaddingLeft = PaddingLeft ?? other.PaddingLeft,
        PaddingRight = PaddingRight ?? other.PaddingRight,
        ResetColorAfter = ResetColorAfter || other.ResetColorAfter,
        SimpleBackground = SimpleBackground ?? other.SimpleBackground,
        SimpleForeground = SimpleForeground ?? other.SimpleForeground,
        StrikeThrough = StrikeThrough || other.StrikeThrough,
        Underline = Underline || other.Underline,
        Width = Width ?? other.Width,
    };

    private string GetAlignedText() => Alignment switch
    {
        null
        or TextAlignment.Left => GetLeftAligned(),
        TextAlignment.Right => GetRightAligned(),
        TextAlignment.Center => GetCentered(),
        _ => throw new Exception("Not a valid alignment position.")
    };

    private string GetAnsi()
    {
        var args = new List<string>();

        if (Bold)
        {
            args.Add(BoldArg.ToString());
        }

        if (Italics)
        {
            args.Add(ItalicsArg.ToString());
        }

        if (Underline)
        {
            args.Add(UnderlineArg.ToString());
        }

        if (Blinking)
        {
            args.Add(BlinkingArg.ToString());
        }

        if (StrikeThrough)
        {
            args.Add(StrikeThroughArg.ToString());
        }

        if (Inverse)
        {
            args.Add(InverseArg.ToString());
        }

        if (SimpleBackground is not null)
        {
            args.Add($"{BackgroundArg};{C256Arg};{SimpleBackground}");
        }
        else if (Background is not null)
        {
            args.Add($"{BackgroundArg};{RgbArg};{Background.Red};{Background.Green};{Background.Blue}");
        }

        if (SimpleForeground is not null)
        {
            args.Add($"{ForegroundArg};{C256Arg};{SimpleForeground}");
        }
        if (Foreground is not null)
        {
            args.Add($"{ForegroundArg};{RgbArg};{Foreground.Red};{Foreground.Green};{Foreground.Blue}");
        }

        var ansiArgs = string.Join(';', args);

        return string.IsNullOrEmpty(ansiArgs) ? "" : $"{AnsiStart}{ansiArgs}{AnsiStop}";
    }

    private string GetLeftAligned()
    {
        var leftPad = Padding ?? PaddingLeft ?? 0;
        var rightPad = (Width - Text.Length - leftPad) ?? Padding ?? PaddingRight ?? 0;

        return $"{Pad(leftPad)}{Text}{Pad(rightPad)}";
    }

    private string GetRightAligned()
    {
        var rightPad = Padding ?? PaddingRight ?? 0;
        var leftPad = (Width - Text.Length - rightPad) ?? Padding ?? PaddingLeft ?? 0;

        return $"{Pad(leftPad)}{Text}{Pad(rightPad)}";
    }

    private string GetCentered()
    {
        var padCount = (Width ?? Text.Length) - Text.Length;
        var leftPad = padCount / 2;
        var rightPad = leftPad + (padCount % 2);

        return $"{Pad(leftPad)}{Text}{Pad(rightPad)}";
    }

    private static string Pad(int count) => new(' ', count);

    // Colors
    private const int BackgroundArg = 48;
    private const int ForegroundArg = 38;
    private const int C256Arg = 5;
    private const int RgbArg = 2;

    // Styling
    private const int BoldArg = 1;
    private const int ItalicsArg = 3;
    private const int UnderlineArg = 4;
    private const int BlinkingArg = 5;
    private const int InverseArg = 7;
    private const int StrikeThroughArg = 9;

    // Common
    private const string AnsiStart = "\x1b[";
    private const char AnsiStop = 'm';
    private const int ResetAllArg = 0;
}

public record Color(int Red, int Green, int Blue)
{
    public static Color White => new(0xff, 0xff, 0xff);
    public static Color Black => new(0, 0, 0);
}

public enum TextAlignment
{
    Center,
    Left,
    Right
}
