using Fansi;

namespace Monline;

public static class Printer
{
    private readonly static Color mainBackground = new(40, 40, 40);
    private readonly static Color secondaryBackground = new(60, 60, 60);

    public static void PrintPokemon(params Pokemon[] mons)
    {
        var consoleAreas = new ConsoleArea[mons.Length];
        for (int i = 0; i < mons.Length; i++)
        {
            consoleAreas[i] = new ConsoleArea(14, 30)
            {
                Common = new()
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

            console[0].Fill(new(""));
            console[1].Fill(new(mon.Name.Capitalized())
            {
                Bold = true,
                Alignment = TextAlignment.Center,
            });
            console[2].Fill(new(""));

            var typeFormat = new ConsoleOutput
            {
                Italics = true,
                Alignment = TextAlignment.Center,
            };

            console[3].AddSegment(
                typeFormat with
                {
                    Text = mon.Types[0].Capitalized(),
                    Background = TranslateTypeToColor(mon.Types[0])
                },
                1d / mon.Types.Length
            );

            if (mon.Types.Length > 1)
            {
                console[3].AddSegment(
                    typeFormat with
                    {
                        Text = mon.Types[1].Capitalized(),
                        Background = TranslateTypeToColor(mon.Types[1])
                    },
                    0.5
                );
            }

            var isEven = i % 2 is 0;
            AddStat(console[4], mon.Stats.Hp, isEven ? mainBackground : secondaryBackground);
            AddStat(console[5], mon.Stats.Attack, isEven ? secondaryBackground : mainBackground);
            AddStat(console[6], mon.Stats.Defence, isEven ? mainBackground : secondaryBackground);
            AddStat(console[7], mon.Stats.SpecialAttack, isEven ? secondaryBackground : mainBackground);
            AddStat(console[8], mon.Stats.SpecialDefence, isEven ? mainBackground : secondaryBackground);
            AddStat(console[9], mon.Stats.Speed, isEven ? secondaryBackground : mainBackground);

            console[10].Fill(new(""));

            AddStat(console[11], mon.Stats.Sum, isEven ? secondaryBackground : mainBackground);

            console[12].Fill(new(""));
            console[13].Fill(new(""));
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

    private static void AddStat(ConsoleRow row, Stat stat, Color background)
    {
        row.Common = row.Common with
        {
            Background = background
        };

        row.AddSegment(
            new(stat.Name)
            {
                Alignment = TextAlignment.Right,
                PaddingRight = 1,
            },
            .5
        );
        row.AddSegment(
            new($"{stat.Value}")
            {
                Alignment = TextAlignment.Right,
                Width = 4
            }
        );
        row.AddSegment(
            new(stat.Effort > 0 ? $"+{stat.Effort}" : "")
            {
                PaddingLeft = 1,
            }
        );
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
