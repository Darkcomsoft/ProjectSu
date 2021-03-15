﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lidgren.Network;
using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.entity;
using Projectsln.darkcomsoft.src.entity.managers;
using Projectsln.darkcomsoft.src.misc;

namespace Projectsln.darkcomsoft.src.network
{
    public class NetworkServer : NetworkBase
    {
        private NetworkCallBacks networkCallBacks;
        private Dictionary<long, NetConnection> _connectionList;

        public NetworkServer(long ip, int port, int maxplayers)
        {
            networkCallBacks = new NetworkCallBacks();
            _connectionList = new Dictionary<long, NetConnection>();

            NetPeerConfiguration config = new NetPeerConfiguration(NetConfig.AppIdentifier);
            config.MaximumConnections = maxplayers;

            config.EnableUPnP = !NetConfig.DedicatedServer;
            config.AutoFlushSendQueue = true;
            config.DefaultOutgoingMessageCapacity = NetConfig.DefaultOutgoingMessageCapacity;
            config.UseMessageRecycling = NetConfig.UseMessageRecycling;
            config.SendBufferSize = NetConfig.SendBufferSize;
            config.AcceptIncomingConnections = NetConfig.AcceptConnection;
            config.NetworkThreadName = Application.AppName + " - Server";

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

                            foreach (var kvp in EntityManager.Instance.getEntityList)
                            {
                                NetViewSerializer neww = new NetViewSerializer();

                                neww.WorldType = kvp.Value.GetWorld.GetType().Name;
                                neww.EntityType = kvp.GetType().Name;

                                neww.Owner = kvp.Value.getOwner;
                                neww.ViewID = kvp.Value.getViewId;
                                neww.ChannelID = kvp.Value._currentChannelID;

                                neww.p_x = kvp.Value.transform.Position.X;
                                neww.p_y = kvp.Value.transform.Position.Y;
                                neww.p_z = kvp.Value.transform.Position.Z;

                                neww.r_x = kvp.Value.transform.Rotation.X;
                                neww.r_y = kvp.Value.transform.Rotation.Y;
                                neww.r_z = kvp.Value.transform.Rotation.Z;

                                netvi.Add(neww);
                            }

                            //Write the data to send when client connect
                            string data = JsonHelper.ToJson(netvi.ToArray());
                            string compressed = DataCompressor.CompressStringToByte(data);

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
            NetDataType type = (NetDataType)inc.ReadByte();

            switch (type)
            {
                case NetDataType.RPC:
                    break;
                case NetDataType.RPC_All:
                    break;
                case NetDataType.RPC_AllOwner:
                    break;
                case NetDataType.RPC_Owner:
                    break;
                case NetDataType.Spawn:
                    break;
                case NetDataType.Destroy:
                    break;
                default:
                    break;
            }
        }

        public override void Spawn(Entity entity)
        {
            int viewid = Utilits.UniqueID(5);

            var msg = m_peer.CreateMessage();

            msg.Write((byte)NetDataType.Spawn);

            msg.Write(entity.GetType().Name);//Entity Type
            msg.Write(entity.GetWorld.GetType().Name);//Curret Entity World

            msg.Write(viewid);
            msg.Write(entity.getRegionID);//Current region id
            msg.WriteVariableInt64(m_peer.UniqueIdentifier);//Netcode ID

            //Position
            msg.Write(entity.transform.Position.X);
            msg.Write(entity.transform.Position.Y);
            msg.Write(entity.transform.Position.Z);

            //Rotation
            msg.Write(entity.transform.Rotation.X);
            msg.Write(entity.transform.Rotation.Y);
            msg.Write(entity.transform.Rotation.Z);

            Server_SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
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

            _connectionList?.Clear();
            _connectionList = null;

            base.OnDispose();
        }

        /// <summary>
        /// Get a connection over the id of the connection
        /// </summary>
        /// <param name="uniq"></param>
        /// <returns></returns>
        private NetConnection GetConnection(long uniq)
        {
            if (_connectionList.TryGetValue(uniq, out NetConnection net))
            {
                return net;
            }

            Debug.Log("Dont found this connection: " + uniq);
            return null;
        }

        /// <summary>
        /// Send to everyone only dont to a specifieduser >> param:long ExcludeUniqid
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="method"></param>
        /// <param name="ExcludeUniqid"></param>
        private void Server_SendToAll(NetOutgoingMessage msg, NetDeliveryMethod method, long ExcludeUniqid)
        {
            if (_connectionList.Count <= 0) { return; }

            NetConnection net = GetConnection(ExcludeUniqid);

            if (net == null) { return; }

            PeerServer.SendToAll(msg, GetConnection(ExcludeUniqid), method, 0);
        }

        /// <summary>
        /// Send to everyone in connectted to server
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="method"></param>
        private void Server_SendToAll(NetOutgoingMessage msg, NetDeliveryMethod method)
        {
            if (_connectionList.Count <= 0) { return; }

            PeerServer.SendToAll(msg, method);
        }
    }
}
