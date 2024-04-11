namespace ForgeWorks.TauBetaDelta.Extensibility;

public interface IRegistryItem : IUnloadable
{
    /// <summary>
    /// Get the current object's name
    /// </summary>
    string Name { get; }
}
