namespace Fansi;

/// <summary>
///     <para>
///         A reusable text formatter that is independent of the text is should format.
///     </para>
///     <para>
///         It defines rules for how to format any text, and provides methods for applying this format to a given text.
///     </para>
/// </summary>
public record OutputFormat
{
    /// <summary>
    ///     Make the text bold, when true.
    /// </summary>
    public bool? Bold { get; init; }

    /// <summary>
    ///     Dim or fade the text, when true.
    /// </summary>
    public bool? Dim { get; init; }

    /// <summary>
    ///     Make the text italic, when true.
    /// </summary>
    public bool? Italics { get; init; }

    /// <summary>
    ///     Underline the text, when true.
    /// </summary>
    public bool? Underline { get; init; }

    /// <summary>
    ///     Make the text blink, when true.
    /// </summary>
    /// <remarks>
    ///     Note that support for this in terminals is limited.
    /// </remarks>
    public bool? Blinking { get; init; }

    /// <summary>
    ///     Swap foreground and background colors, when true.
    /// </summary>
    public bool? Inverse { get; init; }

    /// <summary>
    ///     Hide text, when true.
    /// </summary>
    public bool? Hidden { get; init; }

    /// <summary>
    ///     Add a strike through decoration to the text, when true.
    /// </summary>
    public bool? StrikeThrough { get; init; }

    /// <summary>
    ///     Sets a constant width for the text.
    /// </summary>
    /// <remarks>
    ///     It has to be greater than the set padding, depending on text alignment.
    /// </remarks>
    public int? Width { get; init; }

    /// <summary>
    ///     Specifies how to align the text within the available width.
    /// </summary>
    public TextAlignment? Alignment { get; init; }

    /// <summary>
    ///     Amount of padding to use on either side.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Padding is only applied to one side depending on <see cref="Alignment" />,
    ///         when <see cref="Width" /> is set.
    ///     </para>
    ///     <para>
    ///         Padding is ignored when <see cref="Alignment" /> is set to <see cref="TextAlignment.Center" />.
    ///     </para>
    /// </remarks>
    public int? Padding { get; init; }

    /// <summary>
    ///     Amount of padding to use on the left side.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This property overrides <see cref="Padding" /> for the left side, if both are set.
    ///     </para>
    ///     <para>
    ///         Padding is ignored when <see cref="Alignment" /> is set to <see cref="TextAlignment.Center" />.
    ///     </para>
    /// </remarks>
    public int? PaddingLeft { get; init; }

    /// <summary>
    ///     Amount of padding to use on the right side.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This property overrides <see cref="Padding" /> for the right side, if both are set.
    ///     </para>
    ///     <para>
    ///         Padding is ignored when <see cref="Alignment" /> is set to <see cref="TextAlignment.Center" />.
    ///     </para>
    /// </remarks>
    public int? PaddingRight { get; init; }

    /// <summary>
    ///     Add ellipsis (...) at the end, when text overflows the set <see cref="Width" />.
    /// </summary>
    public bool? AddEllipsisToOverflow { get; init; }

    /// <summary>
    ///     Sets foreground (text) color from a preset 16 color options
    ///     (17 if you include <see cref="BasicColor.Default" />).
    /// </summary>
    /// <remarks>
    ///     Overrides <see cref="Foreground256" /> and <see cref="ForegroundRgb" />.
    /// </remarks>
    public BasicColor? Foreground { get; init; }

    /// <summary>
    ///     Sets foreground (text) color from a preset 256 color options (0 - 255).
    /// </summary>
    /// <remarks>
    ///     Overrides <see cref="ForegroundRgb" />.
    /// </remarks>
    public byte? Foreground256 { get; init; }

    /// <summary>
    ///     Sets foreground (text) color to any 24-bit RGB color.
    /// </summary>
    public Color? ForegroundRgb { get; init; }

    /// <summary>
    ///     Sets background color from a preset 16 color options
    ///     (17 if you include <see cref="BasicColor.Default" />).
    /// </summary>
    /// <remarks>
    ///     Overrides <see cref="Background256" /> and <see cref="BackgroundRgb" />.
    /// </remarks>
    public BasicColor? Background { get; init; }

    /// <summary>
    ///     Sets background color from a preset 256 color options (0 - 255).
    /// </summary>
    /// <remarks>
    ///     Overrides <see cref="BackgroundRgb" />.
    /// </remarks>
    public byte? Background256 { get; init; }

    /// <summary>
    ///     Sets backgrund color to any 24-bit RGB color.
    /// </summary>
    public Color? BackgroundRgb { get; init; }

    /// <summary>
    ///     Reset all colors and styles at the end.
    /// </summary>
    public bool? ResetAllAfter { get; init; }

    /// <summary>
    ///     Calculate width of a given text, when width, alignment, and padding is applied.
    /// </summary>
    /// <param name="text">
    ///     The text to measure on.
    /// </param>
    /// <returns>
    ///     The width of the given text after applying this format to it.
    /// </returns>
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

    /// <summary>
    ///     Apply this format to the given text.
    /// </summary>
    /// <param name="text">
    ///     The text to apply this format on.
    /// </param>
    /// <returns>
    ///     The formatted text.
    /// </returns>
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

        var options = GetAnsiOptionsString();
        var alignedText = GetAlignedText(text);
        var reset = string.IsNullOrEmpty(options) ? "" : GetAnsiResetString();
        return $"{options}{alignedText}{reset}";
    }

    /// <summary>
    ///     Format the given text and write it to console (no extra newline character at the end).
    /// </summary>
    /// <param name="text">
    ///     The text to format.
    /// </param>
    public void Print(string text)
    {
        Console.Write(ApplyToText(text));
    }

    /// <summary>
    ///     Format the given text, append a newline character and write it to console.
    /// </summary>
    /// <param name="text">
    ///     The text to format.
    /// </param>
    public void PrintLine(string text)
    {
        Console.WriteLine(ApplyToText(text));
    }

    /// <summary>
    ///     Combine two formats by creating a copy of this, 
    ///     and applying any properties of the other that are not explicitly set in this.
    /// </summary>
    /// <param name="other">
    ///     The format to enrich this with.
    /// </param>
    /// <returns>
    ///     A combination of both formats, in a new instance.
    /// </returns>
    public OutputFormat EnrichWith(OutputFormat other) => this with
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
        var leftPad = PaddingLeft ?? Padding ?? 0;
        if (Width is not null && text.Length > Width)
        {
            text = Truncate(text, Width.Value - leftPad, AddEllipsisToOverflow ?? true);
        }
        var rightPad = (Width - text.Length - leftPad) ?? PaddingRight ?? Padding ?? 0;

        return $"{Pad(leftPad)}{text}{Pad(rightPad)}";
    }

    private string GetRightAligned(string text)
    {
        var rightPad = PaddingRight ?? Padding ?? 0;
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
