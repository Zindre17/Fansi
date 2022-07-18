namespace Monline;

public static class Printer
{
    public static void PrintPokemon(params Pokemon[] mons)
    {
        const int width = 30;

        var builder = new ConsoleOutputBuilder(10);
        for (int i = 0; i < mons.Length; i++)
        {
            bool isEven = (i % 2) is 0;
            var mainColor = isEven ? ConsoleColor.White : ConsoleColor.Black;
            var secColor = isEven ? ConsoleColor.Black : ConsoleColor.White;
            var mon = mons[i];

            builder
                .AddOutput(0, GetBlock(mainColor, width))
                .AddOutput(1, new(GetCentered(mon.Name.Capitalized(), width), secColor, mainColor))
                .AddOutput(2, GetBlock(mainColor, width))

                .AddOutput(3, new(GetCentered(mon.Types[0].Capitalized(), width / mon.Types.Length), ConsoleColor.Black, TranslateTypeToColor(mon.Types[0])));

            if (mon.Types.Length > 1)
            {
                builder.AddOutput(3, new(GetCentered(mon.Types[1].Capitalized(), width / 2), ConsoleColor.Black, TranslateTypeToColor(mon.Types[1])));
            }

            builder
                .AddOutput(4, new(GetStatText("HP", mon.Stats.Hp), mainColor, secColor))
                .AddOutput(5, new(GetStatText("Attack", mon.Stats.Attack), secColor, mainColor))
                .AddOutput(6, new(GetStatText("Defence", mon.Stats.Defence), mainColor, secColor))
                .AddOutput(7, new(GetStatText("Sp. Attack", mon.Stats.SpecialAttack), secColor, mainColor))
                .AddOutput(8, new(GetStatText("Sp. Defence", mon.Stats.SpecialDefence), mainColor, secColor))
                .AddOutput(9, new(GetStatText("Speed", mon.Stats.Speed), secColor, mainColor));

        }

        builder.Print();
    }

    private static string GetCentered(string text, int width)
    {
        var padCount = width - text.Length;
        var leftPadCount = padCount / 2;
        var leftPad = new string(' ', leftPadCount);
        var rightPad = new string(' ', leftPadCount + (padCount % 2));

        return $"{leftPad}{text}{rightPad}";
    }

    private static Output GetBlock(ConsoleColor color, int width)
    {
        return new(new(' ', width), Background: color);
    }

    private static string GetStatText(string name, Stat stat)
    {
        return $"    {name,-14}{stat.Value,8}{(stat.Effort > 0 ? $"+{stat.Effort}" : ""),3} ";
    }

    public static ConsoleColor TranslateTypeToColor(string type)
    {
        return type switch
        {
            "normal" => ConsoleColor.White,
            "fire" => ConsoleColor.DarkRed,
            "water" => ConsoleColor.Blue,
            "grass" => ConsoleColor.Green,
            "electric" => ConsoleColor.Yellow,
            "ice" => ConsoleColor.Cyan,
            "poison" => ConsoleColor.Magenta,
            "ground" => ConsoleColor.Red,
            "rock" => ConsoleColor.DarkGray,
            "flying" => ConsoleColor.Gray,
            "bug" => ConsoleColor.DarkGreen,
            "psychic" => ConsoleColor.Magenta,
            "steel" => ConsoleColor.White,
            "dark" => ConsoleColor.DarkMagenta,
            "dragon" => ConsoleColor.DarkCyan,
            "fighting" => ConsoleColor.Red,
            "fairy" => ConsoleColor.Magenta,
            _ => throw new Exception($"Type {type} does not exist.")
        };
    }

    private class ConsoleOutputBuilder
    {
        private readonly List<List<Output>> output;

        public ConsoleOutputBuilder(int height)
        {
            output = new(height);
            for (int i = 0; i < height; i++)
            {
                output.Add(new());
            }
        }

        public ConsoleOutputBuilder AddOutput(int lineNumber, Output output)
        {
            var line = this.output[lineNumber];

            if (line is null)
            {
                line = new();
                this.output[lineNumber] = line;
            }

            line.Add(output);

            return this;
        }

        public void Print()
        {
            foreach (var line in output)
            {
                foreach (var segment in line)
                {
                    Console.ResetColor();

                    if (segment.Background is not null)
                    {
                        Console.BackgroundColor = segment.Background.Value;
                    }

                    if (segment.TextColor is not null)
                    {
                        Console.ForegroundColor = segment.TextColor.Value;
                    }

                    Console.Write(segment.Text);
                }

                Console.ResetColor();
                Console.WriteLine();
            }
        }
    }

    private record Output(string Text, ConsoleColor? TextColor = null, ConsoleColor? Background = null);

}
