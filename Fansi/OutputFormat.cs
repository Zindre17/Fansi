namespace Fansi;

public record OutputFormat
{
    public bool? Bold { get; init; }
    public bool? Dim { get; init; }
    public bool? Italics { get; init; }
    public bool? Underline { get; init; }
    public bool? Blinking { get; init; }
    public bool? Inverse { get; init; }
    public bool? Hidden { get; init; }
    public bool? StrikeThrough { get; init; }

    public int? Width { get; init; }
    public TextAlignment? Alignment { get; init; }

    public int? Padding { get; init; }
    public int? PaddingLeft { get; init; }
    public int? PaddingRight { get; init; }

    public int? SimpleForeground { get; init; }
    public int? SimpleBackground { get; init; }

    public Color? Foreground { get; init; }
    public Color? Background { get; init; }

    public bool? ResetAllAfter { get; init; }

    public int CalculateWidth(string text)
    {
        return Width ?? (text.Length + Alignment switch
        {
            null
            or TextAlignment.Left
            or TextAlignment.Right => (PaddingRight ?? Padding ?? 0) + (PaddingLeft ?? Padding ?? 0),
            TextAlignment.Center => 0,
            _ => throw new ArgumentOutOfRangeException("Undefined text alignment.", nameof(Alignment))
        });
    }

    public string ApplyToText(string text)
    {
        var options = GetAnsiOptionsString();
        var alignedText = GetAlignedText(text);
        var reset = string.IsNullOrEmpty(options) ? "" : GetAnsiResetString();
        return $"{options}{alignedText}{reset}";
    }

    public void Print(string text)
    {
        Console.Write(ApplyToText(text));
    }

    public OutputFormat Apply(OutputFormat other) => this with
    {
        Alignment = Alignment ?? other.Alignment,
        Background = Background ?? other.Background,
        Blinking = Blinking ?? other.Blinking,
        Bold = Bold ?? other.Bold,
        Dim = Dim ?? other.Dim,
        Foreground = Foreground ?? other.Foreground,
        Hidden = Hidden ?? other.Hidden,
        Inverse = Inverse ?? other.Inverse,
        Italics = Italics ?? other.Italics,
        Padding = Padding ?? other.Padding,
        PaddingLeft = PaddingLeft ?? other.PaddingLeft,
        PaddingRight = PaddingRight ?? other.PaddingRight,
        ResetAllAfter = ResetAllAfter ?? other.ResetAllAfter,
        SimpleBackground = SimpleBackground ?? other.SimpleBackground,
        SimpleForeground = SimpleForeground ?? other.SimpleForeground,
        StrikeThrough = StrikeThrough ?? other.StrikeThrough,
        Underline = Underline ?? other.Underline,
        Width = Width ?? other.Width,
    };

    private string GetAnsiOptionsString()
    {
        var args = new List<string>();

        if (Bold ?? false)
        {
            args.Add(BoldArg.ToString());
        }

        if (Dim ?? false)
        {
            args.Add(DimArg.ToString());
        }

        if (Italics ?? false)
        {
            args.Add(ItalicsArg.ToString());
        }

        if (Underline ?? false)
        {
            args.Add(UnderlineArg.ToString());
        }

        if (Blinking ?? false)
        {
            args.Add(BlinkingArg.ToString());
        }

        if (Inverse ?? false)
        {
            args.Add(InverseArg.ToString());
        }

        if (Hidden ?? false)
        {
            args.Add(HiddenArg.ToString());
        }

        if (StrikeThrough ?? false)
        {
            args.Add(StrikeThroughArg.ToString());
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

        return GetAnsiString(args.ToArray());
    }

    private string GetAnsiResetString()
    {
        return ResetAllAfter ?? true ? GetAnsiString(ResetAllArg.ToString()) : "";
    }

    private static string GetAnsiString(params string[] args)
    {
        return args.Any() ? $"{AnsiStart}{string.Join(';', args)}{AnsiStop}" : "";
    }

    private string GetAlignedText(string text) => Alignment switch
    {
        null
        or TextAlignment.Left => GetLeftAligned(text),
        TextAlignment.Right => GetRightAligned(text),
        TextAlignment.Center => GetCentered(text),
        _ => throw new Exception("Not a valid alignment position.")
    };

    private string GetLeftAligned(string text)
    {
        var leftPad = Padding ?? PaddingLeft ?? 0;
        var rightPad = (Width - text.Length - leftPad) ?? PaddingRight ?? Padding ?? 0;

        return $"{Pad(leftPad)}{text}{Pad(rightPad)}";
    }

    private string GetRightAligned(string text)
    {
        var rightPad = Padding ?? PaddingRight ?? 0;
        var leftPad = (Width - text.Length - rightPad) ?? PaddingLeft ?? Padding ?? 0;

        return $"{Pad(leftPad)}{text}{Pad(rightPad)}";
    }

    private string GetCentered(string text)
    {
        var padCount = (Width - text.Length) ?? 0;
        var leftPad = padCount / 2;
        var rightPad = leftPad + (padCount % 2);

        return $"{Pad(leftPad)}{text}{Pad(rightPad)}";
    }

    private static string Pad(int count) => new(' ', count);

    // Colors
    private const int BackgroundArg = 48;
    private const int ForegroundArg = 38;
    private const int C256Arg = 5;
    private const int RgbArg = 2;

    // Styling
    private const int BoldArg = 1;
    private const int DimArg = 2;
    private const int ItalicsArg = 3;
    private const int UnderlineArg = 4;
    private const int BlinkingArg = 5;
    private const int InverseArg = 7;
    private const int HiddenArg = 8;
    private const int StrikeThroughArg = 9;

    // Common
    private const string AnsiStart = "\x1b[";
    private const char AnsiStop = 'm';
    private const int ResetAllArg = 0;
}
