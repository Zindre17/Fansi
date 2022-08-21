
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fansi.Test;

[TestClass]
public class AlignmentTests
{
    private const string test = "testing";

    [TestMethod]
    public void Empty_Format_Does_Nothing()
    {
        var emptyFormat = new OutputFormat();
        var result = emptyFormat.ApplyToText(test);

        Assert.AreEqual(test, result);
    }

    [TestMethod]
    public void Explicit_Width()
    {
        var widthFormat = new OutputFormat
        {
            Width = 10
        };
        var result = widthFormat.ApplyToText(test);

        Assert.AreEqual(test + new string(' ', 3), result);
    }

    [TestMethod]
    public void Alignment_Without_Explicit_Width_Does_Nothing()
    {
        var center = new OutputFormat
        {
            Alignment = TextAlignment.Center
        };
        var left = new OutputFormat
        {
            Alignment = TextAlignment.Left
        };
        var right = new OutputFormat
        {
            Alignment = TextAlignment.Right
        };

        Assert.AreEqual(test, center.ApplyToText(test));
        Assert.AreEqual(test, left.ApplyToText(test));
        Assert.AreEqual(test, right.ApplyToText(test));
    }

    [TestMethod]
    public void Alignment_With_Explicit_Width()
    {
        var width = new OutputFormat
        {
            Width = 10
        };

        var center = width with
        {
            Alignment = TextAlignment.Center
        };
        var left = width with
        {
            Alignment = TextAlignment.Left
        };
        var right = width with
        {
            Alignment = TextAlignment.Right
        };

        Assert.AreEqual($" {test}  ", center.ApplyToText(test));
        Assert.AreEqual($"{test}   ", left.ApplyToText(test));
        Assert.AreEqual($"   {test}", right.ApplyToText(test));
    }

    [TestMethod]
    public void Alignment_Center_Even_And_Odd_Padding()
    {
        var centerOdd = new OutputFormat
        {
            Alignment = TextAlignment.Center,
            Width = 10
        };
        var centerEven = new OutputFormat
        {
            Alignment = TextAlignment.Center,
            Width = 20
        };

        Assert.AreEqual($" {test}  ", centerOdd.ApplyToText(test));
        Assert.AreEqual($"   {test + test}   ", centerEven.ApplyToText(test + test));
    }

    [TestMethod]
    public void Width_Overflow_Left_Aligned()
    {
        var widthLimited = new OutputFormat
        {
            Width = 6,
            AddEllipsisToOverflow = true,
            Alignment = TextAlignment.Left
        };
        var padded = widthLimited with { Padding = 2 };
        var paddedNoEllipsis = padded with { AddEllipsisToOverflow = false };
        var noRoomForEllipsis = widthLimited with { Padding = 4 };
        var noRoomForText = widthLimited with { Padding = 6 };

        Assert.AreEqual("tes...", widthLimited.ApplyToText(test));
        Assert.AreEqual("  t...", padded.ApplyToText(test));
        Assert.AreEqual("  test", paddedNoEllipsis.ApplyToText(test));
        Assert.AreEqual("    te", noRoomForEllipsis.ApplyToText(test));
        Assert.ThrowsException<InvalidOperationException>(() => noRoomForText.ApplyToText(test));
    }

    [TestMethod]
    public void Width_Overflow_Right_Aligned()
    {
        var widthLimited = new OutputFormat
        {
            Width = 6,
            AddEllipsisToOverflow = true,
            Alignment = TextAlignment.Right
        };
        var padded = widthLimited with { Padding = 2 };
        var paddedNoEllipsis = padded with { AddEllipsisToOverflow = false };
        var noRoomForEllipsis = widthLimited with { Padding = 4 };
        var noRoomForText = widthLimited with { Padding = 6 };

        Assert.AreEqual("tes...", widthLimited.ApplyToText(test));
        Assert.AreEqual("t...  ", padded.ApplyToText(test));
        Assert.AreEqual("test  ", paddedNoEllipsis.ApplyToText(test));
        Assert.AreEqual("te    ", noRoomForEllipsis.ApplyToText(test));
        Assert.ThrowsException<InvalidOperationException>(() => noRoomForText.ApplyToText(test));
    }

    [TestMethod]
    public void Width_Overflow_Centered()
    {
        var widthLimited = new OutputFormat
        {
            Width = 6,
            AddEllipsisToOverflow = true,
            Alignment = TextAlignment.Center
        };
        var noEllipsis = widthLimited with { AddEllipsisToOverflow = false };
        var padded = widthLimited with { Padding = 2 };

        Assert.AreEqual("tes...", widthLimited.ApplyToText(test));
        Assert.AreEqual("tes...", padded.ApplyToText(test));
        Assert.AreEqual("testin", noEllipsis.ApplyToText(test));
    }
}
