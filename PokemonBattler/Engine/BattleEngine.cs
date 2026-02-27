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

        while (player.Party.Party.Any(p => p.HP > 0) && npc.Party.Party.Any(p => p.HP > 0))
        {
            DisplayStatus(playerPokemon, npcPokemon);

            var playerMove = playerPokemon.QueuedMove ?? ChooseMove(playerPokemon);
            var npcMove = npcPokemon.QueuedMove ?? npcPokemon.Moves[_rng.Next(npcPokemon.Moves.Count)];

            if (playerPokemon.BaseStats.Speed >= npcPokemon.BaseStats.Speed)
            {
                ExecuteMove(playerPokemon, npcPokemon, playerMove);
                if (npcPokemon.HP <= 0)
                {
                    AnsiConsole.MarkupLine($"[red]{npcPokemon.Name} fainted![/]");
                    var next = npc.Party.Party.FirstOrDefault(p => p.HP > 0);
                    if (next != null) { npcPokemon = next; AnsiConsole.MarkupLine($"[grey]Red sends out {npcPokemon.Name}![/]"); }
                }
                if (npcPokemon.HP > 0)
                {
                    ExecuteMove(npcPokemon, playerPokemon, npcMove);
                    if (playerPokemon.HP <= 0)
                    {
                        AnsiConsole.MarkupLine($"[red]{playerPokemon.Name} fainted![/]");
                        var next = player.Party.Party.FirstOrDefault(p => p.HP > 0);
                        if (next != null) playerPokemon = ChoosePokemon(player);
                    }
                }
            }
            else
            {
                ExecuteMove(npcPokemon, playerPokemon, npcMove);
                if (playerPokemon.HP <= 0)
                {
                    AnsiConsole.MarkupLine($"[red]{playerPokemon.Name} fainted![/]");
                    var next = player.Party.Party.FirstOrDefault(p => p.HP > 0);
                    if (next != null) playerPokemon = ChoosePokemon(player);
                }
                if (playerPokemon.HP > 0)
                {
                    ExecuteMove(playerPokemon, npcPokemon, playerMove);
                    if (npcPokemon.HP <= 0)
                    {
                        AnsiConsole.MarkupLine($"[red]{npcPokemon.Name} fainted![/]");
                        var next = npc.Party.Party.FirstOrDefault(p => p.HP > 0);
                        if (next != null) { npcPokemon = next; AnsiConsole.MarkupLine($"[grey]Red sends out {npcPokemon.Name}![/]"); }
                    }
                }
            }
        }

        if (player.Party.Party.Any(p => p.HP > 0))
            AnsiConsole.MarkupLine("[green]You won![/]");
        else
            AnsiConsole.MarkupLine("[red]You lost![/]");
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
        // If the move is a two-turn move and the attacker isn't charging yet
        if (move.ChargesInto != null && attacker.InvulnerableState == InvulnerableState.None)
        {
            attacker.InvulnerableState = move.ChargesInto.Value;
            attacker.QueuedMove = move;
            AnsiConsole.MarkupLine($"[yellow]{attacker.Name} is charging up {move.Name}![/]");
            return;
        }

        // Second turn — execute the queued move
        if (attacker.QueuedMove != null)
        {
            move = attacker.QueuedMove;
            attacker.InvulnerableState = InvulnerableState.None;
            attacker.QueuedMove = null;
        }

        // Check if defender is invulnerable
        if (defender.InvulnerableState != InvulnerableState.None)
        {
            bool canHit = defender.InvulnerableState switch
            {
                InvulnerableState.Underground => move.Type == PokemonType.Ground, // Earthquake/Magnitude
                InvulnerableState.Airborne => move.Type == PokemonType.Electric || move.Name == "Gust" || move.Name == "Twister", // Thunder, Gust etc.
                InvulnerableState.Underwater => move.Type == PokemonType.Water, // Surf/Whirlpool
                _ => false
            };

            if (!canHit)
            {
                AnsiConsole.MarkupLine($"[yellow]{attacker.Name} used {move.Name}... but it missed![/]");
                return;
            }
        }

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