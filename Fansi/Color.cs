namespace Fansi;

public record Color(byte Red, byte Green, byte Blue)
{
    public static Color White => new(0xff, 0xff, 0xff);
    public static Color Black => new(0, 0, 0);

    public override string ToString()
    {
        return $"{Red};{Green};{Blue}";
    }
}

public enum BasicColor
{
    Black = 30,
    Red = 31,
    Green = 32,
    Yellow = 33,
    Blue = 34,
    Magenta = 35,
    Cyan = 36,
    White = 37,
    Default = 39,
    Reset = 0,

    BrigthBlack = 90,
    BrightRed = 91,
    BrightGreen = 92,
    BrightYellow = 93,
    BrightBlue = 94,
    BrightMagenta = 95,
    BrightCyan = 96,
    BrightWhite = 97
}

public static class BasicColorExtensions
{
    public static int ToBackgroundInt(this BasicColor color)
    {
        return (int)color + 10;
    }

    public static int ToForegroundInt(this BasicColor color)
    {
        return (int)color;
    }
}
