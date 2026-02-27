// See https://aka.ms/new-console-template for more information
using Spectre.Console;
namespace PokemonBattler;
class Program
{

    
    static void Main(string[] args)
    {
        string playerName= "chad";
        var player = new Trainer(playerName);

        var charizard = new Pokemon("Charizard", 50, PokemonType.Fire, PokemonType.Flying,
            new Stats { HP = 78, Attack = 84, Defense = 78, SpecialAttack = 109, SpecialDefense = 85, Speed = 100 });

        var blastoise = new Pokemon("Blastoise", 50, PokemonType.Water, PokemonType.Flying,
            new Stats { HP = 78, Attack = 84, Defense = 78, SpecialAttack = 109, SpecialDefense = 85, Speed = 100 });
        
        charizard.Moves.Add(new Move("Flamethrower", PokemonType.Fire, 90, 100, 15, 15, true));
        charizard.Moves.Add(new Move("Air Slash", PokemonType.Flying, 75, 95, 15, 15, false));
        charizard.Moves.Add(new Move("Dragon Claw", PokemonType.Dragon, 80, 100, 15, 15, false));
        charizard.Moves.Add(new Move("Fly", PokemonType.Ground, 80, 100, 10, 10, false)
            { ChargesInto = InvulnerableState.Underground });

        blastoise.Moves.Add(new Move("Surf", PokemonType.Water, 90, 100, 15, 15, true));
        blastoise.Moves.Add(new Move("Ice Beam", PokemonType.Ice, 90, 100, 10, 10, true));
        blastoise.Moves.Add(new Move("Dig", PokemonType.Flying, 90, 95, 15, 15, false)
            { ChargesInto = InvulnerableState.Airborne });

        player.Party.AddPokemon(charizard);
        player.Party.AddPokemon(charizard);

        // NPC trainer
        var npc = new Trainer("Red");
        
        npc.Party.AddPokemon(blastoise);
        npc.Party.AddPokemon(blastoise);

        var engine = new BattleEngine();
        engine.Battle(player, npc);
    }
}
    