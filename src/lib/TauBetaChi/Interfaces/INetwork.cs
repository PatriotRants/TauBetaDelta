using ForgeWorks.TauBetaDelta;
using ForgeWorks.TauBetaDelta.Extensibility;

namespace ForgeWorks.ShowBird;

public interface INetwork : IRegistryItem, IDisposable
{
    /// <summary>
    /// Get the Client interface
    /// </summary>
    IClient Client { get; }
    /// <summary>
    /// Get the <see cref="Network.Status"/>
    /// </summary>
    NetworkStatus Status { get; }
    /// <summary>
    /// Determine whether the <see cref="Network.Server"/> component is publically accessible
    /// </summary>
    bool IsPublic { get; }
    /// <summary>
    /// Get the <see cref="Network.Server"/> <see cref="System.Net.IPEndPoint"/>
    /// </summary>
    string ServerEP { get; }
    /// <summary>
    /// Get the <see cref="Network.Client"/> <see cref="System.Net.IPEndPoint"/>
    /// </summary>
    string ClientEP { get; }
    /// <summary>
    /// Get the <see cref="Network.Time"/> updated to sync with <see cref="GlowFork.Clock"/>
    /// </summary>
    DateTime Time { get; }

    /// <summary>
    /// Start the network
    /// </summary>
    internal void StartNetwork(bool isLocal = true);
    /// <summary>
    /// Stop the network
    /// </summary>
    internal void StopNetwork();
}

public interface IClient
{

}
