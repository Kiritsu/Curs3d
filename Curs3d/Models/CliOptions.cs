using CommandLine;

namespace Curs3d.Models;

public class CliOptions
{
    [Option('s', "modpackUrl", 
        Required = true, 
        HelpText = "The URL to the CurseForge CDN Modpack URL")]
    public required string ModpackUrl { get; set; }
    
    [Option('o', "output", 
        Required = false, 
        HelpText = "The output directory. Defaults to current working directory")]
    public string? OutputDirectory { get; set; }
}