using LeetFramework;

namespace LeetFramework.Solution.TopLevel.MidLevel.BaseLevel;

[ProblemName("Base Level Program : TopLevel/MidLevel/BaseLevel")]
public class Solution : ISolver
{
    public object Solve(string input)
    {
        return input.Split('\n').Length;
    }
}