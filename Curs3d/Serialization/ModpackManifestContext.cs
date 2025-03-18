using System.Text.Json;
using System.Text.Json.Serialization;
using Curs3d.Models;

namespace Curs3d.Serialization;

[JsonSerializable(typeof(ModpackManifest))]
public partial class ModpackManifestContext : JsonSerializerContext;