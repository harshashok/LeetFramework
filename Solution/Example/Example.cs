using System;

namespace LeetFramework.Solution.Example;
/**
 * TESTING : Basic Execution Functionality
 */
[ProblemName("Example Problem : Testing basic execution functionality")]
public class Example : ISolver
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