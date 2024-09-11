namespace LeetFramework;

public class SolutionTemplateGenerator
{
    private readonly Problem _problem;

    private static readonly string DEFAULT_DIR_SOLUTION = "Solution";
    private static readonly string DEFAULT_CLS_SOLUTION = "Solution.cs";
    private static readonly string DEFAULT_INP_SOLUTION = "input.in";
    public SolutionTemplateGenerator(Problem problem) => _problem = problem;

    public void Generate()
    {
        CreateSolutionCsFile();
        CreateSolutionReadmeFile();
        CreateInputFiles();
    }

    private void CreateSolutionCsFile()
    {
        var file = Path.Combine(Environment.CurrentDirectory, DEFAULT_DIR_SOLUTION, $"{_problem.Name}", DEFAULT_CLS_SOLUTION);
        string? dirName = Path.GetDirectoryName(file);
        ArgumentException.ThrowIfNullOrEmpty(dirName);
        
        if (File.Exists(file))
        {
            throw new ArgumentException($"Solution template {_problem.Name} already exists in {dirName}");
        }

        Console.WriteLine($"Creating directory {dirName}");
        Directory.CreateDirectory(dirName);
        Console.WriteLine($"Writing file : {file}");
        File.WriteAllText(file, GenerateSolutionTemplate());
    }

    private void CreateSolutionReadmeFile()
    {
        var mdFileName = _problem.Name + ".md";
        var mdFile = Path.Combine(Environment.CurrentDirectory, DEFAULT_DIR_SOLUTION, $"{_problem.Name}", mdFileName);
        
        if (!File.Exists(mdFile))
        {
            Console.WriteLine($"Writing Readme : {mdFile}");
            File.WriteAllText(mdFile, GenerateReadmeTemplate());
        }
    }

    private void CreateInputFiles()
    {
        var inputFile = Path.Combine(Environment.CurrentDirectory, DEFAULT_DIR_SOLUTION, $"{_problem.Name}", DEFAULT_INP_SOLUTION);
        
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Writing Input : {inputFile}");
            File.Create(inputFile);                            // Change this when there's a nice way to input data.
        }
    }

    private void WriteFile(string file, string content)
    {
        File.WriteAllText(file, content);
    }
    
    private string GenerateSolutionTemplate() {
        return $@"using System;
             |using System.Collections.Generic;
             |using System.Collections.Immutable;
             |using System.Linq;
             |using System.Text;
             |
             |using LeetFramework;
             |namespace LeetFramework.Solution.{_problem.Name};
             |
             |[ProblemName(""{_problem.Title}"")]      
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
    
    private string GenerateReadmeTemplate() {
        return $@"
           > # {_problem.Name} : {_problem.Title}
           > 
           > 
           > ".StripMargin("> ");
    }
}