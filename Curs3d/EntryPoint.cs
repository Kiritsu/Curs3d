using System.Diagnostics.CodeAnalysis;
using CommandLine;
using Curs3d.Models;

namespace Curs3d;

internal class Program
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CliOptions))]
    public static async Task Main(string[] args)
    {
        await Parser.Default
            .ParseArguments<CliOptions>(args)
            .WithParsedAsync(o => Curs3dHelper.RunAsync((CliOptions)o));
    }
}