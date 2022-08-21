using System.Net.Mime;
using Fansi;

var format = new OutputFormat
{
    Background = BasicColor.Cyan,
    Foreground = BasicColor.BrigthBlack,
    Bold = true,
    Padding = 4,
};

var inverse = format with
{
    Inverse = true,
    Bold = false,
    Italics = true,
};
Console.WriteLine();

format.Print("Hello");
inverse.Print("Fansi");
format.Print("World");
inverse.PrintLine("!");

Console.WriteLine();
Console.WriteLine();

var nrStyle = new OutputFormat
{
    Width = 4,
    Alignment = TextAlignment.Right
};

var valueStyle = new OutputFormat
{
    Width = 20,
    PaddingLeft = 2,
    AddEllipsisToOverflow = true
};

var rows = new[] { "Short", "Longeeerrr", "Loooooooooooooooonnnnnnnnggggggggg" };

for (int i = 1; i <= rows.Length; i++)
{
    nrStyle.Print(i + new string('0', i));
    valueStyle.PrintLine(rows[i - 1]);
}

Console.WriteLine();
Console.WriteLine();

var selectorStyle = new OutputFormat
{
    ForegroundRgb = new(0, 150, 40),
    Alignment = TextAlignment.Center,
    Width = 4
};
var itemStyle = new OutputFormat
{
    Foreground = BasicColor.Blue,
    PaddingRight = 1
};

var selectedBackgroundColor = new Color(60, 60, 60);

var selectedSelectorStyle = selectorStyle with { BackgroundRgb = selectedBackgroundColor };
var selectedItemStyle = itemStyle with { BackgroundRgb = selectedBackgroundColor };


var items = new[] { "item 1", "item 2", "item 3" };
var selectorPosition = 1;
for (var i = 0; i < items.Length; i++)
{
    if (i == selectorPosition)
    {
        selectedSelectorStyle.Print("->");
        selectedItemStyle.PrintLine(items[i]);
    }
    else
    {
        selectorStyle.Print("");
        itemStyle.PrintLine(items[i]);
    }
}

Console.WriteLine();
