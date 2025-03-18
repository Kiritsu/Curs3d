using CommandLine;
using Curs3d;
using Curs3d.Models;

await Parser.Default
    .ParseArguments<CliOptions>(args)
    .WithParsedAsync(Curs3dHelper.RunAsync);