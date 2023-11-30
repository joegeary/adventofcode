using System.Reflection;

namespace AdventOfCode;

internal interface ISolver
{
    object PartOne(string input);
    object PartTwo(string input) => null;
}

internal static class ISolverExtensions
{

    public static IEnumerable<object> Solve(this ISolver solver, string input)
    {
        yield return solver.PartOne(input);
        var res = solver.PartTwo(input);
        if (res != null)
        {
            yield return res;
        }
    }

    public static string GetName(this ISolver solver)
    {
        return (
            solver
                .GetType()
                .GetCustomAttribute(typeof(ProblemName)) as ProblemName
        ).Name;
    }

    public static string DayName(this ISolver solver)
    {
        return $"Day {solver.Day()}";
    }

    public static int Year(this ISolver solver)
    {
        return Year(solver.GetType());
    }

    public static int Year(Type t)
    {
        return int.Parse(t.FullName.Split('.')[1].Substring(1));
    }
    public static int Day(this ISolver solver)
    {
        return Day(solver.GetType());
    }

    public static int Day(Type t)
    {
        return int.Parse(t.FullName.Split('.')[2].Substring(3));
    }

    public static string WorkingDir(int year)
    {
        return Path.Combine(year.ToString());
    }

    public static string WorkingDir(int year, int day)
    {
        return Path.Combine(WorkingDir(year), "Day" + day.ToString("00"));
    }

    public static string WorkingDir(this ISolver solver)
    {
        return WorkingDir(solver.Year(), solver.Day());
    }

    public static ISplashScreen SplashScreen(this ISolver solver)
    {
        var tsplashScreen = Assembly.GetEntryAssembly().GetTypes()
             .Where(t => t.GetTypeInfo().IsClass && typeof(ISplashScreen).IsAssignableFrom(t))
             .Single(t => Year(t) == solver.Year());
        return (ISplashScreen)Activator.CreateInstance(tsplashScreen);
    }
}

internal static class SolverHelper
{
    public static Type[] GetSolverTypes()
    {
        return Assembly.GetEntryAssembly()!.GetTypes()
            .Where(t => t.GetTypeInfo().IsClass && typeof(ISolver).IsAssignableFrom(t))
            .OrderBy(t => t.FullName)
            .ToArray();
    }

    public static ISolver[] GetSolvers(Type[] types)
    {
        return types.Select(t => Activator.CreateInstance(t) as ISolver).ToArray();
    }

}
