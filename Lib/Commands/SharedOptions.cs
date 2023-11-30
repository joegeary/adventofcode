using System.CommandLine;

namespace AdventOfCode.Commands;

internal static class SharedOptions
{
    public static Argument<string> Date => new()
    {
        Name = "date",
        Description = "The specified problem(s)",
    };

    public static Argument<string> RunDate => new("[year]/[day|all] | today | all")
    {
        Description = "The specified problem(s) to run",
    };

    public static Option<string> Session => new(new[] { "--session" })
    {
        Description = "The session cookie to use",
        Arity = ArgumentArity.ExactlyOne,
        IsRequired = false
    };

    public static Option<int?> Step => new(new[] { "--step", "-s" })
    {
        Description = "The step to run",
        Arity = ArgumentArity.ExactlyOne,
        IsRequired = false,
    };
}
