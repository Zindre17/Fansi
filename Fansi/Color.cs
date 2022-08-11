namespace Fansi;

/// <summary>
///     Represents a 24-bit RGB color.
/// </summary>
/// <param name="Red">
///     Value for the red channel.
/// </param>
/// <param name="Green">
///     Value for the green channel.
/// </param>
/// <param name="Blue">
///     Value for the blue channel.
/// </param>
/// <returns>
///     A <see cref="Color" /> instance.
/// </returns>
public record Color(byte Red, byte Green, byte Blue)
{
    /// <summary>
    /// Pure white.
    /// </summary>
    /// <returns>
    ///     A <see cref="Color" /> instance that is pure white.
    /// </returns>
    public static Color White => new(0xff, 0xff, 0xff);

    /// <summary>
    /// Pure black.
    /// </summary>
    /// <returns>
    ///     A <see cref="Color" /> instance that is pure black.
    /// </returns>
    public static Color Black => new(0, 0, 0);

    /// <summary>
    ///     Makes a string where each color channel is separated by ';'.
    /// </summary>
    public override string ToString()
    {
        return $"{Red};{Green};{Blue}";
    }
}

/// <summary>
///     All possible predefined colors.
/// </summary>
public enum BasicColor
{
    /// <summary>Black.</summary>
    Black = 30,
    /// <summary>Red.</summary>
    Red = 31,
    /// <summary>Green.</summary>
    Green = 32,
    /// <summary>Yellow.</summary>
    Yellow = 33,
    /// <summary>Blue.</summary>
    Blue = 34,
    /// <summary>Magenta.</summary>
    Magenta = 35,
    /// <summary>Cyan.</summary>
    Cyan = 36,
    /// <summary>White.</summary>
    White = 37,

    /// <summary>Default.</summary>
    Default = 39,

    /// <summary>BrigthBlack.</summary>
    BrigthBlack = 90,
    /// <summary>BrightRed.</summary>
    BrightRed = 91,
    /// <summary>BrightGreen.</summary>
    BrightGreen = 92,
    /// <summary>BrightYellow.</summary>
    BrightYellow = 93,
    /// <summary>BrightBlue.</summary>
    BrightBlue = 94,
    /// <summary>BrightMagenta.</summary>
    BrightMagenta = 95,
    /// <summary>BrightCyan.</summary>
    BrightCyan = 96,
    /// <summary>BrightWhite.</summary>
    BrightWhite = 97
}

internal static class BasicColorExtensions
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
