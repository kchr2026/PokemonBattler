namespace PokemonBattler.Data;

public static class MoveData
{ 
    private static readonly Dictionary<string, Move> Moves = new()
    {
        ["Flamethrower"] = new Move("Flamethrower", PokemonType.Fire, 90, 100, 15, 15, true),
        ["Air Slash"]    = new Move("Air Slash", PokemonType.Flying, 75, 95, 15, 15, false),
        ["Dragon Claw"]  = new Move("Dragon Claw", PokemonType.Dragon, 80, 100, 15, 15, false),
        ["Slash"]        = new Move("Slash", PokemonType.Normal, 70, 100, 20, 20, false),
        ["Fly"]          = new Move("Fly", PokemonType.Flying, 90, 95, 15, 15, false) { ChargesInto = InvulnerableState.Airborne },
        ["Surf"]         = new Move("Surf", PokemonType.Water, 90, 100, 15, 15, true),
        ["Ice Beam"]     = new Move("Ice Beam", PokemonType.Ice, 90, 100, 10, 10, true),
        ["Dig"]          = new Move("Dig", PokemonType.Ground, 80, 100, 10, 10, false) { ChargesInto = InvulnerableState.Underground },
    };

    public static Move Get(string name)
    {
        if (!Moves.ContainsKey(name))
            throw new ArgumentException($"Move '{name}' not found in MoveData.");

        // Return a fresh copy so PP changes don't affect the template
        var m = Moves[name];
        return new Move(m.Name, m.Type, m.Power, m.Accuracy, m.PP, m.MaxPP, m.IsSpecial) 
            { ChargesInto = m.ChargesInto };
    }
}