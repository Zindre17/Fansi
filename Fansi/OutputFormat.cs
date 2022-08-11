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

    public bool? AddEllipsisToOverflow { get; init; }

    public BasicColor? Foreground { get; init; }
    public int? Foreground256 { get; init; }
    public Color? ForegroundRgb { get; init; }

    public BasicColor? Background { get; init; }
    public int? Background256 { get; init; }
    public Color? BackgroundRgb { get; init; }

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
        if (Width is not null)
        {
            if (Width <= 0)
            {
                throw new InvalidOperationException("Width can not be 0 or less.");
            }
            switch (Alignment)
            {
                case null
                  or TextAlignment.Left when (Padding ?? PaddingLeft ?? 0) >= Width:
                    throw new InvalidOperationException("Width must be greater than left padding.");
                case TextAlignment.Right when (Padding ?? PaddingRight ?? 0) >= Width:
                    throw new InvalidOperationException("Width must be greater than right padding.");
            };
        }

        if (Foreground256 > 255 || Background256 > 256)
        {
            throw new InvalidOperationException("Foreground256 and Background256 can only be in the range 0 through 255 (8-bit).");
        }

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
        AddEllipsisToOverflow = AddEllipsisToOverflow ?? other.AddEllipsisToOverflow,
        Background = Background ?? other.Background,
        Background256 = Background256 ?? other.Background256,
        BackgroundRgb = BackgroundRgb ?? other.BackgroundRgb,
        Blinking = Blinking ?? other.Blinking,
        Bold = Bold ?? other.Bold,
        Dim = Dim ?? other.Dim,
        Foreground = Foreground ?? other.Foreground,
        Foreground256 = Foreground256 ?? other.Foreground256,
        ForegroundRgb = ForegroundRgb ?? other.ForegroundRgb,
        Hidden = Hidden ?? other.Hidden,
        Inverse = Inverse ?? other.Inverse,
        Italics = Italics ?? other.Italics,
        Padding = Padding ?? other.Padding,
        PaddingLeft = PaddingLeft ?? other.PaddingLeft,
        PaddingRight = PaddingRight ?? other.PaddingRight,
        ResetAllAfter = ResetAllAfter ?? other.ResetAllAfter,
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

        if (Background is not null)
        {
            args.Add(Background.Value.ToBackgroundInt().ToString());
        }
        else if (Background256 is not null)
        {
            args.Add($"{Background256Args};{Background256}");
        }
        else if (BackgroundRgb is not null)
        {
            args.Add($"{BackgroundRgbArgs};{BackgroundRgb}");
        }

        if (Foreground is not null)
        {
            args.Add(Foreground.Value.ToForegroundInt().ToString());
        }
        else if (Foreground256 is not null)
        {
            args.Add($"{Foreground256Args};{Foreground256}");
        }
        else if (ForegroundRgb is not null)
        {
            args.Add($"{ForegroundRgbArgs};{ForegroundRgb}");
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
        if (Width is not null && text.Length > Width)
        {
            text = Truncate(text, Width.Value - leftPad, AddEllipsisToOverflow ?? true);
        }
        var rightPad = (Width - text.Length - leftPad) ?? PaddingRight ?? Padding ?? 0;

        return $"{Pad(leftPad)}{text}{Pad(rightPad)}";
    }

    private string GetRightAligned(string text)
    {
        var rightPad = Padding ?? PaddingRight ?? 0;
        if (Width is not null && text.Length > Width)
        {
            text = Truncate(text, Width.Value - rightPad, AddEllipsisToOverflow ?? true);
        }
        var leftPad = (Width - text.Length - rightPad) ?? PaddingLeft ?? Padding ?? 0;

        return $"{Pad(leftPad)}{text}{Pad(rightPad)}";
    }

    private string GetCentered(string text)
    {
        if (Width is not null && text.Length > Width)
        {
            text = Truncate(text, Width.Value, AddEllipsisToOverflow ?? true);
        }
        var padCount = (Width - text.Length) ?? 0;
        var leftPad = padCount / 2;
        var rightPad = leftPad + (padCount % 2);

        return $"{Pad(leftPad)}{text}{Pad(rightPad)}";
    }

    private static string Truncate(string text, int maxLength, bool addEllipsis)
    {
        if (maxLength < 0)
        {
            throw new ArgumentException("Must be grater than 0.", nameof(maxLength));
        }
        if (maxLength > 3 && addEllipsis)
        {
            return $"{text[..(maxLength - 3)]}...";
        }
        return text[..maxLength];
    }

    private static string Pad(int count) => new(' ', count);

    // Colors
    private const string ForegroundRgbArgs = $"38;2";
    private const string Foreground256Args = $"38;5";
    private const string BackgroundRgbArgs = $"48;2";
    private const string Background256Args = $"48;5";

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
