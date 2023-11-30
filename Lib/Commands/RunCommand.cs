using System.CommandLine;

namespace AdventOfCode.Commands;

internal class RunCommand : Command
{
    public RunCommand() : base("run", "Run a specific day")
    {
        var date = SharedOptions.RunDate;
        var session = SharedOptions.Session;
        var step = SharedOptions.Step;

        AddArgument(date);
        AddOption(session);
        AddOption(step);

        this.SetHandler(Execute, date, session, step);
    }

    private void Execute(string date, string session, int? step)
    {
        var (year, day) = ParseDate(date);
        var solvers = SolverHelper.GetSolverTypes()
            .Where(s => (year is null || ISolverExtensions.Year(s) == year) && (day is null || ISolverExtensions.Day(s) == day))
            .ToArray();

        Runner.RunAll(SolverHelper.GetSolvers(solvers));
    }

    private (int? year, int? day) ParseDate(string date)
    {
        if (date == "all")
        {
            return (null, null);
        }

        if (date == "today")
        {
            var now = DateTime.Now;
            return now.Month != 12
                ? throw new InvalidOperationException("Advent of Code is only active from Dec 1st - 25th!")
                : (now.Year, now.Day);
        }

        if (!date.Contains('/'))
        {
            return (int.Parse(date), null);
        }

        var parts = date.Split('/');
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    }
}
