using System.Reflection;

namespace LeetFramework;

static class SolverExtensions
{
    public static IEnumerable<object> SolveAll(this ISolver solver, string input)
    {
        yield return solver.Solve(input);
        var res = solver.SolveLinq(input);
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
    
    public static string DayName(this ISolver solver) => $"Day {solver.Day()}";
    public static int Day(this ISolver solver) => Day(solver.GetType());

    public static int Day(Type t) => int.Parse(t.FullName.Split('.')[2].Substring(3));

    public static string SolverName(this ISolver solver) => SolverName(solver.GetType());

    public static string SolverName(Type t) => t.FullName!.Split('.')[^2];

    public static string WorkingDirPath(this ISolver solver)
    {
        var fullName = solver.GetType()?.FullName?
            .Split('.').Skip(1).SkipLast(1);
        var outputStr = String.Join('/', fullName);
        return outputStr;
    }
}