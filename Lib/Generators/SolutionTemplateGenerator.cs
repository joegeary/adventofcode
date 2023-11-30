using AdventOfCode.Models;

namespace AdventOfCode.Generators;

internal class SolutionTemplateGenerator
{
    public string Generate(Problem problem)
    {
        return $$"""
                 namespace AdventOfCode.Y{{problem.Year}}.Day{{problem.Day:00}};

                 [ProblemName("{{problem.Title}}")]
                 internal class Solution : ISolver {

                 public object PartOne(string input) {
                      return 0;
                 }

                 public object PartTwo(string input) {
                     return 0;
                 }
             }
             """;
    }
}
