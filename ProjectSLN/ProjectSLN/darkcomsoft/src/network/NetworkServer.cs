using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lidgren.Network;
using Projectsln.darkcomsoft.src.engine;

namespace Projectsln.darkcomsoft.src.network
{
    public class NetworkServer : NetworkBase
    {
        private NetworkCallBacks networkCallBacks;

        public NetworkServer(long ip, int port, int maxplayers)
        {
            networkCallBacks = new NetworkCallBacks();

            NetPeerConfiguration config = new NetPeerConfiguration(NetConfig.AppIdentifier);
            config.MaximumConnections = maxplayers;

            config.EnableUPnP = !NetConfig.DedicatedServer;
            config.AutoFlushSendQueue = true;
            config.DefaultOutgoingMessageCapacity = NetConfig.DefaultOutgoingMessageCapacity;
            config.UseMessageRecycling = NetConfig.UseMessageRecycling;
            config.SendBufferSize = NetConfig.SendBufferSize;
            config.AcceptIncomingConnections = NetConfig.AcceptConnection;
            config.NetworkThreadName = "DarckNet - Server";

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            config.Port = port;
            config.BroadcastAddress = new IPAddress(ip);

            NetServer peer = new NetServer(config);
            peer.Start(); // needed for initialization

            if (config.EnableUPnP)
            {
                peer.UPnP.ForwardPort(port, "Port for the server of " + Application.AppName);
            }

            m_peer = peer;
            _peerStatistics = peer.Statistics;

            //OUTRO CODIGO QUE TALVEZ NAO SEJA NESSESARIO, SE NAO FOR REMOVER DEPOIS
            /*if (peer.Status == NetPeerStatus.Running)
            {
                Started = true;
            }*/

            Debug.Log("Unique identifier is " + peer.UniqueIdentifier);

            networkCallBacks.OnServerStart?.Invoke();
        }

        public override void Tick()
        {
            base.Tick();
        }

        protected override void OnDispose()
        {
            networkCallBacks?.Dispose();
            networkCallBacks = null;

            base.OnDispose();
        }
    }
}
