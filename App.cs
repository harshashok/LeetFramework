using System.Reflection;
using CommandLine;
using LeetFramework;

Parser.Default.ParseArguments<AddOptions, SolveOptions>(args)
.WithParsed<AddOptions>(RunOptionsAdd)
.WithParsed<SolveOptions>(RunOptionsSolve)
.WithNotParsed(HandleParseError);

void RunOptionsAdd(AddOptions opts)
{
    Console.WriteLine($"Name : {opts.Name}");
    Console.WriteLine($"Title : {opts.Title}");
    Console.WriteLine("Verbose {0}", opts.Verbose.ToString());
    Console.WriteLine($"No-Readme : {opts.NoReadMe}");
    
    var workingDir = Environment.CurrentDirectory + "/Solution" + $"/{opts.Name}";
    var problem = new Problem(opts.Name, opts.Title, workingDir);
    var solution = new SolutionTemplateGenerator(problem);
    solution.Generate();
}

void RunOptionsSolve(SolveOptions opts)
{
    var allTSolvers = Assembly.GetEntryAssembly()!.GetTypes()
        .Where(t => t.GetTypeInfo().IsClass && typeof(ISolver).IsAssignableFrom(t));

    var selectedTsolvers = from first in allTSolvers
        join second in opts.InputFiles 
        on SolverExtensions.SolverName(first).ToLower() equals second.ToLower()
        select first;
    
    //Runner.RunSolver(GetSolvers(selectedTsolvers)[0]);
    Runner.RunAll(GetSolvers(selectedTsolvers));
}

// ISolver?[] GetSolvers(params Type[] tsolver)
// {
//     return tsolver.Select(t => Activator.CreateInstance(t) as ISolver).ToArray();
// }

ISolver?[] GetSolvers(IEnumerable<Type> tsolver) => tsolver.Select(t => Activator.CreateInstance(t) as ISolver).ToArray();

static void HandleParseError(IEnumerable<Error> errs)
{
    Console.WriteLine($"Errors : {errs.Any()}");
}