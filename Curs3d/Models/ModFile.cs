using System.Text.Json.Serialization;

namespace Curs3d.Models;

public class ModFile
{
    [JsonPropertyName("ProjectID")]
    public int ProjectId { get; set; }
    
    [JsonPropertyName("FileID")]
    public int FileId { get; set; }
    
    public required string DownloadUrl { get; set; }
    
    public bool Required { get; set; }
}