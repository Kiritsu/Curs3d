namespace Curs3d;

public class ModFile
{
    public required int ProjectID { get; set; }
    public required int FileID { get; set; }
    public required string DownloadUrl { get; set; }
    public required bool Required { get; set; }
}