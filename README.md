<p align="center">
  <img width="460" height="300" src="/content/splash.png">
</p>

<p style="text-align: center;">A rewrite to a game which I am legally disallowed to reference. There can be no association between <b>TauBetaDelta</b> and its progenitor.</p>
<p style="text-align: center;"><i>This is a <b>Work In Progress</b>.</i></p>   
  
I welcome anyone who wants to join and work on this. *If you do*, I have to let you know that there will be an agreement to not mention or reference the original from which this work is derived. And I think it's terrible we can't openly and publically pay homage to the original work. ***TBD*** will remain an open-source work. This is (intended to be) a mod-capable game.

So ... there is a lot to be done. I'm working on issues and the project roadmap. That is a lot of work in and of itself.

I've streamlined the Game initialization and window creation. Also, we are displaying the temporary splash screen.

**Mar 17, 2024**  
Finished the basic implementation of the `Network` components: `NetPeer`, `Client`, `Server`. A couple of interfaces provided: `INetwork` and `IClient`.

When `LoadingState` begins initialization, `INetwork.StartNetwork(bool isLocal)` initiates network start up. If `isLocal = false`, then we are expecting a public server IP Address (this will be addressed in a later iteration). Once the `Client` and `Server` components' `IsRunning = true`, the `INetwork.Status = NetworkStatus.Running`. When the *game* is shutting down (`Window.OnClose(...)` event raised), the `INetwork.StopNetwork()` method initiates shutdown:
1. Client sends the Server a ShutDownRquestCommand (`"<SHUTDOWN>"`)
2. Server initiates shut down
3. Server responds to Client with `"<SHUTDOWN>"`
4. Client receives response and initiates shut down

When bot components are shut down, `INetwork.Status = NetworkStatus.Idle`.

There is a lot more to do with networking. Not knowing what I don't know (yet), I have not implemented any of the serialization models for messaging and commands.
