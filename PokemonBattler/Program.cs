// See https://aka.ms/new-console-template for more information
using Spectre.Console;
namespace PokemonBattler;
class Program
{

    
    static void Main(string[] args)
    {
        var playerName = UI.AskPlayerForName();
        var player = new Trainer(playerName);

        var charizard = new Pokemon("Charizard", 50, PokemonType.Fire, PokemonType.Flying,
            new Stats { HP = 78, Attack = 84, Defense = 78, SpecialAttack = 109, SpecialDefense = 85, Speed = 100 });

        charizard.Moves.Add(new Move("Flamethrower", PokemonType.Fire, 90, 100, 15, 15));
        charizard.Moves.Add(new Move("Air Slash", PokemonType.Flying, 75, 95, 15, 15));
        charizard.Moves.Add(new Move("Dragon Claw", PokemonType.Dragon, 80, 100, 15, 15));
        charizard.Moves.Add(new Move("Slash", PokemonType.Normal, 70, 100, 20, 20));

        player.Party.AddPokemon(charizard);

        // NPC trainer
        var npc = new Trainer("Red");
        var blastoise = new Pokemon("Blastoise", 50, PokemonType.Water, null,
            new Stats { HP = 79, Attack = 83, Defense = 100, SpecialAttack = 85, SpecialDefense = 105, Speed = 78 });

        blastoise.Moves.Add(new Move("Surf", PokemonType.Water, 90, 100, 15, 15));
        blastoise.Moves.Add(new Move("Ice Beam", PokemonType.Ice, 90, 100, 10, 10));

        npc.Party.AddPokemon(blastoise);

        var engine = new BattleEngine();
        engine.Battle(player, npc);
    }
}
    