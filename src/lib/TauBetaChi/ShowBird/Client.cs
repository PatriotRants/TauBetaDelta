using System.Net;
using System.Text;
using System.Net.Sockets;

namespace ForgeWorks.ShowBird;

public class Client : NetPeer, IClient
{
    private IPEndPoint Remote { get; }

    internal Socket Sender { get; private set; }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override IPEndPoint Address => (IPEndPoint)Sender?.LocalEndPoint;

    //  TODO: NetworkConfiguration ctor parameter
    internal Client(string name) : base(name)
    {
        Remote = new IPEndPoint(IP_ADDRESS, PORT);
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override void Start()
    {
        Sender = new(IP_ADDRESS.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        IsRunning = true;
    }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override void RequestShutdown()
    {
        try
        {
            Sender?.Shutdown(SocketShutdown.Both);
        }
        catch (ObjectDisposedException disposedEx)
        {
            LOGGER.Post(disposedEx, disposedEx.Message);
        }

        IsRunning = false;
    }
    /// <summary>
    /// Send string data packet to server
    /// </summary>
    public void Send(string out_data)
    {
        try
        {
            if (!Sender.Connected)
            { Sender.Connect(Remote); }
        }
        catch (ObjectDisposedException disposedEx)
        {
            LOGGER.Post(disposedEx, disposedEx.Message);

            return;
        }

        byte[] out_bytes = Encoding.ASCII.GetBytes(out_data);
        Sender.Send(out_bytes);
    }
    public override void Dispose()
    {
        Sender?.Close();
    }
}
