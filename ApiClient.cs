using System.Net.Http.Json;

namespace Monline;

public class ApiClient
{
    private readonly HttpClient client;

    public ApiClient()
    {
        client = new()
        {
            BaseAddress = new Uri("https://pokeapi.co/api/v2/")
        };
    }

    public async Task<Pokemon?> GetPokemonAsync(string name)
    {
        var res = await client.GetAsync($"pokemon/{name}");
        return res.IsSuccessStatusCode
            ? new(await res.Content.ReadFromJsonAsync<PokemonResponse>()
                ?? throw new Exception("Unable to deserialize response json."))
            : null;
    }

}

public record Pokemon
{
    public Pokemon(PokemonResponse res)
    {
        Name = res.Name ?? throw new ArgumentException("Response did not contain a name.");
        Stats = new Stats(res.Stats ?? throw new ArgumentException("Response did not contain stats."));
        Types = res.Types?.Select(t => t.Type?.Name).OfType<string>().ToArray() ?? throw new ArgumentException("No types found.");
    }

    public string Name { get; init; }
    public string[] Types { get; init; }
    public Stats Stats { get; init; }
}


public record Stats
{
    public Stats(StatResponse[] stats)
    {
        if (stats.Length < 6)
        {
            throw new ArgumentException("Not 6 stats.");
        }

        var stat = stats[0];
        Hp = new(stat.Base_Stat, stat.Effort);

        stat = stats[1];
        Attack = new(stat.Base_Stat, stat.Effort);

        stat = stats[2];
        Defence = new(stat.Base_Stat, stat.Effort);

        stat = stats[3];
        SpecialAttack = new(stat.Base_Stat, stat.Effort);

        stat = stats[4];
        SpecialDefence = new(stat.Base_Stat, stat.Effort);

        stat = stats[5];
        Speed = new(stat.Base_Stat, stat.Effort);
    }

    public Stat Hp { get; init; }
    public Stat Attack { get; init; }
    public Stat Defence { get; init; }
    public Stat SpecialAttack { get; init; }
    public Stat SpecialDefence { get; init; }
    public Stat Speed { get; init; }
}

public record Stat(int Value, int Effort);

public record PokemonResponse
{
    public string? Name { get; init; }
    public StatResponse[]? Stats { get; init; }
    public TypeResponse[]? Types { get; init; }
}

public record TypeResponse
{
    public int Slot { get; init; }
    public NamedResource? Type { get; init; }
}

public record StatResponse
{
    public NamedResource? Stat { get; init; }
    public int Base_Stat { get; init; }
    public int Effort { get; init; }
}

public record NamedResource
{
    public string? Name { get; init; }
    public string? Url { get; init; }
}
