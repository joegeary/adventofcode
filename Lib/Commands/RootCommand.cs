using System.CommandLine;
using Spectre.Console;

namespace AdventOfCode.Commands;

internal class AocCli : RootCommand
{
    internal static void WelcomeMessage()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new FigletText("Advent Of Code").Color(Color.HotPink));
        AnsiConsole.MarkupLine("[bold lime]Yet another Advent of Code CLI tool...[/]");
        AnsiConsole.WriteLine();
    }

    public AocCli() : base("CLI for Advent of Code")
    {
        AddCommand(new RunCommand());
        AddCommand(new UpdateCommand());
        AddCommand(new UploadCommand());
        AddCommand(new CalendarsCommand());
    }
}

