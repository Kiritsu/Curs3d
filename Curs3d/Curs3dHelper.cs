using System.IO.Compression;
using System.Text.Json;
using Curs3d.Models;
using Curs3d.Serialization;

namespace Curs3d;

public class Curs3dHelper
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        TypeInfoResolver = ModpackManifestContext.Default
    };
    
    public static async Task RunAsync(CliOptions options)
    {
        var instanceId = Guid.NewGuid();
        var tempPath = Path.GetTempPath();
        
        var zipFileName = $"Modpack_{instanceId}.zip";
        var zipExtractFolderName = $"Modpack_{instanceId}_extracted";
        var minecraftFolderName = $"Modpack_{instanceId}_ready";
        
        var zipFilePath = Path.Combine(tempPath, zipFileName);
        var extractedFilePath = Path.Combine(tempPath, zipExtractFolderName);
        var modsDirectoryPath = Path.Combine(minecraftFolderName, "mods");

        // Download the ZIP file
        await DownloadFile(options.ModpackUrl, zipFilePath);
        await Console.Out.WriteLineAsync($"[Info]: Downloaded {new Uri(options.ModpackUrl).LocalPath} to {zipFilePath}");

        // Extract the ZIP file
        ExtractZipFile(zipFilePath, extractedFilePath);

        // Read the manifest.json file
        var manifestPath = Path.Combine(extractedFilePath, "manifest.json");
        if (!File.Exists(manifestPath))
        {
            await Console.Out.WriteLineAsync("[Error]: manifest.json not found in the extracted files.");
            return;
        }

        var manifestContent = await File.ReadAllTextAsync(manifestPath);
        var manifest = JsonSerializer.Deserialize<ModpackManifest>(manifestContent, JsonSerializerOptions);

        if (manifest == null || manifest.Files.Count == 0)
        {
            await Console.Out.WriteLineAsync("[Error]: No mod files found in the JSON file.");
            return;
        }

        // Create the final output directory
        Directory.CreateDirectory(minecraftFolderName);
        
        // Create the mods folder in the final output directory 
        Directory.CreateDirectory(modsDirectoryPath);
        
        CopyDirectory(Path.Combine(extractedFilePath, "overrides"), minecraftFolderName);

        await Parallel.ForEachAsync(manifest.Files, async (modFile, _) =>
        {
            var modFileName = Path.GetFileName(new Uri(modFile.DownloadUrl).LocalPath);
            var modFilePath = Path.Combine(modsDirectoryPath, modFileName);

            await DownloadFile(modFile.DownloadUrl, modFilePath);
            await Console.Out.WriteLineAsync($"[Info]: Downloaded mod {Path.GetFileNameWithoutExtension(modFileName)}");
        });

        if (string.IsNullOrWhiteSpace(options.OutputDirectory))
        {
            Console.WriteLine($"[Success]: Created the folder {minecraftFolderName} in {new DirectoryInfo(minecraftFolderName).FullName}. Please copy its content to your .minecraft folder.");
        }
        else
        {
            var outputDirectory = Path.Combine(options.OutputDirectory);

            CopyDirectory(minecraftFolderName, outputDirectory);
            Console.WriteLine($"[Success]: Copied the folder {minecraftFolderName} in {outputDirectory}");
            
            Directory.Delete(minecraftFolderName, true);
        }
        
        File.Delete(zipFilePath);
        Directory.Delete(extractedFilePath, true);
    }

    private static void CopyDirectory(string sourceDir, string destinationDir)
    {
        Parallel.ForEach(Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories), dir =>
        {
            var path = dir.Replace(sourceDir, destinationDir);
            Directory.CreateDirectory(path);
        });

        Parallel.ForEach(Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories), file =>
        {
            var path = file.Replace(sourceDir, destinationDir);
            File.Copy(file, path, true);
        });
    }

    private static async Task DownloadFile(string url, string outputPath)
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        await using var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await response.Content.CopyToAsync(fs);
    }

    private static void ExtractZipFile(string zipFilePath, string extractPath)
    {
        Directory.CreateDirectory(extractPath);
        ZipFile.ExtractToDirectory(zipFilePath, extractPath, true);
    }
}