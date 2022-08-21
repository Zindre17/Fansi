using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fansi.Test;

[TestClass]
public class StyleTests
{
    private const string test = "testing";

    [TestMethod]
    public void Bold()
    {
        var bold = new OutputFormat
        {
            Bold = true
        };

        var output = bold.ApplyToText(test);
        Assert.AreEqual($"\x1b[1m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void Italics()
    {
        var italic = new OutputFormat
        {
            Italics = true
        };

        var output = italic.ApplyToText(test);
        Assert.AreEqual($"\x1b[3m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void Underline()
    {
        var underlined = new OutputFormat
        {
            Underline = true
        };

        var output = underlined.ApplyToText(test);
        Assert.AreEqual($"\x1b[4m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void Blinking()
    {
        var blinking = new OutputFormat
        {
            Blinking = true
        };

        var output = blinking.ApplyToText(test);
        Assert.AreEqual($"\x1b[5m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void Inverse()
    {
        var striked = new OutputFormat
        {
            Inverse = true
        };

        var output = striked.ApplyToText(test);
        Assert.AreEqual($"\x1b[7m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void StrikeThrough()
    {
        var striked = new OutputFormat
        {
            StrikeThrough = true
        };

        var output = striked.ApplyToText(test);
        Assert.AreEqual($"\x1b[9m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void Dim()
    {
        var striked = new OutputFormat
        {
            Dim = true
        };

        var output = striked.ApplyToText(test);
        Assert.AreEqual($"\x1b[2m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void Hidden()
    {
        var striked = new OutputFormat
        {
            Hidden = true
        };

        var output = striked.ApplyToText(test);
        Assert.AreEqual($"\x1b[8m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void Background_Trumps_Background256_And_BackgroundRgb()
    {
        var allBackgrounds = new OutputFormat
        {
            Background = BasicColor.White,
            BackgroundRgb = Color.Black,
            Background256 = 200
        };
        var output = allBackgrounds.ApplyToText(test);
        Assert.AreEqual($"\x1b[47m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void Background256_Trumps_BackgroundRgb()
    {
        var bothBackgrounds = new OutputFormat
        {
            BackgroundRgb = Color.Black,
            Background256 = 200
        };
        var output = bothBackgrounds.ApplyToText(test);
        Assert.AreEqual($"\x1b[48;5;200m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void Foreground_Trumps_Foreground256_And_ForegroundRgb()
    {
        var bothBackgrounds = new OutputFormat
        {
            Foreground = BasicColor.White,
            ForegroundRgb = Color.Black,
            Foreground256 = 200
        };
        var output = bothBackgrounds.ApplyToText(test);
        Assert.AreEqual($"\x1b[37m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void Foreground256_Trumps_ForegroundRgb()
    {
        var bothBackgrounds = new OutputFormat
        {
            ForegroundRgb = Color.Black,
            Foreground256 = 200
        };
        var output = bothBackgrounds.ApplyToText(test);
        Assert.AreEqual($"\x1b[38;5;200m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void Background()
    {
        var background = new OutputFormat
        {
            Background = BasicColor.Black
        };

        var output = background.ApplyToText(test);
        Assert.AreEqual($"\x1b[40m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void Background256()
    {
        var background = new OutputFormat
        {
            Background256 = 100
        };

        var output = background.ApplyToText(test);
        Assert.AreEqual($"\x1b[48;5;100m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void BackgroundRgb()
    {
        var background = new OutputFormat
        {
            BackgroundRgb = Color.Black,
        };

        var output = background.ApplyToText(test);
        Assert.AreEqual($"\x1b[48;2;0;0;0m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void Foreground()
    {
        var foreground = new OutputFormat
        {
            Foreground = BasicColor.Black
        };

        var output = foreground.ApplyToText(test);
        Assert.AreEqual($"\x1b[30m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void Foreground256()
    {
        var foreground = new OutputFormat
        {
            Foreground256 = 100
        };

        var output = foreground.ApplyToText(test);
        Assert.AreEqual($"\x1b[38;5;100m{test}\x1b[0m", output);
    }

    [TestMethod]
    public void ForegroundRgb()
    {
        var foreground = new OutputFormat
        {
            ForegroundRgb = Color.Black
        };

        var output = foreground.ApplyToText(test);
        Assert.AreEqual($"\x1b[38;2;0;0;0m{test}\x1b[0m", output);
    }
}
