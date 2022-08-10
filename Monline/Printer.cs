using Fansi.Layout;

namespace Monline;

public static class Printer
{
    public static void PrintPokemon(params Pokemon[] mons)
    {
        var consoleArrangement = new ConsoleOutputArranger();

        for (int i = 0; i < mons.Length; i++)
        {
            consoleArrangement.Append(new PokemonConsoleArea(mons[i], i % 2 is 1));
        }

        consoleArrangement.Print();
    }
}
