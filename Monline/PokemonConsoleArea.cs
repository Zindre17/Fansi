using Fansi;

namespace Monline;

public class PokemonConsoleArea
{
    private const int width = 30;
    public const int height = 14;
    private const int TypeRowIndex = 3;
    private const int NameRowIndex = 1;
    private const int StatRowStartIndex = 4;
    private const int StatSumRowIndex = 11;

    private static readonly Color primaryColor = new(40, 40, 40);
    private static readonly Color secondaryColor = new(60, 60, 60);
    private readonly Color mainBackground = primaryColor;
    private readonly Color secondaryBackground = secondaryColor;

    private readonly static OutputFormat nameFormat = new()
    {
        Bold = true,
        Alignment = TextAlignment.Center,
    };

    private readonly ConsoleArea area = new(width, height);

    public PokemonConsoleArea(Pokemon mon, bool useSecondaryColors)
    {
        if (useSecondaryColors)
        {
            mainBackground = secondaryColor;
            secondaryBackground = primaryColor;
        }

        area.Apply(new() { Background = mainBackground, Foreground = Color.White });

        SetName(mon.Name);
        SetTypes(mon.Types);
        SetStats(mon.Stats);
    }

    public void PrintRow(int index)
    {
        area.PrintRow(index);
    }

    private void SetName(string name)
    {
        area[NameRowIndex].Fill(name.Capitalized(), nameFormat);
    }

    private void SetTypes(string[] types)
    {
        var type1 = types[0];
        area[TypeRowIndex].AddSegment(type1.Capitalized(), GetTypeFormat(type1), 1d / types.Length);

        if (types.Length > 1)
        {
            var type2 = types[1];
            area[TypeRowIndex].AddSegment(type2.Capitalized(), GetTypeFormat(type2), 0.5);
        }
    }

    private void SetStats(Stats stats)
    {
        var counter = 0;
        foreach (var stat in stats)
        {
            var isOdd = counter % 2 is 1;
            SetStat(stat, StatRowStartIndex + counter++, isOdd);
        }

        SetStat(stats.Sum, StatSumRowIndex, true);
    }

    private void SetStat(Stat stat, int rowIndex, bool useSecondaryColors)
    {
        var row = area[rowIndex];
        if (useSecondaryColors)
        {
            row.Apply(new() { Background = secondaryBackground });
        }

        row.AddSegment(stat.Name, statNameFormat, 0.5);
        row.AddSegment(stat.Value.ToString(), statValueFormat);
        row.AddSegment(stat.Effort > 0 ? $" +{stat.Effort}" : "");
    }

    private static OutputFormat GetTypeFormat(string type)
    {
        return typeStyle.Apply(new() { Background = TranslateTypeToColor(type) });
    }

    private readonly static OutputFormat typeStyle = new()
    {
        Italics = true,
        Alignment = TextAlignment.Center,
    };

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

    private static Color TranslateTypeToColor(string type)
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
