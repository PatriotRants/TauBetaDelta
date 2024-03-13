using System.Reflection;

using ForgeWorks.GlowFork.Graphics;

using ForgeWorks.TauBetaDelta.Logging;
using ForgeWorks.TauBetaDelta.Extensibility;

namespace ForgeWorks.TauBetaDelta;

public class Resources : IRegistryItem
{
    public LoggerManager LoggerManager { get; }
    public AssetManager AssetManager { get; }
    public ShaderManager ShaderManager { get; }

    internal Resources()
    {
        LoggerManager = GetSingleton<LoggerManager>();
        AssetManager = GetSingleton<AssetManager>();
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
}
