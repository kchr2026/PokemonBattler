namespace PokemonBattler;

public class PokemonParty
{
    public List<Pokemon> Party { get; set; }

    public PokemonParty()
    {
        Party = new List<Pokemon>();
    }

    public void AddPokemon(Pokemon pokemon)
    {
        Party.Add(pokemon);
    }

    public Pokemon GetPokemon(int index)
    {
        return Party[index];
    }
}