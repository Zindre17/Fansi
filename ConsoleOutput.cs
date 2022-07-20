namespace Monline;

public class ConsoleOutput
{
    public ConsoleOutput(string text)
    {
        Text = text;
    }

    public string Text { get; }

    public bool Bold { get; set; }
    public bool Italics { get; set; }
    public bool Inverse { get; set; }
    public bool Underline { get; set; }
    public bool StrikeThrough { get; set; }
    public bool Blinking { get; set; }

    public int? Width { get; set; }
    public Alignment Alignment { get; set; } = Alignment.Center;

    public int? Padding { get; set; }
    public int? PaddingLeft { get; set; }
    public int? PaddingRight { get; set; }

    public int? SimpleForeground { get; set; }
    public int? SimpleBackground { get; set; }

    public Color? Foreground { get; set; }
    public Color? Background { get; set; }

    public bool ResetColorAfter { get; set; } = true;

    public void Print() => Console.Write(this);

    public override string ToString()
    {
        var ansi = GetAnsi();
        var alignedText = GetAlignedText();
        var leftPad = new string(' ', Padding ?? PaddingLeft ?? 0);
        var rightPad = new string(' ', Padding ?? PaddingRight ?? 0);
        var reset = ResetColorAfter ? $"{AnsiStart}{ResetAllArg}{AnsiStop}" : "";

        return $"{ansi}{leftPad}{alignedText}{rightPad}{reset}";
    }

    private string GetAlignedText() => Alignment switch
    {
        Alignment.Center => GetCentered(),
        Alignment.Left => GetLeftAligned(),
        Alignment.Right => GetRightAligned(),
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
        if (Width is null)
        {
            return Text;
        }

        var pad = Width.Value - Text.Length;
        return $"{Text}{new string(' ', pad)}";
    }

    private string GetRightAligned()
    {
        if (Width is null)
        {
            return Text;
        }

        var pad = Width.Value - Text.Length;
        return $"{new string(' ', pad)}{Text}";
    }

    private string GetCentered()
    {
        if (Width is null)
        {
            return Text;
        }

        var width = Width.Value;
        var padCount = width - Text.Length;
        var leftPadCount = padCount / 2;
        var leftPad = new string(' ', leftPadCount);
        var rightPad = new string(' ', leftPadCount + (padCount % 2));

        return $"{leftPad}{Text}{rightPad}";
    }


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

public enum Alignment
{
    Center,
    Left,
    Right
}
