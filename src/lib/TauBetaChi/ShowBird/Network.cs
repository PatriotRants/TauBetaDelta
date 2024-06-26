using System.Text;

using ForgeWorks.GlowFork;
using ForgeWorks.RailThorn;

using ForgeWorks.TauBetaDelta;
using ForgeWorks.TauBetaDelta.Logging;

using ForgeWorks.ShowBird.Messaging;
using ForgeWorks.ShowBird.Serialization;

namespace ForgeWorks.ShowBird;

public class Network : INetwork
{
    public const int MAX_PKT_SIZE = 1024;

    private static readonly LoggerManager LOGGER = LoggerManager.Instance;

    private static readonly Clock CLOCK = Clock.Instance;

    private DateTime Start { get; set; }
    private DateTime Tick { get; set; }
    private UpdateAgent Updater { get; set; }

    internal Server Server { get; private set; }
    internal Client Client { get; private set; }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    IClient INetwork.Client { get; }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public NetworkStatus Status { get; set; }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public bool IsPublic => Server?.IsPublic ?? true;
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string ServerEP => Server.Address.ToString();
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string ClientEP => Client.Address.ToString();
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public DateTime Time { get; private set; }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public bool IsAvailable => Client != null;

    /// <summary>
    /// Construct a network controller
    /// </summary>
    /// <param name="name"></param>
    internal Network(string name)
    {
        Name = name;
        Status = NetworkStatus.Idle;
        CLOCK.Tick += OnClockTick;
        Time = DateTime.UtcNow;
    }

    /// <summary>
    /// Starts the local network peers
    /// </summary>
    string INetwork.StartNetwork(UpdateAgent updateAgent, bool isLocal)
    {
        Updater = updateAgent;
        Status = NetworkStatus.Starting;

        if (!isLocal)
        {
            //  this will set up a server peer in its own app domain
            //Server = new Server(NetworkConfiguration);
        }
        else
        { Server = new(Name); }

        Client = new(Name);


        if (!IsPublic)
        {
            StartServer();
        }

        StartClient();

        Status = NetworkStatus.Running;
        Start = DateTime.Now;

        string status = $"[{Start}] ::: Network Status: {Status}";
        Log(LoadStatus.Ready, status);

        return status;
    }
    /// <summary>
    /// Requests local network peers shut down
    /// </summary>
    void INetwork.StopNetwork()
    {
        ShutDownNetwork();
        Log(LoadStatus.Ready, $"[{Start}] Network Status: {Status}");
    }

    private void StartServer()
    {
        Thread serverListen;

        try
        {
            //  calls server's Listener.Listen(...) method
            Server.Start();
            serverListen = new(() => ServerListen());
            serverListen.Start();

            Log(LoadStatus.Ready, $"Server {(Server.IsRunning ? "IS_RUNNING" : "IS_IDLE")}");
        }
        catch (Exception ex)
        {
            LOGGER.Post(ex, ex.Message);
        }
    }
    private void StartClient()
    {
        Thread clientListen;

        try
        {
            Client.Start();
            clientListen = new(() => ClientListen());
            clientListen.Start();

            Log(LoadStatus.Ready, $"Client {(Client.IsRunning ? "IS_RUNNING" : "IS_IDLE")}");

            //  test message
            Client.Send("Log in Client");
        }
        catch (Exception ex)
        {
            LOGGER.Post(ex, ex.Message);
        }
    }
    private void ServerListen()
    {
        while (Server.IsRunning)
        {
            //  wait for message from receiver
            Server.Handler = Server.Listener.Accept();

            //  set up receiving data packet
            StringBuilder received = new();
            int start = received.Length;
            byte[] rcvBuffer = new byte[MAX_PKT_SIZE];
            bool shutdown = false;

            while (!shutdown)
            {
                int bufferLen = Server.Handler.Receive(rcvBuffer);

                start += received.Length;
                received.Append(Encoding.ASCII.GetString(rcvBuffer, 0, bufferLen));


                if (bufferLen > 0)
                {
                    shutdown = received.IndexOf("<SHUTDOWN>", start) >= 0;

                    Log(LoadStatus.Okay, $"{nameof(Server)} <- {received}");
                    received.Clear();
                }
            }

            // one shot use
            Server.Send("<SHUTDOWN>");
            Server.RequestShutdown();
        }

        Server.Dispose();
    }
    private void ClientListen()
    {
        //  set up receiving data packet
        StringBuilder received = new();

        while (Client.IsRunning)
        {
            byte[] rcvBuffer = new byte[MAX_PKT_SIZE];
            int bufferLen = 0;

            //  random intermittent exception thrown - Sender will disconnect before Receive called
            // if (Client.Sender.Connected)
            // { bufferLen = Client.Sender.Receive(rcvBuffer); }

            try
            {
                bufferLen = Client.Sender.Connected ? Client.Sender.Receive(rcvBuffer) : 0;
                received.Append(Encoding.ASCII.GetString(rcvBuffer, 0, bufferLen));

                if (bufferLen > 0)
                {
                    Log(LoadStatus.Okay, $"{nameof(Client)} <- {received}");
                    received.Clear();
                }

            }
            catch (Exception ex)
            {
                LOGGER.Post(ex, ex.Message);
                break;
            }
        }

        Client.Dispose();
    }
    private void ShutDownNetwork()
    {
        if (!IsPublic)
        {
            /* **
                Sends command to server to shutdown.
                TODO: CommandPacket
            ** */
            Client.Send("<SHUTDOWN>");
            Status = NetworkStatus.ShutdownRequested;
            Client.RequestShutdown();
        }
        if (!Server.IsRunning && !Client.IsRunning)
        {
            Status = NetworkStatus.Idle;
        }
    }
    private void OnClockTick(TimeSpan span, DateTime time)
    {
        if (Status == NetworkStatus.Running)
        {
            /* **
                TODO: move time update to a threaded Sync method with the Clock
                Moving this to a threaded sync method ensures a more accurate time record as 
                more subscribers to the OnClockTick event can inject inaccuracies.
                update network time
            ** */
            var tick = DateTime.UtcNow;
            var period = tick - Tick;
            Time += period;
            Tick = tick;
        }
    }

    protected void Log(LoadStatus loadStatus, string logEntry)
    {
        LOGGER.Post(loadStatus, $"[{GetType().Name}.{Name}] {logEntry}");
        Updater($"[{loadStatus}] {logEntry}");
    }

    public void Unload(AutoResetEvent taskEvent)
    {
        //  if the network is not stopped, request shut down
        if (Status != NetworkStatus.ShutdownRequested || Status != NetworkStatus.Idle)
        {
            ShutDownNetwork();
        }

        taskEvent.Set();
    }
}
