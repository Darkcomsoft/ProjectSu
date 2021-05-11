using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lidgren.Network;
using OpenTK.Mathematics;
using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.entity;
using Projectsln.darkcomsoft.src.entity.managers;
using Projectsln.darkcomsoft.src.misc;
using Projectsln.darkcomsoft.src.world;

namespace Projectsln.darkcomsoft.src.network
{
    public class NetworkServer : NetworkBase
    {
        private Dictionary<long, NetConnection> _connectionList;

        public NetworkServer(long ip, int port, int maxplayers)
        {
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

            NetworkCallBacks.OnServerStart?.Invoke();
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
                            NetworkCallBacks.OnPlayerDisconnect?.Invoke(inc.SenderConnection);
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        ReadData(inc);
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        string s = inc.ReadString();
                        if (NetworkCallBacks.PlayerApproval != null)
                        {
                            NetworkCallBacks.PlayerApproval.Invoke(s, inc.SenderConnection);
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
                            NetworkCallBacks.OnPlayerConnect?.Invoke(inc.SenderConnection);

                            List<NetViewSerializer> netvi = new List<NetViewSerializer>();

                            var OuMS = m_peer.CreateMessage();
                            OuMS.Write((byte)NetDataType.ConnectData);

                            foreach (var kvp in GameObjManager.Instance.getEntityList)
                            {
                                NetViewSerializer neww = new NetViewSerializer();

                                neww.WorldType = kvp.GetWorld.GetType().Name;
                                neww.EntityType = kvp.GetType().Name;

                                neww.Owner = kvp.getOwner;
                                neww.ViewID = kvp.getViewId;
                                neww.RegionID = kvp.getRegionID;

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
                            byte[] compressed = DataCompressor.CompressStringToByte(data);

                            OuMS.Write(compressed.Length);//send the bytes count to the Reder know how many need to read
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
                            NetworkCallBacks.OnPlayerDisconnect?.Invoke(inc.SenderConnection);

                            Entity[] entitys = GameObjManager.Instance.getEntityArray;

                            for (int i = 0; i < entitys.Length; i++)
                            {
                                Debug.Log("Entity : " + entitys[i].getOwner, "NETWORK");
                                if (entitys[i].getOwner == inc.SenderConnection.RemoteUniqueIdentifier)
                                {
                                    Debug.Log("Entity Destroyed: " + entitys[i].getOwner, "NETWORK");
                                    NetworkManager.DestroyEntity(entitys[i]);
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
                    ReceiveSpawnData(inc);
                    break;
                case NetDataType.Destroy:
                    ReceiveDestroyData(inc);
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

            entity.SetupEntityNetcode(viewid, m_peer.UniqueIdentifier);
            base.Spawn(entity);
        }

        public override void Destroy(Entity entity)
        {
            var msg = m_peer.CreateMessage();

            msg.Write((byte)NetDataType.Destroy);
            msg.Write(entity.getViewId);

            Server_SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
            base.Destroy(entity);
        }

        private void Disconnect()
        {
            PeerServer.Shutdown("The Server-System Requested to Shutdown!");
        }

        protected override void OnDispose()
        {
            Disconnect();

            NetworkCallBacks.OnServerStop?.Invoke();

            _connectionList?.Clear();
            _connectionList = null;

            base.OnDispose();
        }

        #region ReadReceiveData
        private void ReceiveSpawnData(NetIncomingMessage inc)
        {
            #region ReadClientReceiveMSG
            var typeName = inc.ReadString();//Entity Type
            var worldType = inc.ReadString();//Curret Entity World

            var viewId = inc.ReadInt32();
            var regionId = inc.ReadInt32();//Current region id
            var ownerId = inc.ReadVariableInt64();//Netcode ID

            Vector3d position = new Vector3d(inc.ReadDouble(), inc.ReadDouble(), inc.ReadDouble());
            Quaterniond rotation = new Quaterniond(inc.ReadDouble(), inc.ReadDouble(), inc.ReadDouble(), Quaterniond.Identity.W);

            //DISABILITA ESSE GODIGO EMBAIXO, APENAS PARA LEMBRETE, DEPOIS QUANDO TIVER PRONTO O NETCODE TALVEZ ATIVALO DENOVO
            //if (Type.GetType(typeName) == null || Type.GetType(worldType) == null) { return; }//check if this type exist
            //if (Type.GetType(typeName) != typeof(Entity).BaseType) { return; } // check if is derivated from the entity class

            Entity entityBase = GameObjManager.AddEntity(Type.GetType(typeName), WorldManager.GetWorld(Type.GetType(worldType)));
            entityBase.SetupEntityNetcode(viewId, ownerId);
            entityBase.transform.Position = position;
            entityBase.transform.Rotation = rotation;
            #endregion

            //Send to every one but exclude the sender
            #region ServerResend
            int viewid = Utilits.UniqueID(5);

            var msg = m_peer.CreateMessage();

            msg.Write((byte)NetDataType.Spawn);

            msg.Write(entityBase.GetType().Name);//Entity Type
            msg.Write(entityBase.GetWorld.GetType().Name);//Curret Entity World

            msg.Write(viewid);
            msg.Write(entityBase.getRegionID);//Current region id
            msg.WriteVariableInt64(m_peer.UniqueIdentifier);//Netcode ID

            //Position
            msg.Write(entityBase.transform.Position.X);
            msg.Write(entityBase.transform.Position.Y);
            msg.Write(entityBase.transform.Position.Z);

            //Rotation
            msg.Write(entityBase.transform.Rotation.X);
            msg.Write(entityBase.transform.Rotation.Y);
            msg.Write(entityBase.transform.Rotation.Z);

            Server_SendToAll(msg, NetDeliveryMethod.ReliableOrdered, ownerId);
            #endregion
        }

        private void ReceiveDestroyData(NetIncomingMessage inc)
        {
            var viewId = inc.ReadInt32();
            var owner = NetworkManager.instance.getNetViewEntityList[viewId].getOwner;

            if (inc.SenderConnection.RemoteUniqueIdentifier != owner) { return; }//if the destroy sender don't own the entity return, only owner of the entity can destroy the entity or the server

            var msg = m_peer.CreateMessage();

            msg.Write((byte)NetDataType.Destroy);
            msg.Write(viewId);

            Server_SendToAll(msg, NetDeliveryMethod.ReliableOrdered, owner);//send to everyone to destroy this entity

            GameObjManager.RemoveEntity(NetworkManager.instance.getNetViewEntityList[viewId]);//Destroy the entity in engine
        }
        #endregion

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
