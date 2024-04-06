using System;
namespace RunnerFramework.Solution.Testing
{
    public class Testing : Solver
    {
        public object Solve(string input)
        {
            return input.Count();
        }
    }
}

