using System.Reflection;
using CommandLine;
using LeetFramework;

Console.WriteLine("Leet Framework");

Parser.Default.ParseArguments<AddOptions, SolveOptions>(args)
.WithParsed<AddOptions>(RunOptionsAdd)
.WithParsed<SolveOptions>(RunOptionsSolve)
.WithNotParsed(HandleParseError);

static void RunOptionsAdd(AddOptions opts)
{
    opts.InputFiles.ToList().ForEach(Console.WriteLine);
    opts.InputTitles.ToList().ForEach(Console.WriteLine);
    Console.WriteLine("Verbose {0}", opts.Verbose.ToString());
}

void RunOptionsSolve(SolveOptions opts)
{
    var allTSolvers = Assembly.GetEntryAssembly()!.GetTypes()
        .Where(t => t.GetTypeInfo().IsClass && typeof(ISolver).IsAssignableFrom(t));

    // single instance. -- for POC purposes only.
    //var selectedTsolvers = allTSolvers.FirstOrDefault(tsolver => tsolver.Name.ToLower() == opts.InputFiles.First().ToLower());
    var selectedTsolvers = allTSolvers.First(solver => SolverExtensions.SolverName(solver).ToLower() == opts.InputFiles.First().ToLower());
    
    //Runner.RunSolver(GetSolvers(selectedTsolvers)[0]);
    Runner.RunAll(GetSolvers(selectedTsolvers));
}

ISolver?[] GetSolvers(params Type[] tsolver)
{
    return tsolver.Select(t => Activator.CreateInstance(t) as ISolver).ToArray();
}

static void HandleParseError(IEnumerable<Error> errs)
{
    //handle errors.
}