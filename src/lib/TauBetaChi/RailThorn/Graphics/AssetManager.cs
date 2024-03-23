using ForgeWorks.GlowFork;
using ForgeWorks.ShowBird.Messaging;
using ForgeWorks.TauBetaDelta.Logging;

namespace ForgeWorks.RailThorn.Graphics;

public class AssetManager : IDisposable
{
    private static readonly Lazy<AssetManager> INSTANCE = new(() => new());

    private readonly Dictionary<string, Texture> _textures = new();

    private ResourceLogger Log { get; } = LoggerManager.Instance.Post;
    private (LoadStatus condition, ErrorCode errCode, string message) Status { get; set; }

    internal static AssetManager Instance => INSTANCE.Value;

    public string ContentDirectory { get; }
    public string Info => $"[{Status.condition}.{Status.errCode}] {Status.message}";

    private AssetManager()
    {
        //  TODO: replace hardcode directory with settings
        ContentDirectory = "./content";

        if (!Directory.Exists(ContentDirectory))
        {
            Status = (LoadStatus.Error, ErrorCode.DirNotFound, $"[{nameof(AssetManager)}] Content directory '{ContentDirectory}' does not exist");
        }
        else
        {
            Status = (LoadStatus.Okay, ErrorCode.NoErr, $"[{nameof(AssetManager)}] Loading Content directory '{ContentDirectory}'");
        }
    }

    public bool Load(UpdateAgent updateAgent)
    {
        //  load assets
        if (Status.condition == LoadStatus.Error)
        {
            Log(Status.condition, Status.message);
            updateAgent($"[{Status.condition}] {Status.message}");
        }
        else
        {
            //  select files considereing some may have been loaded already
            var files = new DirectoryInfo(ContentDirectory)
                .GetFiles()
                .Where(f => !_textures.ContainsKey(Path.GetFileNameWithoutExtension(f.Name)))
                .Select(f => (name: Path.GetFileNameWithoutExtension(f.Name), path: f.FullName));

            foreach (var file in files)
            {
                updateAgent($"Loading [{file.name}]");
                Texture texture = Texture.LoadFromFile(file.path);

                _textures.TryAdd(file.name, texture);
            }
        }

        //  expected status condition is Ready
        return Status.condition == LoadStatus.Ready;
    }
    public bool GetTexture(string texName, out Texture texture)
    {
        return _textures.TryGetValue(texName, out texture);
    }
    public bool LoadTexture(string texName, out Texture texture)
    {
        var file = Path.Combine(ContentDirectory, $"{texName}.png");
        texture = Texture.LoadFromFile(file);

        _textures.Add(texName, texture);

        return texture != null;
    }
    public bool GetError(out ErrorCode errCode)
    {
        errCode = Status.errCode;

        return errCode != ErrorCode.NoErr;
    }

    public void Dispose()
    {
        foreach (Texture texture in _textures.Values)
        {
            texture.Dispose();
        }
    }
}
