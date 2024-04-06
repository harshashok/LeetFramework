using System;
namespace RunnerFramework.Solution.Example
{
    [ProblemName("Example Problem")]
    public class Example : Solver
    {
        public object Solve(string input)
        {
            return input.Count();
        }

        public object SolveAsync(string input)
        {
            return input.Split('\n')
                .Count();
        }
    }
}

