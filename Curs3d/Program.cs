// See https://aka.ms/new-console-template for more information

using System.IO.Compression;
using System.Text.Json;
using Curs3d;

if (args.Length != 1)
{
    args = [
        "https://mediafilez.forgecdn.net/files/6179/384/Enigmatica10-1.18.0.zip"
    ];
}

var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

var zipUrl = args[0];
const string zipFilePath = "downloaded.zip";
const string extractPath = "extracted";
const string minecraftDirectory = ".minecraft";
const string outputZipPath = ".minecraft.zip";

// Download the ZIP file
await DownloadFile(zipUrl, zipFilePath);

// Extract the ZIP file
ExtractZipFile(zipFilePath, extractPath);

// Read the manifest.json file
var manifestPath = Path.Combine(extractPath, "manifest.json");
if (!File.Exists(manifestPath))
{
    await Console.Out.WriteLineAsync("manifest.json not found in the extracted files.");
    return;
}

var jsonContent = await File.ReadAllTextAsync(manifestPath);
var manifest = JsonSerializer.Deserialize<ModpackManifest>(jsonContent, jsonOptions);

if (manifest == null || manifest.Files.Count == 0)
{
    await Console.Out.WriteLineAsync("No mod files found in the JSON file.");
    return;
}

Directory.CreateDirectory(minecraftDirectory);
Directory.CreateDirectory(Path.Combine(minecraftDirectory, "mods"));
CopyDirectory(Path.Combine(extractPath, "overrides"), minecraftDirectory);

await Parallel.ForEachAsync(manifest.Files, async (modFile, _) =>
{
    var modFileName = Path.GetFileName(new Uri(modFile.DownloadUrl).LocalPath);
    var modFilePath = Path.Combine(minecraftDirectory, "mods", modFileName);

    await DownloadFile(modFile.DownloadUrl, modFilePath);
});

CreateZipFile(minecraftDirectory, outputZipPath);

await Console.Out.WriteLineAsync($"All mods have been downloaded and compressed into {outputZipPath}");

Directory.Delete(extractPath, true);
Directory.Delete(zipFilePath, true);
Directory.Delete(minecraftDirectory, true);

return;

static void CopyDirectory(string sourceDir, string destinationDir)
{
    foreach (var dir in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
    {
        Directory.CreateDirectory(dir.Replace(sourceDir, destinationDir));
    }

    foreach (var file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
    {
        File.Copy(file, file.Replace(sourceDir, destinationDir), true);
    }
}

static async Task DownloadFile(string url, string outputPath)
{
    await Console.Out.WriteLineAsync($"Downloading {url} to {outputPath}");
    
    using var client = new HttpClient();
    using var response = await client.GetAsync(url);
    response.EnsureSuccessStatusCode();

    await using var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None);
    await response.Content.CopyToAsync(fs);
}

static void ExtractZipFile(string zipFilePath, string extractPath)
{
    Directory.CreateDirectory(extractPath);
    ZipFile.ExtractToDirectory(zipFilePath, extractPath, true);
}

static void CreateZipFile(string sourceDirectory, string zipFilePath)
{
    ZipFile.CreateFromDirectory(sourceDirectory, zipFilePath);
}
