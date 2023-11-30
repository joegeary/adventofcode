using System.CommandLine;
using System.Reflection;

namespace AdventOfCode.Commands;

internal class CalendarsCommand : Command
{
    public CalendarsCommand() : base("calendars", "Show the calendars")
    {
        this.SetHandler(Execute);
    }

    private void Execute()
    {
        var tsolvers = Assembly.GetEntryAssembly()!.GetTypes()
            .Where(t => t.GetTypeInfo().IsClass && typeof(ISolver).IsAssignableFrom(t))
            .OrderBy(t => t.FullName)
            .ToArray();

        var tsolversSelected = (
                from tsolver in tsolvers
                group tsolver by ISolverExtensions.Year(tsolver) into g
                orderby ISolverExtensions.Year(g.First()) descending
                select g.First()
            ).ToArray();

        var solvers = tsolversSelected
            .Select(t => Activator.CreateInstance(t) as ISolver)
            .ToArray();

        foreach (var solver in solvers)
        {
            solver.SplashScreen().Show();
        }
    }
}
