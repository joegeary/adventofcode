using System.CommandLine;

namespace AdventOfCode.Commands;

internal class UploadCommand : Command
{
    public UploadCommand() : base("upload", "Upload the answer for the selected year and day")
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

        var solvers = SolverHelper.GetSolverTypes()
            .Where(s => ISolverExtensions.Year(s) == year && ISolverExtensions.Day(s) == day)
            .ToArray();

        return new Updater().Upload(SolverHelper.GetSolvers(solvers)[0], session);
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
