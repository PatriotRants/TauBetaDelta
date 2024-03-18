using System.Net;
using System.Text;
using System.Net.Sockets;

using ForgeWorks.GlowFork;

namespace ForgeWorks.ShowBird;

public class Server : NetPeer
{

    internal Socket Listener { get; }
    internal Socket Handler { get; set; }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override IPEndPoint Address => (IPEndPoint)Listener?.LocalEndPoint;
    //  because eventually, a public server will be initiated from this class
    /// <summary>
    /// Determine if the Server is public (TRUE); otherwise FALSE
    /// </summary>
    public bool IsPublic { get; } = false;

    //  TODO: NetworkConfiguration ctor parameter
    internal Server(string name) : base(name)
    {
        try
        {
            //  IP_ADDRESS needs to be set for re-use (just in case)

            //  our listening socket
            Listener = new(IP_ADDRESS.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //  bind listener to the local IP end point
            Listener.Bind(LOCAL_EP);
        }
        catch (Exception ex)
        {
            Log(LoadStatus.Error, ex.Message);
        }
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override void Start()
    {
        //  tell the server to listen to its IP:PORT
        Listener.Listen();
        //  set IsRunning TRUE
        IsRunning = true;
    }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override void Stop()
    {
        //  if a Handler was never assigned then we don't want to throw an exception here
        Handler?.Shutdown(SocketShutdown.Both);
        Handler?.Close();
        Listener?.Shutdown(SocketShutdown.Both);
        Listener?.Close();
        //  set IsRunning FALSE
        IsRunning = false;
    }

    internal void Send(string out_data)
    {
        byte[] out_bytes = Encoding.ASCII.GetBytes(out_data);
        Handler.Send(out_bytes);
    }
}
