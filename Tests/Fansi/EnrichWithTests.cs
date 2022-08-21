using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fansi.Test;

[TestClass]
public class EnrichWithTests
{
    [TestMethod]
    public void Enrich_With_Other_Format()
    {
        var baseFormat = new OutputFormat
        {
            Width = 20,
            Alignment = TextAlignment.Center,
            BackgroundRgb = Color.Black
        };
        var formatToApply = new OutputFormat
        {
            Width = 15,
            ForegroundRgb = Color.White
        };
        var expectedResult = baseFormat with { ForegroundRgb = Color.White };

        Assert.AreEqual(expectedResult, baseFormat.EnrichWith(formatToApply));
    }

    [TestMethod]
    public void No_Override_Width()
    {
        var format = new OutputFormat { Width = 20 };
        var other = new OutputFormat { Width = 10 };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Aligment()
    {
        var format = new OutputFormat { Alignment = TextAlignment.Center };
        var other = new OutputFormat { Alignment = TextAlignment.Right };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Ellipsis()
    {
        var format = new OutputFormat { AddEllipsisToOverflow = false };
        var other = new OutputFormat { AddEllipsisToOverflow = true };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Background()
    {
        var format = new OutputFormat { Background = BasicColor.Black };
        var other = new OutputFormat { Background = BasicColor.White };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Background256()
    {
        var format = new OutputFormat { Background256 = 10 };
        var other = new OutputFormat { Background256 = 20 };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_BackgroundRgb()
    {
        var format = new OutputFormat { BackgroundRgb = Color.White };
        var other = new OutputFormat { BackgroundRgb = Color.Black };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Blinking()
    {
        var format = new OutputFormat { Blinking = false };
        var other = new OutputFormat { Blinking = true };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Bold()
    {
        var format = new OutputFormat { Bold = false };
        var other = new OutputFormat { Bold = true };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Italics()
    {
        var format = new OutputFormat { Italics = false };
        var other = new OutputFormat { Italics = true };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Strike_Through()
    {
        var format = new OutputFormat { StrikeThrough = false };
        var other = new OutputFormat { StrikeThrough = true };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Dim()
    {
        var format = new OutputFormat { Dim = false };
        var other = new OutputFormat { Dim = true };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Inverse()
    {
        var format = new OutputFormat { Inverse = false };
        var other = new OutputFormat { Inverse = true };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Underline()
    {
        var format = new OutputFormat { Underline = false };
        var other = new OutputFormat { Underline = true };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Hidden()
    {
        var format = new OutputFormat { Hidden = false };
        var other = new OutputFormat { Hidden = true };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Foreground()
    {
        var format = new OutputFormat { Foreground = BasicColor.Black };
        var other = new OutputFormat { Foreground = BasicColor.White };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Foreground256()
    {
        var format = new OutputFormat { Foreground256 = 20 };
        var other = new OutputFormat { Foreground256 = 10 };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_ForegroundRgb()
    {
        var format = new OutputFormat { ForegroundRgb = Color.Black };
        var other = new OutputFormat { ForegroundRgb = Color.White };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Padding()
    {
        var format = new OutputFormat { Padding = 2 };
        var other = new OutputFormat { Padding = 5 };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Padding_Left()
    {
        var format = new OutputFormat { PaddingLeft = 2 };
        var other = new OutputFormat { PaddingLeft = 5 };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Padding_Right()
    {
        var format = new OutputFormat { PaddingRight = 2 };
        var other = new OutputFormat { PaddingRight = 5 };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void No_Override_Reset_All_After()
    {
        var format = new OutputFormat { ResetAllAfter = false };
        var other = new OutputFormat { ResetAllAfter = true };

        var enriched = format.EnrichWith(other);

        Assert.AreEqual(format, enriched);
    }

    [TestMethod]
    public void Override_All_Non_Set_Properties()
    {
        var emtpy = new OutputFormat();
        var full = new OutputFormat
        {
            AddEllipsisToOverflow = true,
            Alignment = TextAlignment.Center,
            Background = BasicColor.Black,
            Background256 = 10,
            BackgroundRgb = Color.White,
            Blinking = true,
            Bold = true,
            Dim = true,
            Foreground = BasicColor.White,
            Foreground256 = 20,
            ForegroundRgb = Color.Black,
            Hidden = true,
            Inverse = true,
            Italics = true,
            Padding = 3,
            PaddingLeft = 3,
            PaddingRight = 3,
            ResetAllAfter = true,
            StrikeThrough = true,
            Underline = true,
            Width = 10
        };

        Assert.AreEqual(full, emtpy.EnrichWith(full));
    }
}
