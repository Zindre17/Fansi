# Fansi

Create fancy console output, easily.

___

## Demo
Code:
```csharp
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

format.Print("Hello");
inverse.Print("Fansi");
format.Print("World");
inverse.PrintLine("!");
```
Console output:

![Hello fansi world!][1]

[1]: https://raw.githubusercontent.com/Zindre17/Fansi/main/Fansi/Images/hello-fansi-world.png
[1]: Images/hello-fansi-world.png

---

Code: 
```csharp
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

var rows = new[] { 
    "Short",
    "Longeeerrr",
    "Loooooooooooooooonnnnnnnnggggggggg"
};

for (int i = 1; i <= rows.Length; i++)
{
    nrStyle.Print(i + new string('0', i));
    valueStyle.PrintLine(rows[i - 1]);
}
```
Console output:

![Aligned and sized output][2]

[2]: https://raw.githubusercontent.com/Zindre17/Fansi/main/Fansi/Images/format-and-alignment.png
[2]: Images/format-and-alignment.png

---

Code: 
```csharp
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

var selectedSelectorStyle = selectorStyle with {
    BackgroundRgb = selectedBackgroundColor
};
var selectedItemStyle = itemStyle with {
    BackgroundRgb = selectedBackgroundColor 
};


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
```
Console output:

![Styled list selection][3]

[3]: https://raw.githubusercontent.com/Zindre17/Fansi/main/Fansi/Images/selector.png
[3]: Images/selector.png



## How does it work?
It makes it possible to define a format, which 
mostly uses ANSI escape sequences to style output, but also has the capability to align and pad the text.
