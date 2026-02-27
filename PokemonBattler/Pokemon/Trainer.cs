public class Trainer
{
    public string Name { get; set; }
    public PokemonParty Party { get; set; }

    public Trainer(string name)
    {
        Name = name;
        Party = new PokemonParty();
    }
}