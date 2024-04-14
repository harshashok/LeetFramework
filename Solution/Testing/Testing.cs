using System;
using LeetFramework;

// Testing Lack of ProblemName attribute.
namespace LeetFramework.Solution.Testing
{
    public class Testing : ISolver
    {
        public object Solve(string input)
        {
            return input.Count();
        }
    }
}

