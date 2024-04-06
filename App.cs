using System.Reflection;
using CommandLine;
using RunnerFramework;

Console.WriteLine("Leet Framework");

Parser.Default.ParseArguments<AddOptions, SolveOptions>(args)
.WithParsed<AddOptions>(RunOptions)
.WithParsed<SolveOptions>(RunOptionsSolve)
.WithNotParsed(HandleParseError);

static void RunOptions(AddOptions opts)
{
    opts.InputFiles.ToList().ForEach(x => Console.WriteLine(x));
    Console.WriteLine("Verbose {0}", opts.Verbose.ToString());
}

void RunOptionsSolve(SolveOptions opts)
{
    var allTSolvers = Assembly.GetEntryAssembly()!.GetTypes()
    .Where(t => t.GetTypeInfo().IsClass && typeof(Solver).IsAssignableFrom(t))
    .OrderBy(t => t.FullName)
    .ToArray();

    // single instance. -- for POC purposes only.
    var selectedTsolvers = allTSolvers.FirstOrDefault(tsolver => tsolver.Name.ToLower() == opts.InputFiles.First().ToLower());

    //Runner.RunSolver(GetSolvers(selectedTsolvers)[0]);
    Runner.RunAll(GetSolvers(selectedTsolvers));
}

Solver?[] GetSolvers(params Type[] tsolver)
{
    return tsolver.Select(t => Activator.CreateInstance(t) as Solver).ToArray();
}

static void HandleParseError(IEnumerable<Error> errs)
{
    //handle errors.
}