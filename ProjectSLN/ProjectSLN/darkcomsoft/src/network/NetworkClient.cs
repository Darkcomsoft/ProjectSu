using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lidgren.Network;
using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.entity;

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
            config.ConnectionTimeout = NetConfig.ConnectionTimeout;
            config.NetworkThreadName = Application.AppName + " - Client";

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

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
            NetIncomingMessage inc = m_peer.ReadMessage();
            while (inc != null)
            {
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                        Debug.Log(inc.ReadString(), "NETWORK");
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        Debug.Log(inc.ReadString(), "NETWORK");
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        Debug.LogWarning(inc.ReadString(), "NETWORK");
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        string erro = inc.ReadString();
                        Debug.LogError(erro, "NETWORK");
                        if (erro == "Shutdown complete")
                        {
                            networkCallBacks.OnPlayerDisconnect?.Invoke(inc.SenderConnection);
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        ReadData(inc);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)inc.ReadByte();

                        if (status == NetConnectionStatus.Disconnected)
                        {
                            //networkCallBacks.OnDisconnect?.Invoke(); //VER SE PODE ADICIONAR ISSO AQUI DEPOIS
                        }
                        else if (status == NetConnectionStatus.Disconnecting)
                        {

                        }
                        else if (status == NetConnectionStatus.Connected)
                        {
                            networkCallBacks.OnConnect?.Invoke();
                        }
                        break;
                    default:
                        Debug.Log("Unhandled type: " + inc.MessageType + " " + inc.LengthBytes + " bytes " + inc.DeliveryMethod + "|" + inc.SequenceChannel, "NETWORK");

                        switch (inc.SenderConnection.Status)
                        {
                            case NetConnectionStatus.InitiatedConnect:
                                break;
                            case NetConnectionStatus.ReceivedInitiation:
                                break;
                            case NetConnectionStatus.RespondedAwaitingApproval:
                                break;
                            case NetConnectionStatus.RespondedConnect:
                                Debug.LogError("This Player : " + NetUtility.ToHexString(inc.SenderConnection.RemoteUniqueIdentifier) + " Are Accepted to server", "NETWORK");
                                break;
                            case NetConnectionStatus.Connected:
                                break;
                            case NetConnectionStatus.Disconnecting:
                                break;
                            case NetConnectionStatus.Disconnected:
                                networkCallBacks.OnPlayerDisconnect?.Invoke(inc.SenderConnection);
                                Debug.Log("Player : " + NetUtility.ToHexString(inc.SenderConnection.RemoteUniqueIdentifier) + " Disconnected!", "NETWORK");
                                break;
                            default:
                                break;
                        }
                        break;
                }
                m_peer.Recycle(inc);
            }
            base.Tick();
        }

        private void ReadData(NetIncomingMessage inc)
        {
            NetDataType type = (NetDataType)inc.ReadByte();

            switch (type)
            {
                case NetDataType.RPC:
                    break;
                case NetDataType.Spawn:
                    break;
                case NetDataType.Destroy:
                    break;
                case NetDataType.ConnectData:
                    break;
                default:
                    break;
            }
        }

        public override void Spawn(Entity entity)
        {
            base.Spawn(entity);
        }

        public override void Destroy(Entity entity)
        {
            base.Destroy(entity);
        }

        protected override void OnDispose()
        {
            networkCallBacks?.Dispose();
            networkCallBacks = null;
            base.OnDispose();
        }
    }
}