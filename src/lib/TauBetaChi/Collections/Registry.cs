using ForgeWorks.TauBetaDelta.Extensibility;

namespace ForgeWorks.TauBetaDelta.Collections;

public class Registry
{
    private static readonly Lazy<Registry> INSTANCE = new(() => new());

    private readonly List<IRegistryItem> registry = new();

    private static Registry Instance => INSTANCE.Value;


    private Registry() { }

    public static void Add<TRegistryItem>(TRegistryItem registryItem) where TRegistryItem : IRegistryItem
    {
        Instance.registry.Add(registryItem);
    }
    public static TRegistryItem Get<TRegistryItem>() where TRegistryItem : IRegistryItem
    {
        return Instance.registry.Where(r => r is TRegistryItem)
            .Select(ri => (TRegistryItem)ri)
            .FirstOrDefault();
    }
}
