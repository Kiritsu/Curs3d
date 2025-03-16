namespace Curs3d;

public class ModpackManifest
{
    public required string ManifestType { get; set; }
    public required string Version { get; set; }
    public required List<ModFile> Files { get; set; }
    public required int ManifestVersion { get; set; }
    public required string Name { get; set; }
    public required string Overrides { get; set; }
    public required string Author { get; set; }
    public required Minecraft Minecraft { get; set; }
}