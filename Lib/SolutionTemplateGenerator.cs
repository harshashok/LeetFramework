namespace LeetFramework;

public class SolutionTemplateGenerator
{
    public string Generate(Problem problem) {
        return $@"using System;
             |using System.Collections.Generic;
             |using System.Collections.Immutable;
             |using System.Linq;
             |using System.Text;
             |
             |namespace AdventOfCode.Y{problem.Dir}.Day{problem.Name};
             |
             |[ProblemName(""{problem.Title}"")]      
             |class Solution : ISolver {{
             |
             |    public object Solve(string input) {{
             |        return 0;
             |    }}
             |
             |    public object SolveLinq(string input) {{
             |        return 0;
             |    }}
             |}}
             |".StripMargin();
    }
}