namespace PokemonBattler;

public class Move
{
    public string Name { get; set; }
    public PokemonType Type { get; set; }
    public int Power { get; set; }
    public int Accuracy { get; set; }
    public int PP { get; set; }
    public int MaxPP { get; set; }
    public bool IsSpecial { get; set; }

public Move(string name, PokemonType type, int power, int accuracy, int pp, int maxPP, bool isSpecial){
    this.Name = name;
    this.Type = type;
    this.Power = power;
    this.Accuracy = accuracy;
    this.PP = pp;
    this.MaxPP = maxPP;
    this.IsSpecial = isSpecial;
}


}