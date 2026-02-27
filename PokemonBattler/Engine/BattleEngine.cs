namespace PokemonBattler;
using Spectre.Console;
public class BattleEngine
{
    private Random _rng = new Random();

    public void Battle(Trainer player, Trainer npc)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("POKéMON BATTLE").LeftJustified().Color(Color.Yellow));

        var playerPokemon = ChoosePokemon(player);
        var npcPokemon = npc.Party.Party.First(p => p.HP > 0); // NPC sends first healthy pokemon

        AnsiConsole.WriteLine($"Go, {playerPokemon.Name}! vs {npcPokemon.Name}!\n");

        while (playerPokemon.HP > 0 && npcPokemon.HP > 0)
        {
            DisplayStatus(playerPokemon, npcPokemon);
            
            var playerMove = ChooseMove(playerPokemon);
            var npcMove = npcPokemon.Moves[_rng.Next(npcPokemon.Moves.Count)]; // NPC picks randomly

            // Speed determines who goes first
            if (playerPokemon.BaseStats.Speed >= npcPokemon.BaseStats.Speed)
            {
                ExecuteMove(playerPokemon, npcPokemon, playerMove);
                if (npcPokemon.HP > 0)
                    ExecuteMove(npcPokemon, playerPokemon, npcMove);
            }
            else
            {
                ExecuteMove(npcPokemon, playerPokemon, npcMove);
                if (playerPokemon.HP > 0)
                    ExecuteMove(playerPokemon, npcPokemon, playerMove);
            }
        }

        if (playerPokemon.HP > 0)
            AnsiConsole.MarkupLine($"[green]{playerPokemon.Name} wins![/]");
        else
            AnsiConsole.MarkupLine($"[red]{playerPokemon.Name} fainted![/]");
    }

    private Pokemon ChoosePokemon(Trainer trainer)
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[grey]Which Pokemon will you send out?[/]")
                .AddChoices(trainer.Party.Party.Select(p => p.Name)));

        return trainer.Party.Party.First(p => p.Name == choice);
    }

    private Move ChooseMove(Pokemon pokemon)
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[grey]What will {pokemon.Name} do?[/]")
                .AddChoices(pokemon.Moves.Select(m => $"{m.Name} (PP: {m.PP}/{m.MaxPP})")));

        // Match back to the actual Move object
        var moveName = choice.Split('(')[0].Trim();
        var move = pokemon.Moves.First(m => m.Name == moveName);
        move.PP--;
        return move;
    }

    private void ExecuteMove(Pokemon attacker, Pokemon defender, Move move)
    {
        // Miss check
        if (_rng.Next(100) >= move.Accuracy)
        {
            AnsiConsole.MarkupLine($"[yellow]{attacker.Name} used {move.Name}... but it missed![/]");
            return;
        }

        int damage = CalculateDamage(attacker, defender, move);
        defender.HP = Math.Max(0, defender.HP - damage);

        AnsiConsole.MarkupLine($"[bold]{attacker.Name}[/] used [yellow]{move.Name}[/]! Dealt [red]{damage}[/] damage.");
    }

    private int CalculateDamage(Pokemon attacker, Pokemon defender, Move move)
    {
        if (move.Power == 0) return 0; // status moves

        // Standard gen 3+ damage formula
        double attack = move.IsSpecial
            ? (move.Type == attacker.Type1 || move.Type == attacker.Type2 ? attacker.BaseStats.SpecialAttack * 1.5 : attacker.BaseStats.SpecialAttack)
            : (move.Type == attacker.Type1 || move.Type == attacker.Type2 ? attacker.BaseStats.Attack * 1.5 : attacker.BaseStats.Attack);

        double defense = move.IsSpecial ? defender.BaseStats.SpecialDefense : defender.BaseStats.Defense;

        double damage = ((2.0 * attacker.Level / 5 + 2) * move.Power * attack / defense) / 50 + 2;
        // Random factor (85-100%)
        damage *= _rng.Next(85, 101) / 100.0;

        return (int)damage;
    }

    private void DisplayStatus(Pokemon player, Pokemon npc)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[cyan]{player.Name}[/] HP: [green]{player.HP}/{player.MaxHP}[/]  |  [red]{npc.Name}[/] HP: [green]{npc.HP}/{npc.MaxHP}[/]");
        AnsiConsole.WriteLine();
    }
}