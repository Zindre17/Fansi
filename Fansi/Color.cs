namespace Fansi;

public record Color(int Red, int Green, int Blue)
{
    public static Color White => new(0xff, 0xff, 0xff);
    public static Color Black => new(0, 0, 0);
}
