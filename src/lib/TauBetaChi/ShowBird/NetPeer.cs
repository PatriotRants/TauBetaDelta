using System.Net;

using ForgeWorks.RailThorn;

using ForgeWorks.TauBetaDelta.Logging;
using ForgeWorks.TauBetaDelta.Collections;

namespace ForgeWorks.ShowBird;

/// <summary>
/// Base EndPoint class from which the Client/Server objects are built
/// </summary>
public abstract class NetPeer : IDisposable
{
    protected static readonly LoggerManager LOGGER = LoggerManager.Instance;
    protected static readonly INetwork NETWORK = Registry.Get<Network>();

    protected const int PORT = 8000;
    protected const string LOCALHOST = "localhost";
    protected static readonly IPHostEntry HOST = Dns.GetHostEntry(LOCALHOST);
    protected static readonly IPAddress IP_ADDRESS = HOST.AddressList[0];
    protected static readonly IPEndPoint LOCAL_EP = new IPEndPoint(IP_ADDRESS, PORT);

    protected const int MAX_PKT_SIZE = Network.MAX_PKT_SIZE;

    /// <summary>
    /// Get the current <see cref="Network.Name"/>
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Get the current <see cref="NetPeer"/> IP Address
    /// </summary>
    public abstract IPEndPoint Address { get; }
    /// <summary>
    /// Get the current <see cref="NetPeer"/> run status
    /// </summary>
    public bool IsRunning { get; protected set; }

    protected NetPeer(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Start the current <see cref="ShowBird.NetPeer"/> component
    /// </summary>
    public abstract void Start();
    /// <summary>
    /// Stop the current <see cref="ShowBird.NetPeer"/> component
    /// </summary>
    public abstract void RequestShutdown();

    protected void Log(LoadStatus loadStatus, string logEntry)
    {
        LOGGER.Post(loadStatus, $"[{GetType().Name}.{Name}] {logEntry}");
    }

    public abstract void Dispose();
}
