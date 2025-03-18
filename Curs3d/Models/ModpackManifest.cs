namespace Curs3d.Models;

public class ModpackManifest
{
    public string? ManifestType { get; set; }
    
    public string? Version { get; set; }
    
    public required List<ModFile> Files { get; set; }
    
    public int ManifestVersion { get; set; }
    
    public string? Name { get; set; }
    
    public string? Overrides { get; set; }
    
    public string? Author { get; set; }
    
    public Minecraft? Minecraft { get; set; }
}