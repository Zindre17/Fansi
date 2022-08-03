using System.Text;

namespace Monline;

public static class Printer
{
    public static void PrintPokemon(params Pokemon[] mons)
    {
        var consoleAreas = new PokemonConsoleArea[mons.Length];
        for (int i = 0; i < mons.Length; i++)
        {
            consoleAreas[i] = new PokemonConsoleArea(mons[i], i % 2 is 1);
        }

        for (int i = 0; i < PokemonConsoleArea.height; i++)
        {
            foreach (var pokemon in consoleAreas)
            {
                pokemon.PrintRow(i);
            }
            Console.WriteLine();
        }
    }
}
