using Monline;

var api = new ApiClient();

var calls = new List<Task<Pokemon?>>();
foreach (var arg in args)
{
    calls.Add(api.GetPokemonAsync(arg));
};

var potentialMons = await Task.WhenAll(calls);

var mons = potentialMons.OfType<Pokemon>().ToArray();

Printer.PrintPokemon(mons);

