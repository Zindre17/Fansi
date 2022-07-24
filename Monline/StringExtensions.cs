namespace System;

public static class StringExtensions
{
    public static string Capitalized(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            throw new ArgumentException("String cannot be empty.");
        }
        return char.ToUpperInvariant(str[0]) + str[1..];
    }
}
