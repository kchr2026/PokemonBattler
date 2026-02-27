using Spectre.Console;

namespace PokemonBattler;

public class UI
{
    // public static string AskPlayerForName()
    // {
    //
    //     return AnsiConsole.Ask<string>("What is your name?");
    //     
    // }
    // public int Battle(Object player, Object npc)
    // {
    //     var pokemon = new
    //     {
    //         Name = "Pikachu",
    //         Level = 25,
    //         Type = "Electric",
    //         HP = 80,
    //         Moves = new[] { "Thunderbolt", "Quick Attack", "Iron Tail", "Thunder Wave" }
    //     };
    //
    //     AnsiConsole.Clear();
    //     AnsiConsole.Write(
    //         new FigletText("POKéMON BATTLE")
    //             .LeftJustified()
    //             .Color(Color.Yellow));
    //
    //     var moveChoice = AnsiConsole.Prompt(
    //         new SelectionPrompt<string>()
    //             .Title($"[grey]What will {pokemon.Name} do?[/]")
    //             .PageSize(10)
    //             .AddChoices(pokemon.Moves));
    //
    //     AnsiConsole.WriteLine($"\n[bold green]{pokemon.Name}[/] used [bold yellow]{moveChoice}[/]!");       
    //     return 0;
    // }
}