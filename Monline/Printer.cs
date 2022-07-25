using Fansi;

namespace Monline;

public static class Printer
{
    private readonly static Color mainBackground = new(40, 40, 40);
    private readonly static Color secondaryBackground = new(60, 60, 60);

    private readonly static OutputFormat nameStyle = new()
    {
        Bold = true,
        Alignment = TextAlignment.Center,
    };

    private readonly static OutputFormat typeStyle = new()
    {
        Italics = true,
        Alignment = TextAlignment.Center,
    };

    public static void PrintPokemon(params Pokemon[] mons)
    {
        var consoleAreas = new ConsoleArea[mons.Length];
        for (int i = 0; i < mons.Length; i++)
        {
            consoleAreas[i] = new ConsoleArea(30)
            {
                CommonFormat = new()
                {
                    Background = i % 2 is 0 ? mainBackground : secondaryBackground,
                    Foreground = Color.White,
                }
            };
        }

        for (int i = 0; i < mons.Length; i++)
        {
            var console = consoleAreas[i];
            var mon = mons[i];

            console.AddNewRow();
            console.AddRow(mon.Name.Capitalized(), nameStyle);
            console.AddNewRow();

            var typeRow = console.AddNewRow();
            var type1 = mon.Types[0];
            typeRow.AddSegment(type1.Capitalized(), GetTypeFormat(type1), 1d / mon.Types.Length);

            if (mon.Types.Length > 1)
            {
                var type2 = mon.Types[1];
                typeRow.AddSegment(type2.Capitalized(), GetTypeFormat(type2), 0.5);
            }

            var counter = i;
            bool isEven;
            foreach (var stat in mon.Stats)
            {
                isEven = counter++ % 2 is 0;
                AddStat(console.AddNewRow(), stat, isEven ? mainBackground : secondaryBackground);
            }

            console.AddNewRow();

            isEven = counter++ % 2 is 0;
            AddStat(console.AddNewRow(), mon.Stats.Sum, isEven ? secondaryBackground : mainBackground);

            console.AddNewRow();
            console.AddNewRow();
        }

        for (int i = 0; i < 14; i++)
        {
            foreach (var console in consoleAreas)
            {
                console.PrintRow(i);
            }
            Console.WriteLine();
        }
    }

    private static OutputFormat GetTypeFormat(string type)
    {
        return typeStyle.Apply(new() { Background = TranslateTypeToColor(type) });
    }

    private static readonly OutputFormat statNameFormat = new()
    {
        Alignment = TextAlignment.Right,
        PaddingRight = 1,
    };

    private static readonly OutputFormat statValueFormat = new()
    {
        Alignment = TextAlignment.Right,
        Width = 4
    };

    private static void AddStat(ConsoleRow row, Stat stat, Color background)
    {
        row.Apply(new() { Background = background });

        row.AddSegment(stat.Name, statNameFormat, 0.5);
        row.AddSegment(stat.Value.ToString(), statValueFormat);
        row.AddSegment(stat.Effort > 0 ? $" +{stat.Effort}" : "");
    }

    public static Color TranslateTypeToColor(string type)
    {
        // Grabbed from Serebii.net
        return type switch
        {
            "normal" => new(0xad, 0xa5, 0x94),
            "fire" => new(0xf7, 0x52, 0x31),
            "water" => new(0x39, 0x9c, 0xff),
            "grass" => new(0x7b, 0xce, 0x52),
            "electric" => new(0xff, 0xc6, 0x31),
            "ice" => new(0x5a, 0xce, 0xe7),
            "poison" => new(0xb5, 0x5a, 0xa5),
            "ground" => new(0xd6, 0xb5, 0x5a),
            "rock" => new(0xbd, 0xa5, 0x5a),
            "flying" => new(0x9c, 0xad, 0xf7),
            "bug" => new(0xad, 0xbd, 0x21),
            "psychic" => new(0xff, 0x73, 0xa5),
            "ghost" => new(0x63, 0x63, 0xb5),
            "steel" => new(0xad, 0xad, 0xc6),
            "dark" => new(0x73, 0x5a, 0x4a),
            "dragon" => new(0x7b, 0x63, 0xe7),
            "fighting" => new(0xa5, 0x52, 0x39),
            "fairy" => new(0xf7, 0xb5, 0xf7),
            _ => throw new Exception($"Type {type} does not exist.")
        };
    }
}
