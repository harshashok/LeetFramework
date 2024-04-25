using LeetFramework;

namespace LeetFramework.Solution.TestFolder;

[ProblemName("TestFolder : Testing Solution named class - TOP LEVEL")]
public class Solution : ISolver
{
    public object Solve(string input)
    {
        return input.Count();
    }

    public object SolveLinq(string input)
    {
        return input.Split('\n')
            .Count();
    }
}