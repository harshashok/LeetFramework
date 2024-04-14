using System;
using CommandLine;

namespace LeetFramework;


[Verb("add", HelpText = "Add file contents to the index.")]
class AddOptions
{
    [Option('f', "files", Separator = ',', Required = true, HelpText = "Input files to be processed.")]
    public IEnumerable<string> InputFiles { get; set; }

    // Omitting long name, defaults to name of property, ie "--verbose"
    [Option(Default = false, HelpText = "Prints all messages to standard output.")]
    public bool Verbose { get; set; }

    [Option("no-readme", Default = false, HelpText = "Dont create readme files.")]
    public bool NoReadMe { get; set; }
}

[Verb("solve", true, HelpText = "Run one or more solutions specified.")]
class SolveOptions
{
    [Option('f', "files", Separator = ',', Default = true, Required = true, HelpText = "Input files to be processed.")]
    public IEnumerable<string> InputFiles { get; set; }

    // Omitting long name, defaults to name of property, ie "--verbose"
    [Option(Default = false, HelpText = "Prints all messages to standard output.")]
    public bool Verbose { get; set; }

    [Option("async", Default = false, HelpText = "Solve asynchronously.")]
    public bool Async { get; set; }
}
