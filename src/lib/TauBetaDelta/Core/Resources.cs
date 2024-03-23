using System.Reflection;

using ForgeWorks.RailThorn.Fonts;
using ForgeWorks.RailThorn.Graphics;

using ForgeWorks.TauBetaDelta.Logging;
using ForgeWorks.TauBetaDelta.Extensibility;

namespace ForgeWorks.TauBetaDelta;

public class Resources : IRegistryItem, IDisposable
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Get the current <see cref="AssetManager.Instance"/>
    /// </summary>
    public AssetManager AssetManager { get; }
    /// <summary>
    /// Get the current <see cref="FontManager.Instance"/>
    /// </summary>
    public FontManager FontManager { get; }
    /// <summary>
    /// Get the current <see cref="LoggerManager.Instance"/>
    /// </summary>
    public LoggerManager LoggerManager { get; }
    /// <summary>
    /// Get the current <see cref="ShaderManager.Instance"/>
    /// </summary>
    public ShaderManager ShaderManager { get; }

    internal Resources()
    {
        Name = "Resources";

        AssetManager = GetSingleton<AssetManager>();
        FontManager = GetSingleton<FontManager>();
        LoggerManager = GetSingleton<LoggerManager>();
        ShaderManager = GetSingleton<ShaderManager>();
    }

    private static TSingleton GetSingleton<TSingleton>()
    {
        var type = typeof(TSingleton);
        var propName = "Instance";

        //  find static property "Instance"
        var instance = type.GetProperty(propName, BindingFlags.NonPublic | BindingFlags.Static);
        if (instance == null)
        {
            throw new InvalidOperationException($"Cannot find static '{propName}' property on {type.Name}");
        }

        return (TSingleton)instance.GetValue(null);
    }

    public void Dispose()
    {
        //  TODO: implement dispose on resource items
        AssetManager.Dispose();
        FontManager.Dispose();
        LoggerManager.Dispose();
        ShaderManager.Dispose();
    }
}
