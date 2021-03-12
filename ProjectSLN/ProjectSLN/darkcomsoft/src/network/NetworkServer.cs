using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lidgren.Network;
using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.entity.managers;

namespace Projectsln.darkcomsoft.src.network
{
    public class NetworkServer : NetworkBase
    {
        private NetworkCallBacks networkCallBacks;
        private Dictionary<long, NetConnection> _connectionList;

        public NetworkServer(long ip, int port, int maxplayers)
        {
            networkCallBacks = new NetworkCallBacks();
            _connectionList = new Dictionary<long, NetConnection>;

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

            Debug.Log("Unique identifier is " + peer.UniqueIdentifier, "NETWORK");

            networkCallBacks.OnServerStart?.Invoke();
        }

        public override void Tick()
        {
            NetIncomingMessage inc;
            while ((inc = m_peer.ReadMessage()) != null)
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
                        Debug.Log(erro, "NETWORK");
                        if (erro == "Shutdown complete")
                        {
                            networkCallBacks.OnPlayerDisconnect?.Invoke(inc.SenderConnection);
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        ReadData(inc);
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        string s = inc.ReadString();
                        if (networkCallBacks.PlayerApproval != null)
                        {
                            networkCallBacks.PlayerApproval?.Invoke(s, inc.SenderConnection);
                        }
                        else
                        {
                            if (s == NetConfig.SecretKey)
                            {
                                inc.SenderConnection.Approve();
                            }
                            else
                            {
                                inc.SenderConnection.Deny("Sorry your data isnt equal to server!");
                            }
                        }
                        break;
                    default:
                        if (inc.SenderConnection.Status == NetConnectionStatus.Connected)
                        {
                            networkCallBacks.OnPlayerConnect?.Invoke(inc.SenderConnection);

                            List<NetViewSerializer> netvi = new List<NetViewSerializer>();

                            var OuMS = m_peer.CreateMessage();
                            OuMS.Write((byte)NetDataType.ConnectData);

                            foreach (var kvp in EntityManager.Instance)
                            {
                                NetViewSerializer neww = new NetViewSerializer();
                                neww.EntityTypeName = kvp.GetType().Name;

                                neww.SpaceName = kvp.SpaceName;
                                neww.Owner = kvp._Owner;
                                neww.ViewID = kvp._ViewID;
                                neww.ChannelID = kvp._currentChannelID;

                                neww.p_x = kvp.transform.Position.X;
                                neww.p_y = kvp.transform.Position.Y;
                                neww.p_z = kvp.transform.Position.Z;

                                neww.r_x = kvp.transform.Rotation.X;
                                neww.r_y = kvp.transform.Rotation.Y;
                                neww.r_z = kvp.transform.Rotation.Z;

                                netvi.Add(neww);
                            }

                            //Write the data to send when client connect
                            string data = JsonHelper.ToJson(netvi.ToArray());
                            string compressed = CompressString.StringCompressor.CompressString(data);

                            OuMS.Write(compressed);

                            Debug.Log("ConnectData(XML): " + data, "NETWORK");
                            Debug.Log("ConnectData(CompressString): " + compressed, "NETWORK");

                            m_peer.SendMessage(OuMS, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered);//Send the data to Connection
                        }
                        else if (inc.SenderConnection.Status == NetConnectionStatus.RespondedConnect)
                        {
                            Debug.Log("This Player : " + NetUtility.ToHexString(inc.SenderConnection.RemoteUniqueIdentifier) + " Are Accepted to server", "NETWORK");

                            _connectionList.Add(inc.SenderConnection.RemoteUniqueIdentifier, inc.SenderConnection);
                        }
                        else if (inc.SenderConnection.Status == NetConnectionStatus.Disconnected)
                        {
                            networkCallBacks.OnPlayerDisconnect?.Invoke(inc.SenderConnection);

                            NEntity[] obj = _NetviewList.Values.ToArray();

                            for (int i = 0; i < obj.Length; i++)
                            {
                                Debug.Log("Entity : " + obj[i]._Owner, "NETWORK");
                                if (obj[i]._Owner == inc.SenderConnection.RemoteUniqueIdentifier)
                                {
                                    Debug.Log("Entity Destroyed: " + obj[i]._Owner, "NETWORK");
                                    Destroy(obj[i]);
                                }
                            }

                            _connectionList.Remove(inc.SenderConnection.RemoteUniqueIdentifier);

                            Debug.Log("Player : " + NetUtility.ToHexString(inc.SenderConnection.RemoteUniqueIdentifier) + " Disconnected!", "NETWORK");
                        }
                        else if (inc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
                        {
                            //last paket sande to client, and after this is disconnect
                        }
                        break;
                }
                m_peer.Recycle(inc);
            }
            base.Tick();
        }

        private void ReadData(NetIncomingMessage inc)
        {

        }

        protected override void OnDispose()
        {
            networkCallBacks?.Dispose();
            networkCallBacks = null;

            _connectionList?.Clear();
            _connectionList = null;

            base.OnDispose();
        }
    }
}
