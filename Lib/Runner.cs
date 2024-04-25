using System;
using System.Diagnostics;

namespace LeetFramework;

class ProblemName : Attribute
{
    public readonly string Name;
    public ProblemName(string name)
    {
        this.Name = name;
    }
}

interface ISolver
{
    object Solve(string input);
    object SolveLinq(string input) => null;
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

    public static SolverResult RunSolver(ISolver solver, bool sample = false)
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

    public static void RunAll(params ISolver[] solvers)
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

