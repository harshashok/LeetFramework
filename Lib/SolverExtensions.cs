using System.Reflection;

namespace LeetFramework;

static class SolverExtensions
{
    public static IEnumerable<object> SolveAll(this ISolver solver, string input)
    {
        yield return solver.Solve(input);
        var res = solver.SolveAsync(input);
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
    public static int Day(this ISolver solver)
    {
        return Day(solver.GetType());
    }

    public static int Day(Type t)
    {
        return int.Parse(t.FullName.Split('.')[2].Substring(3));
    }

    internal static string SolverName(this ISolver solver)
    {
        return SolverName(solver.GetType());
    }
    
    public static string SolverName(Type t)
    {
        return t.FullName!.Split('.')[^2];
    }

    public static string WorkingDirPath(this ISolver solver)
    {
        var fullName = solver.GetType()?.FullName?
            .Split('.').Skip(1).SkipLast(1);
        var outputStr = String.Join('/', fullName);
        return outputStr;
    }
}