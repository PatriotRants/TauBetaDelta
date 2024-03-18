using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Net.NetworkInformation;

using ForgeWorks.GlowFork;

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
        // //  will attempt to ping the server; TODO - max tries and then quit 
        // void wait_for_ping()
        // {
        //     while (!PingServer())
        //     {
        //         //  failure to connect after so many attempts will throw exception and exit
        //         NETWORK.WaitOne();
        //     }
        // }

        // var pingServer = new Thread(() => wait_for_ping());
        // pingServer.Start();
        // pingServer.Join();
    }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override void Stop()
    {
        Sender?.Shutdown(SocketShutdown.Both);
        Sender?.Close();

        IsRunning = false;
    }
    /// <summary>
    /// Send string data packet to server
    /// </summary>
    public void Send(string out_data)
    {
        if (!Sender.Connected)
        { Sender.Connect(Remote); }
        byte[] out_bytes = Encoding.ASCII.GetBytes(out_data);
        Sender.Send(out_bytes);
    }

    /* 
        /// <summary>
        /// Ping the server
        /// </summary>
        /// <returns></returns>
        public bool PingServer()
        {
            bool pingable = false;
            Ping pinger = new();

            try
            {
                PingReply reply = pinger.Send(Remote.Address);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                pinger?.Dispose();
                Log(pingable ? LoadStatus.Okay : LoadStatus.Error,
                    $"Ping [{Remote.Address.ToString()}] ${(pingable ? "SUCCESS" : "FAILURE")}");
            }

            return pingable;
        }
     */

}
