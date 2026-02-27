// See https://aka.ms/new-console-template for more information

using PokemonBattler.Data;
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
        var charizard1 = new Pokemon("Charizard", 50, PokemonType.Fire, PokemonType.Flying,
            new Stats { HP = 78, Attack = 84, Defense = 78, SpecialAttack = 109, SpecialDefense = 85, Speed = 100 });

        var blastoise = new Pokemon("Blastoise", 50, PokemonType.Water, PokemonType.Flying,
            new Stats { HP = 78, Attack = 84, Defense = 78, SpecialAttack = 109, SpecialDefense = 85, Speed = 100 });
        var blastoise1 = new Pokemon("Blastoise", 50, PokemonType.Water, PokemonType.Flying,
            new Stats { HP = 78, Attack = 84, Defense = 78, SpecialAttack = 109, SpecialDefense = 85, Speed = 100 });
        
        charizard.Moves.Add(MoveData.Get("Flamethrower"));
        charizard.Moves.Add(MoveData.Get("Air Slash"));
        charizard.Moves.Add(MoveData.Get("Dragon Claw"));
        charizard.Moves.Add(MoveData.Get("Fly"));

        blastoise.Moves.Add(MoveData.Get("Surf"));
        blastoise.Moves.Add(MoveData.Get("Ice Beam"));
        blastoise.Moves.Add(MoveData.Get("Dig"));
        
        charizard1.Moves.Add(MoveData.Get("Flamethrower"));
        charizard1.Moves.Add(MoveData.Get("Air Slash"));
        charizard1.Moves.Add(MoveData.Get("Dragon Claw"));
        charizard1.Moves.Add(MoveData.Get("Fly"));

        blastoise1.Moves.Add(MoveData.Get("Surf"));
        blastoise1.Moves.Add(MoveData.Get("Ice Beam"));
        blastoise1.Moves.Add(MoveData.Get("Dig"));

        player.Party.AddPokemon(charizard);
        player.Party.AddPokemon(charizard1);

        // NPC trainer
        var npc = new Trainer("Red");
        
        npc.Party.AddPokemon(blastoise);
        npc.Party.AddPokemon(blastoise1);

        var engine = new BattleEngine();
        engine.Battle(player, npc);
    }
}
    