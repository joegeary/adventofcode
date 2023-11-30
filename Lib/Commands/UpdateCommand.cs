using System.CommandLine;

namespace AdventOfCode.Commands;

internal class UpdateCommand : Command
{
    public UpdateCommand() : base("update", "Prepares a folder for the given day, updates the input, the readme and creates a solution template.")
    {
        var date = SharedOptions.Date;
        var session = SharedOptions.Session;

        AddArgument(date);
        AddOption(session);

        this.SetHandler(Execute, date, session);
    }

    private Task Execute(string date, string session)
    {
        var (year, day) = ParseDate(date);
        return new Updater().Update(year, day, session);
    }

    private (int year, int day) ParseDate(string date)
    {
        if (date == "today")
        {
            var now = DateTime.Now;
            return now.Month != 12
                ? throw new InvalidOperationException("Advent of Code is only active from Dec 1st - 25th!")
                : (now.Year, now.Day);
        }

        var parts = date.Split('/');
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    }
}
