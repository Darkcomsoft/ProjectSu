using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lidgren.Network;
using Projectsln.darkcomsoft.src.engine;

namespace Projectsln.darkcomsoft.src.network
{
    public class NetworkClient : NetworkBase
    {
        private NetworkCallBacks networkCallBacks;

        public NetworkClient(string ip, int port)
        {
            networkCallBacks = new NetworkCallBacks();

            NetPeerConfiguration config = new NetPeerConfiguration(NetConfig.AppIdentifier);

            config.AutoFlushSendQueue = true;
            config.DefaultOutgoingMessageCapacity = NetConfig.DefaultOutgoingMessageCapacity;
            config.UseMessageRecycling = true;
            config.SendBufferSize = NetConfig.SendBufferSize;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.ConnectionTimeout = NetConfig.ConnectionTimeout;
            config.NetworkThreadName = "DarckNet - Client";

            NetClient peer = new NetClient(config);
            peer.Start(); // needed for initialization

            m_peer = peer;

            NetOutgoingMessage approval = peer.CreateMessage();
            approval.Write(NetConfig.SecretKey);

            peer.Connect(ip, port, approval);

            _peerStatistics = peer.Statistics;//used for get peer statistics

            Debug.Log("Unique identifier is " + peer.UniqueIdentifier, "NETWORK");

            //ESSE CODIGO AQUI EM BAIXO E NESSESARIO? NAO SEI SE PA NAO, REMOVER DEPOIS SE NAO FOR.
            /*var om = peer.CreateMessage();
            peer.SendUnconnectedMessage(om, new IPEndPoint(IPAddress.Loopback, Port));
            try
            {
                peer.SendUnconnectedMessage(om, new IPEndPoint(IPAddress.Loopback, Port));
            }
            catch (NetException nex)
            {
                if (nex.Message != "This message has already been sent! Use NetPeer.SendMessage() to send to multiple recipients efficiently")
                    throw;
            }*/

            networkCallBacks.OnClientStart?.Invoke();
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
