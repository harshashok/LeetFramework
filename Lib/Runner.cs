using System;
using System.Diagnostics;
using System.Reflection;

namespace RunnerFramework;

class ProblemName : Attribute
{
    public readonly string Name;
    public ProblemName(string name)
    {
        this.Name = name;
    }
}

interface Solver
{
    object Solve(string input);
    object SolveAsync(string input) => null;
}

static class SolverExtensions
{
    public static IEnumerable<object> SolveAll(this Solver solver, string input)
    {
        yield return solver.Solve(input);
        var res = solver.SolveAsync(input);
        if (res != null)
        {
            yield return res;
        }
    }

    public static string GetName(this Solver solver)
    {
        return (
            solver
                .GetType()
                .GetCustomAttribute(typeof(ProblemName)) as ProblemName
        ).Name;
    }

    public static string DayName(this Solver solver)
    {
        return $"Day {solver.Day()}";
    }

    public static int Year(this Solver solver)
    {
        return Year(solver.GetType());
    }

    public static int Year(Type t)
    {
        return int.Parse(t.FullName.Split('.')[1].Substring(1));
    }
    public static int Day(this Solver solver)
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

    public static string WorkingDir(this Solver solver)
    {
        return WorkingDir(solver.Year(), solver.Day());
    }

    public static string WorkingDirPath(this Solver solver)
    {
        var fullName = solver.GetType()?.FullName?
            .Split('.').Skip(1).SkipLast(1);
        var outputStr = String.Join('/', fullName);
        return outputStr;

    }
}


record SolverResult(string[] answers, string[] errors);

class Runner
{

    private static string GetNormalizedInput(string file)
    {
        var input = File.ReadAllText(file);
        if (input.EndsWith("\n"))
        {
            input = input.Substring(0, input.Length - 1);
        }
        return input;
    }

    public static SolverResult RunSolver(Solver solver, bool sample = false)
    {

        var workingDir = solver.WorkingDirPath();
        var indent = "    ";
        var isSample = sample ? "[SAMPLE]" : "";
        Write(ConsoleColor.White, $"{indent} {solver.GetName()} {isSample}");
        WriteLine();
        var dir = workingDir;
        var file = Path.Combine(workingDir, sample ? "sample.in" : "input.in");
        var refoutFile = file.Replace(".in", ".refout");
        var refout = File.Exists(refoutFile) ? File.ReadAllLines(refoutFile) : null;
        var input = GetNormalizedInput(file);
        var iline = 0;
        var answers = new List<string>();
        var errors = new List<string>();
        var stopwatch = Stopwatch.StartNew();

        foreach (var line in solver.SolveAll(input))
        {
            var ticks = stopwatch.ElapsedTicks;
            //if (line is OcrString)
            //{
            //    Console.WriteLine("\n" + (line as OcrString).st.Indent(10, firstLine: true));
            //}
            answers.Add(line.ToString());
            var (statusColor, status, err) =
                refout == null || refout.Length <= iline ? (ConsoleColor.Cyan, "?", null) :
                refout[iline] == line.ToString() ? (ConsoleColor.DarkGreen, "✓", null) :
                (ConsoleColor.Red, "X", $"{solver.DayName()}: In line {iline + 1} expected '{refout[iline]}' but found '{line}'");

            if (err != null)
            {
                errors.Add(err);
            }

            Write(statusColor, $"{indent}  {status}");
            Console.Write($" {line} ");
            var diff = ticks * 1000.0 / Stopwatch.Frequency;

            WriteLine(
                diff > 1000 ? ConsoleColor.Red :
                diff > 500 ? ConsoleColor.Yellow :
                ConsoleColor.DarkGreen,
                $"({diff.ToString("F3")} ms)"
            );
            iline++;
            stopwatch.Restart();
        }

        return new SolverResult(answers.ToArray(), errors.ToArray());
    }

    public static void RunAll(params Solver[] solvers)
    {
        var errors = new List<string>();

        foreach (var solver in solvers)
        {
            var result = RunSolver(solver);
            WriteLine();
            errors.AddRange(result.errors);
        }

        WriteLine();

        if (errors.Any())
        {
            WriteLine(ConsoleColor.Red, "Errors:\n" + string.Join("\n", errors));
        }
    }

    private static void WriteLine(ConsoleColor color = ConsoleColor.Gray, string text = "")
    {
        Write(color, text + "\n");
    }
    private static void Write(ConsoleColor color = ConsoleColor.Gray, string text = "")
    {
        var c = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = c;
    }
}

