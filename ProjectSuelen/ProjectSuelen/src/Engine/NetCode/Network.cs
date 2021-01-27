﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ProjectSuelen;
using Lidgren.Network;
using OpenTK;
using ProjectSuelen.src.Engine.Entitys;
using ProjectSuelen.src.Engine;
using Newtonsoft.Json;
using ProjectSuelen.src.Entitys;

namespace ProjectSuelen.src.Engine.NetCode
{
    public class Network : ClassBase
    {
        internal static NetPeer MyPeer;
        internal static NetConnection MyConnection;
        internal static NetServer Server;
        internal static NetClient Client;
        internal static NetDeliveryMethod DefaultDeliveryMethod;
        internal static bool Runing;

        private static bool _ISSERVER = false;
        private static bool _ISCLIENT = false;

        private static Dictionary<long, NetConnection> ConnectionList = new Dictionary<long, NetConnection>();
        public static Dictionary<int, NEntity> _NetviewList = new Dictionary<int, NEntity>();

        public static bool IsServer { get { return _ISSERVER; } }
        public static bool IsClient { get { return _ISCLIENT; } }
        public static bool Ready { get; private set; }
        public static NetPeerStatistics PeerStat;

        private static NetConnectionStatus _NetConnectionStatus;

        public static Action<NetConnection> OnPlayerDisconnect;
        public static Action<NetConnection> OnPlayerConnect;
        public static Action<string, NetConnection> PlayerApproval;
        public static Action OnDisconnect;
        public static Action OnConnect;
        public static Action OnServerStart;
        public static Action OnServerStop;
        public static Action OnClientStart;
        public static Action OnReceivedServerData;

        protected override void OnDispose()
        {
            Disconnect();
            base.OnDispose();
        }

        /// <summary>
        /// Create a server, local server or dedicated server
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="maxplayers"></param>
        /// <returns></returns>
        public static bool CreateServer(string ip, int port, int maxplayers)
        {
            if (Client == null && Server == null)
            {
                Runing = true;

                DefaultDeliveryMethod = NetDeliveryMethod.ReliableOrdered;

                bool Started = false;
                long ipe = 0;
                long.TryParse(ip, out ipe);

                NetPeerConfiguration config = new NetPeerConfiguration(NetConfig.AppIdentifier);
                config.MaximumConnections = maxplayers;

                config.EnableUPnP = !NetConfig.DedicatedServer;
                config.AutoFlushSendQueue = true;
                config.DefaultOutgoingMessageCapacity = NetConfig.DefaultOutgoingMessageCapacity;
                config.UseMessageRecycling = NetConfig.UseMessageRecycling;
                config.SendBufferSize = NetConfig.SendBufferSize;
                config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
                config.AcceptIncomingConnections = NetConfig.AcceptConnection;
                config.NetworkThreadName = "DarckNet - Server";

                config.Port = port;
                config.BroadcastAddress = new IPAddress(ipe);

                NetServer peer = new NetServer(config);
                peer.Start(); // needed for initialization

                MyPeer = peer;

                if (peer.Status == NetPeerStatus.Running)
                {
                    Started = true;
                }

                Server = peer;
                PeerStat = peer.Statistics;
                MyConnection = ServerConnection;

                _ISCLIENT = false;
                _ISSERVER = true;

                Debug.Log("Unique identifier is " + peer.UniqueIdentifier);
                Ready = true;

                if (OnServerStart != null)
                {
                    OnServerStart();
                }
                return Started;
            }
            else
            {
                Debug.LogError("Server already started");
                return false;
            }
        }

        /// <summary>
        /// Connect to remote server.
        /// </summary>
        /// <param name="Ip"> ip to connect </param>
        /// <param name="Port"></param>
        /// <param name="Password"></param>
        public static NetPeer Connect(string Ip, int Port, string Password)
        {
            if (Server == null && Client == null)
            {
                Runing = true;

                DefaultDeliveryMethod = NetDeliveryMethod.ReliableOrdered;

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

                MyPeer = peer;

                NetOutgoingMessage approval = peer.CreateMessage();
                approval.Write(NetConfig.SecretKey);

                peer.Connect(Ip, Port, approval);

                Client = peer;
                PeerStat = peer.Statistics;

                _ISCLIENT = true;
                _ISSERVER = false;

                Debug.Log("Unique identifier is " + peer.UniqueIdentifier);

                Ready = true;

                var om = peer.CreateMessage();
                peer.SendUnconnectedMessage(om, new IPEndPoint(IPAddress.Loopback, Port));
                try
                {
                    peer.SendUnconnectedMessage(om, new IPEndPoint(IPAddress.Loopback, Port));
                }
                catch (NetException nex)
                {
                    if (nex.Message != "This message has already been sent! Use NetPeer.SendMessage() to send to multiple recipients efficiently")
                        throw;
                }

                if (OnClientStart != null)
                {
                    OnClientStart();
                }
                return peer;
            }
            else
            {
                Debug.LogError("You already connected in some server");
                return null;
            }
        }


        public static void Instantiate(string spacename, NEntity entity)
        {
            if (Ready)
            {
                int viewid = UniqueID(5);

                if (IsClient)
                {
                    entity.SetUpNet(viewid, MyPeer.UniqueIdentifier, NetDeliveryMethod.Unreliable);
                    GameObject.Instantiate(spacename, entity);

                    Debug.Log("Spawned A Entity, ID: " + viewid);
                    var om = MyPeer.CreateMessage();

                    /*while (NetworkViews.ContainsKey(viewid))
                    {
                        viewid = UniqueID(5);
                    }*/

                    om.Write((byte)DataType.Instantiate);

                    om.Write(entity.GetType().Name);

                    om.Write(spacename);
                    om.Write(viewid);
                    om.Write(entity._currentChannelID);
                    om.WriteVariableInt64(MyPeer.UniqueIdentifier);

                    //Position
                    om.Write(entity.transform.Position.X);
                    om.Write(entity.transform.Position.Y);
                    om.Write(entity.transform.Position.Z);

                    //Rotation
                    om.Write(entity.transform.Rotation.X);
                    om.Write(entity.transform.Rotation.Y);
                    om.Write(entity.transform.Rotation.Z);

                    Client.SendMessage(om, DefaultDeliveryMethod);
                }
                else
                {
                    entity.SetUpNet(viewid, MyPeer.UniqueIdentifier, NetDeliveryMethod.Unreliable);
                    GameObject.Instantiate(spacename, entity);

                    Debug.Log("Spawned A Entity, ID: " + viewid);
                    var om = MyPeer.CreateMessage();

                    /*while (NetworkViews.ContainsKey(viewid))
                    {
                        viewid = UniqueID(5);
                    }*/

                    om.Write((byte)DataType.Instantiate);

                    om.Write(entity.GetType().Name);

                    om.Write(spacename);
                    om.Write(viewid);
                    om.Write(entity._currentChannelID);
                    om.WriteVariableInt64(MyPeer.UniqueIdentifier);

                    //Position
                    om.Write(entity.transform.Position.X);
                    om.Write(entity.transform.Position.Y);
                    om.Write(entity.transform.Position.Z);

                    //Rotation
                    om.Write(entity.transform.Rotation.X);
                    om.Write(entity.transform.Rotation.Y);
                    om.Write(entity.transform.Rotation.Z);

                    Server_SendToAll(om, DefaultDeliveryMethod, MyPeer.UniqueIdentifier);
                }
            }
        }

        public static void Destroy(NEntity entity)
        {
            if (Ready)
            {
                int Dimension = entity._currentChannelID;
                int viewid = entity._ViewID;

                var om = MyPeer.CreateMessage();

                om.Write((byte)DataType.DestroyEntity);
                om.Write(viewid);

                if (IsClient)
                {
                    Client.SendMessage(om, DefaultDeliveryMethod);
                    GameObject.Destroy(entity);
                }
                else if (IsServer)
                {
                    Server_SendToAll(om, DefaultDeliveryMethod);
                    GameObject.Destroy(entity);
                }
            }
            else
            {
                Debug.LogError("You are disconnected from any server, you can't destroy objects!");
            }
        }

        public static void RequestServerData(string UserName, int UserID)
        {
            var om = MyPeer.CreateMessage();

            om.Write((byte)DataType.RequestServerData);

            om.Write(UserName);
            om.Write(UserID);

            Client.SendMessage(om, DefaultDeliveryMethod);
        }

        public static I CreateInstance<I>() where I : class
        {
            string assemblyPath = Environment.CurrentDirectory + "\\DynamicCreateInstanceofclass.exe";

            Assembly assembly;

            assembly = Assembly.LoadFrom(assemblyPath);
            Type type = assembly.GetType("DynamicCreateInstanceofclass.UserDetails");
            return Activator.CreateInstance(type) as I;
        }

        /// <summary>
        /// Move a entity to a Region
        /// </summary>
        /// <param name="entity"></param>
        public static void MoveEntityToRegion(Entity entity, int channelId)
        {
            throw new NotImplementedException("NotImplemented: For now is this disabled!");
            /*RemoveEntityFromRegion(entity);//Firts Remove and move to the new one

            entity._currentChannelID = channelId;*/
        }
        public static void RemoveEntityFromRegion(Entity entity)
        {
            //Destroy the entity on the current region
            throw new NotImplementedException("NotImplemented: For now is this disabled!");
        }

        /// <summary>
        /// Send to everyone only dont to a specifieduser >> param:long ExcludeUniqid
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="method"></param>
        /// <param name="ExcludeUniqid"></param>
        public static void Server_SendToAll(NetOutgoingMessage msg, NetDeliveryMethod method, long ExcludeUniqid)
        {
            if (!IsServer) { return; }
            if (ConnectionList.Count <= 0) { return; }

            NetConnection net = GetConnection(ExcludeUniqid);

            if (net == null) { return; }

            Server.SendToAll(msg, GetConnection(ExcludeUniqid), method, 0);
        }

        /// <summary>
        /// Send to everyone in connectted to server
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="method"></param>
        public static void Server_SendToAll(NetOutgoingMessage msg, NetDeliveryMethod method)
        {
            if (!IsServer) { return; }

            if (ConnectionList.Count <= 0) { return; }

            Server.SendToAll(msg, method);
        }

        public static EntityType GetEntityType(Entity entity)
        {

            return EntityType.none;
        }

        /// <summary>
        /// Get a connection over the id of the connection
        /// </summary>
        /// <param name="uniq"></param>
        /// <returns></returns>
        public static NetConnection GetConnection(long uniq)
        {
            if (ConnectionList.TryGetValue(uniq, out NetConnection net))
            {
                return net;
            }

            Debug.Log("Dont found this connection: " + uniq);
            return null;
        }

        public static NetConnection GetMyConnection()
        {
            foreach (var item in MyPeer.Connections)
            {
                if (item.RemoteUniqueIdentifier == MyPeer.UniqueIdentifier)
                {
                    return item;
                }
            }

            Debug.LogError("Sorry this connection don't exist!");
            return null;
        }

        public static NetConnection ServerConnection
        {
            get
            {
                NetConnection retval = null;
                if (MyPeer.Connections.Count > 0)
                {
                    try
                    {
                        retval = MyPeer.Connections[0];
                    }
                    catch
                    {
                        // preempted!
                        return null;
                    }
                }
                return retval;
            }
        }

        /// <summary>
        /// Generate a unique id. Length is for how long you want to be the id, 1 is normal(short)
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static int UniqueID(int Length)
        {
            Random random = new Random();
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
            int z1 = random.Next(0, 1000000);
            int z2 = random.Next(0, 1000);
            return (currentEpochTime / z1 + z2 * Length);
        }

        /// <summary>
        /// Disconnect From Server, or if is server Shutdown the server.
        /// </summary>
        public static void Disconnect()
        {
            if (IsServer)
            {
                var om = MyPeer.CreateMessage();

                Ready = false;//Stop any outgoing pakets

                om.Write((byte)DataType.ServerStop);
                Server.SendToAll(om, DefaultDeliveryMethod);

                Debug.Log("SERVER-NET: Waiting for everyone disconnect, before shutdown the server");
                while (Server.Connections.Count > 0) { }
                Debug.Log("SERVER-NET: Everyone is out, now we can shutdown the server (:");

                Server.Shutdown("ServerClosed");

                Client = null;
                Server = null;
                MyPeer = null;
                Runing = false;

                _ISCLIENT = false;
                _ISSERVER = false;

                //Clear the entitys
                /*foreach (var item in _NetviewList)
                {
                    GameObject.Destroy(item.Value);
                }*/

                _NetviewList.Clear();
                ConnectionList.Clear();

                if (OnServerStop != null)
                {
                    OnServerStop();
                }

                Debug.Log("SERVER-NET: Server is Offline, and memory is clear!");
            }
            else if (IsClient)
            {
                Client.Disconnect("Disconnect");
            }
        }

        /// <summary>
        ///  Update method network
        /// </summary>
        public static void NetworkTick()
        {
            if (MyPeer != null)
            {
                if (IsServer)
                {
                    NetIncomingMessage inc;
                    while ((inc = Server.ReadMessage()) != null)
                    {
                        switch (inc.MessageType)
                        {
                            case NetIncomingMessageType.VerboseDebugMessage:
                                Debug.Log(inc.ReadString());
                                break;
                            case NetIncomingMessageType.DebugMessage:
                                Debug.Log(inc.ReadString());
                                break;
                            case NetIncomingMessageType.WarningMessage:
                                Debug.LogWarning(inc.ReadString());
                                break;
                            case NetIncomingMessageType.ErrorMessage:
                                string erro = inc.ReadString();
                                Debug.Log(erro);
                                if (erro == "Shutdown complete")
                                {
                                    if (OnPlayerDisconnect != null)
                                    {
                                        OnPlayerDisconnect(inc.SenderConnection);
                                    }
                                }
                                break;
                            case NetIncomingMessageType.Data:
                                ReadServerData(inc);
                                break;
                            case NetIncomingMessageType.ConnectionApproval:
                                string s = inc.ReadString();
                                if (PlayerApproval != null)
                                {
                                    PlayerApproval.Invoke(s, inc.SenderConnection);
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
                                    if (OnPlayerConnect != null)
                                    {
                                        OnPlayerConnect(inc.SenderConnection);
                                    }

                                    List<NetViewSerializer> netvi = new List<NetViewSerializer>();

                                    var OuMS = MyPeer.CreateMessage();
                                    OuMS.Write((byte)DataType.OnConnectData);

                                    foreach (var kvp in _NetviewList.Values)
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

                                    Debug.Log("ConnectData(XML): " + data);
                                    Debug.Log("ConnectData(CompressString): " + compressed);

                                    Server.SendMessage(OuMS, inc.SenderConnection, DefaultDeliveryMethod);//Send the data to Connection
                                }
                                else if (inc.SenderConnection.Status == NetConnectionStatus.RespondedConnect)
                                {
                                    Debug.Log("This Player : " + NetUtility.ToHexString(inc.SenderConnection.RemoteUniqueIdentifier) + " Are Accepted to server");

                                    ConnectionList.Add(inc.SenderConnection.RemoteUniqueIdentifier, inc.SenderConnection);
                                }
                                else if (inc.SenderConnection.Status == NetConnectionStatus.Disconnected)
                                {
                                    if (OnPlayerDisconnect != null)
                                    {
                                        OnPlayerDisconnect(inc.SenderConnection);
                                    }

                                    NEntity[] obj = _NetviewList.Values.ToArray();

                                    for (int i = 0; i < obj.Length; i++)
                                    {
                                        Debug.Log("Entity : " + obj[i]._Owner);
                                        if (obj[i]._Owner == inc.SenderConnection.RemoteUniqueIdentifier)
                                        {
                                            Debug.Log("Entity Destroyed: " + obj[i]._Owner);
                                            Destroy(obj[i]);
                                        }
                                    }

                                    ConnectionList.Remove(inc.SenderConnection.RemoteUniqueIdentifier);

                                    Debug.Log("Player : " + NetUtility.ToHexString(inc.SenderConnection.RemoteUniqueIdentifier) + " Disconnected!");
                                }
                                else if (inc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
                                {
                                    //last paket sande to client, and after this is disconnect
                                }
                                break;
                        }
                        Server.Recycle(inc);
                    }
                }
                else if (IsClient)
                {
                    NetIncomingMessage inc;
                    while ((inc = Client.ReadMessage()) != null)
                    {
                        switch (inc.MessageType)
                        {
                            case NetIncomingMessageType.VerboseDebugMessage:
                                Debug.Log(inc.ReadString());
                                break;
                            case NetIncomingMessageType.DebugMessage:
                                Debug.Log(inc.ReadString());
                                break;
                            case NetIncomingMessageType.WarningMessage:
                                Debug.LogWarning(inc.ReadString());
                                break;
                            case NetIncomingMessageType.ErrorMessage:
                                string erro = inc.ReadString();
                                Debug.Log(erro);
                                if (erro == "Shutdown complete")
                                {
                                    if (OnPlayerDisconnect != null)
                                    {
                                        OnPlayerDisconnect(inc.SenderConnection);
                                    }
                                }
                                break;
                            case NetIncomingMessageType.Data:
                                ReadClientData(inc);
                                break;
                            case NetIncomingMessageType.StatusChanged:
                                NetConnectionStatus status = (NetConnectionStatus)inc.ReadByte();

                                if (status == NetConnectionStatus.Disconnected)
                                {
                                    _NetConnectionStatus = status;
                                }
                                else if (status == NetConnectionStatus.Disconnecting)
                                {

                                }
                                else if (status == NetConnectionStatus.Connected)
                                {
                                    if (OnConnect != null)
                                    {
                                        OnConnect();
                                    }
                                }
                                break;
                            default:
                                switch (inc.SenderConnection.Status)
                                {
                                    case NetConnectionStatus.None:
                                        break;
                                    case NetConnectionStatus.InitiatedConnect:
                                        break;
                                    case NetConnectionStatus.ReceivedInitiation:
                                        break;
                                    case NetConnectionStatus.RespondedAwaitingApproval:
                                        break;
                                    case NetConnectionStatus.RespondedConnect:
                                        ConnectionList.Add(inc.SenderConnection.RemoteUniqueIdentifier, inc.SenderConnection);
                                        Debug.LogError("This Player : " + NetUtility.ToHexString(inc.SenderConnection.RemoteUniqueIdentifier) + " Are Accepted to server");
                                        break;
                                    case NetConnectionStatus.Connected:
                                        break;
                                    case NetConnectionStatus.Disconnecting:
                                        break;
                                    case NetConnectionStatus.Disconnected:
                                        if (OnPlayerDisconnect != null)
                                        {
                                            OnPlayerDisconnect(inc.SenderConnection);
                                        }

                                        NEntity[] obj = _NetviewList.Values.ToArray();

                                        for (int i = 0; i < obj.Length; i++)
                                        {
                                            Debug.Log("Entity : " + obj[i]._Owner);
                                            if (obj[i]._Owner == inc.SenderConnection.RemoteUniqueIdentifier)
                                            {
                                                Debug.Log("Entity Destroyed: " + obj[i]._Owner);
                                                Destroy(obj[i]);
                                            }
                                        }

                                        ConnectionList.Remove(inc.SenderConnection.RemoteUniqueIdentifier);

                                        Debug.Log("Player : " + NetUtility.ToHexString(inc.SenderConnection.RemoteUniqueIdentifier) + " Disconnected!");
                                        break;
                                    default:
                                        break;
                                }
                                break;
                        }
                        Client.Recycle(inc);
                    }
                }
            }

            if (_NetConnectionStatus == NetConnectionStatus.Disconnected)
            {
                Client = null;
                Server = null;
                MyPeer = null;
                Runing = false;

                Ready = false;

                _ISCLIENT = false;
                _ISSERVER = false;

                //Clear the entitys
                foreach (var item in _NetviewList)
                {
                    GameObject.Destroy(item.Value);
                }

                _NetviewList.Clear();
                ConnectionList.Clear();

                if (OnDisconnect != null)
                {
                    OnDisconnect();
                }

                _NetConnectionStatus = NetConnectionStatus.None;
            }
        }

        private static void ReadServerData(NetIncomingMessage inc)
        {
            DataType type = (DataType)inc.ReadByte();

            switch (type)
            {
                case DataType.RPC:
                    break;
                case DataType.RPC_All:
                    RPC_All(inc);
                    break;
                case DataType.RPC_AllOwner:
                    RPC_AllOwner(inc);
                    break;
                case DataType.RPC_Owner:
                    RPC_Owner(inc);
                    break;
                case DataType.Instantiate:
                    NetViewSerializer instantiateEntity = new NetViewSerializer();

                    instantiateEntity.EntityTypeName = inc.ReadString();

                    instantiateEntity.SpaceName = inc.ReadString();

                    instantiateEntity.ViewID = inc.ReadInt32();
                    instantiateEntity.ChannelID = inc.ReadInt32();
                    instantiateEntity.Owner = inc.ReadVariableInt64();

                    //Position
                    instantiateEntity.p_x = inc.ReadFloat();
                    instantiateEntity.p_y = inc.ReadFloat();
                    instantiateEntity.p_z = inc.ReadFloat();

                    //Rotation
                    instantiateEntity.r_x = inc.ReadFloat();
                    instantiateEntity.r_y = inc.ReadFloat();
                    instantiateEntity.r_z = inc.ReadFloat();

                    GameObject.Instantiate(instantiateEntity.SpaceName, (NEntity)Activator.CreateInstance(Type.GetType(instantiateEntity.EntityTypeName)));

                    ///Send to all to instantiate///
                    if (MyPeer.Connections.Count > 0)
                    {
                        var om = MyPeer.CreateMessage();

                        om.Write((byte)DataType.Instantiate);

                        om.Write(instantiateEntity.EntityTypeName);

                        om.Write(instantiateEntity.SpaceName);
                        om.Write(instantiateEntity.ViewID);
                        om.Write(instantiateEntity.ChannelID);
                        om.WriteVariableInt64(instantiateEntity.Owner);

                        //Position
                        om.Write(instantiateEntity.p_x);
                        om.Write(instantiateEntity.p_y);
                        om.Write(instantiateEntity.p_z);

                        //Rotation
                        om.Write(instantiateEntity.r_x);
                        om.Write(instantiateEntity.r_y);
                        om.Write(instantiateEntity.r_z);

                        Server_SendToAll(om, DefaultDeliveryMethod, inc.SenderConnection.RemoteUniqueIdentifier);
                    }
                    break;
                case DataType.DestroyEntity:
                    int viewid = inc.ReadInt32();
                    GameObject.Destroy(_NetviewList[viewid]);

                    //Send To all Clients
                    if (MyPeer.Connections.Count > 0)
                    {
                        var destroyMSG = MyPeer.CreateMessage();

                        destroyMSG.Write((byte)DataType.DestroyEntity);
                        destroyMSG.Write(viewid);

                        Server_SendToAll(destroyMSG, DefaultDeliveryMethod, inc.SenderConnection.RemoteUniqueIdentifier);
                    }
                    break;
                case DataType.ExitDimension:
                    break;
                case DataType.RequestServerData:
                    string charName = inc.ReadString();
                    int userId = inc.ReadInt32();

                    //check if this player is the same of the owner id

                    var outms = MyPeer.CreateMessage();

                    Server.SendMessage(outms, inc.SenderConnection, DefaultDeliveryMethod);//Send the data to Connection
                    break;
                default:
                    break;
            }
        }

        private static void ReadClientData(NetIncomingMessage inc)
        {
            DataType type = (DataType)inc.ReadByte();

            switch (type)
            {
                case DataType.RPC:
                    inc.ReadString(out string funcname);
                    inc.ReadInt32(out int viewidd);

                    if (_NetviewList.TryGetValue(viewidd, out NEntity Net))
                    {
                        Net.Execute(funcname, inc);
                    }
                    break;
                case DataType.Instantiate:
                    NetViewSerializer instantiateEntity = new NetViewSerializer();

                    instantiateEntity.EntityTypeName = inc.ReadString();

                    instantiateEntity.SpaceName = inc.ReadString(); ;
                    instantiateEntity.ViewID = inc.ReadInt32();
                    instantiateEntity.ChannelID = inc.ReadInt32();
                    instantiateEntity.Owner = inc.ReadVariableInt64();

                    //Position
                    instantiateEntity.p_x = inc.ReadFloat();
                    instantiateEntity.p_y = inc.ReadFloat();
                    instantiateEntity.p_z = inc.ReadFloat();

                    //Rotation
                    instantiateEntity.r_x = inc.ReadFloat();
                    instantiateEntity.r_y = inc.ReadFloat();
                    instantiateEntity.r_z = inc.ReadFloat();

                    GameObject.Instantiate(instantiateEntity.SpaceName, (NEntity)Activator.CreateInstance(Type.GetType(instantiateEntity.EntityTypeName)));
                    break;
                case DataType.DestroyEntity:
                    int viewid = inc.ReadInt32();
                    GameObject.Destroy(_NetviewList[viewid]);
                    break;
                case DataType.OnConnectData:

                    string compressed = inc.ReadString();
                    string xml = CompressString.StringCompressor.DecompressString(compressed);

                    NetViewSerializer[] entitylist = JsonHelper.FromJson<NetViewSerializer>(xml);

                    foreach (var kvp in entitylist)
                    {
                        Debug.Log("EntityReceived: " + kvp.ViewID);
                        GameObject.Instantiate(kvp.SpaceName,(NEntity)Activator.CreateInstance(Type.GetType(kvp.EntityTypeName)));
                    }

                    if (OnReceivedServerData != null)
                    {
                        OnReceivedServerData();
                    }
                    break;
                case DataType.ExitDimension:
                    break;
                case DataType.ServerStop:
                    Debug.Log("Server Stoped!");
                    Disconnect();
                    break;
                case DataType.ServerDataReceived:

                    if (OnReceivedServerData != null)
                    {
                        OnReceivedServerData();
                    }
                    break;
                default:
                    break;
            }
        }

        #region RPC_Type_Receive

        private static void RPC_All(NetIncomingMessage inc)
        {
            inc.ReadString(out string funcname);
            inc.ReadInt32(out int viewidd);
            //inc.ReadInt32(out int d);

            NEntity Net = _NetviewList[viewidd];

            object[] obj = Net.Execute(funcname, inc);

            var om = Network.MyPeer.CreateMessage();
            om.Write((byte)DataType.RPC);

            om.Write(funcname);
            om.Write(viewidd);

            DoData(om, obj);

            Server_SendToAll(om, Net._DefaultNetDeliveryMethod);
        }

        private static void RPC_AllOwner(NetIncomingMessage inc)
        {
            inc.ReadString(out string funcname);
            inc.ReadInt32(out int viewidd);
            //inc.ReadInt32(out int d);
            //inc.ReadInt64();

            NEntity Net = _NetviewList[viewidd];

            object[] obj = Net.Execute(funcname, inc);

            var om = Network.MyPeer.CreateMessage();
            om.Write((byte)DataType.RPC);

            om.Write(funcname);
            om.Write(viewidd);

            DoData(om, obj);

            Server_SendToAll(om, Net._DefaultNetDeliveryMethod, Net._Owner);
        }

        private static void RPC_Owner(NetIncomingMessage inc)
        {
            inc.ReadString(out string funcname);
            inc.ReadInt32(out int viewidd);
            NEntity Net = _NetviewList[viewidd];

            object[] obj = Net.Execute(funcname, inc);


            var om = Network.MyPeer.CreateMessage();
            om.Write((byte)DataType.RPC);

            om.Write(funcname);
            om.Write(viewidd);

            DoData(om, obj);

            Server.SendMessage(om, GetConnection(Net._Owner), inc.DeliveryMethod);
        }

        private static NetOutgoingMessage DoData(NetOutgoingMessage om, object[] param)
        {
            for (int i = 0; i < param.Length; i++)
            {
                if (param[i].GetType() == typeof(string))
                {
                    om.Write((string)param[i]);
                }
                else if (param[i].GetType() == typeof(int))
                {
                    om.Write((int)param[i]);
                }
                else if (param[i].GetType() == typeof(bool))
                {
                    om.Write((bool)param[i]);
                }
                else if (param[i].GetType() == typeof(double))
                {
                    om.Write((double)param[i]);
                }
                else if (param[i].GetType() == typeof(Vector3d))
                {
                    Vector3d vec = (Vector3d)param[i];

                    om.Write(vec.X);
                    om.Write(vec.Y);
                    om.Write(vec.Z);
                }
                else if (param[i].GetType() == typeof(Vector2d))
                {
                    Vector2d vec = (Vector2d)param[i];

                    om.Write(vec.X);
                    om.Write(vec.Y);
                }
                else if (param[i].GetType() == typeof(Quaterniond))
                {
                    Quaterniond vec = (Quaterniond)param[i];

                    om.Write(vec.X);
                    om.Write(vec.Y);
                    om.Write(vec.Z);
                }
            }

            return om;
        }

        #endregion

        #region RPC_Type_Local

        public static void RPC_All(string funcname, int viewid, object[] param)
        {
            NEntity Net = _NetviewList[viewid];

            Net.Execute(funcname, param);

            var om = Network.MyPeer.CreateMessage();
            om.Write((byte)DataType.RPC);

            om.Write(funcname);
            om.Write(viewid);

            DoData(om, param);

            Server_SendToAll(om, Net._DefaultNetDeliveryMethod);
        }

        public static void RPC_AllOwner(string funcname, int viewid, object[] param)
        {
            NEntity Net = _NetviewList[viewid];

            if (Net._Owner != MyPeer.UniqueIdentifier)
            {
                Net.Execute(funcname, param);
            }

            var om = Network.MyPeer.CreateMessage();
            om.Write((byte)DataType.RPC);

            om.Write(funcname);
            om.Write(viewid);

            DoData(om, param);

            Server_SendToAll(om, Net._DefaultNetDeliveryMethod, Net._Owner);
        }

        public static void RPC_Owner(string funcname, int viewid, object[] param)
        {
            NEntity Net = _NetviewList[viewid];

            var om = Network.MyPeer.CreateMessage();
            om.Write((byte)DataType.RPC);

            om.Write(funcname);
            om.Write(viewid);

            DoData(om, param);

            Server.SendMessage(om, GetConnection(Net._Owner), Net._DefaultNetDeliveryMethod);
        }

        public static void RPC_Server(string funcname, int viewid, object[] param)
        {
            NEntity Net = _NetviewList[viewid];
            Net.Execute(funcname, param);
        }

        #endregion
    }

    [Serializable]
    public class NetViewSerializer
    {
        public string SpaceName;

        public int ChannelID = 0;
        public string EntityTypeName;
        public long Owner;
        public int ViewID = 0;

        public double p_x;
        public double p_y;
        public double p_z;

        public double r_x;
        public double r_y;
        public double r_z;
    }

    public static class JsonHelper
    {
        public static string SerializeObject<T>(this T toSerialize)
        {
            /*JsonConvert.SerializeObject(product);
            ne xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }*/

            return JsonConvert.SerializeObject(toSerialize);
        }

        public static T DeSerializeObject<T>(string jsonstring)
        {
            /*XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader textReader = new StringReader(xmlstring))
            {
                return (T)xmlSerializer.Deserialize(textReader);
            }*/

            return JsonConvert.DeserializeObject<T>(jsonstring);
        }

        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonConvert.DeserializeObject<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonConvert.SerializeObject(wrapper);
        }

        #region ArrayCord

        public static T[,] FromJsonChunk<T>(string json)
        {
            WrapperChunk<T> wrapper = JsonConvert.DeserializeObject<WrapperChunk<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[,] array)
        {
            WrapperChunk<T> wrapper = new WrapperChunk<T>();
            wrapper.Items = array;
            return JsonConvert.SerializeObject(wrapper);
        }
        #endregion

        [System.Serializable]
        public class Wrapper<T>
        {
            public T[] Items;
        }

        [System.Serializable]
        public class WrapperChunk<T>
        {
            public T[,] Items;
        }
    }

    public static class NetConfig
    {
        public static string SecretKey = "secret";
        public static int DefaultOutgoingMessageCapacity = 99999;
        public static int SendBufferSize = 131071;
        public static float ConnectionTimeout = 50;
        public static bool AcceptConnection = true;
        public static bool UseMessageRecycling = true;
        public static string AppIdentifier = Application.AppName + Application.Version;
        /// <summary>
        /// TiketRate is the milliseconds to Thread stop and continue. 15 is Recommended
        /// </summary>
        public static int TickRate = 15;
        /// <summary>
        /// If is true Unat is false, if is false Unat is true.
        /// </summary>
        public static bool DedicatedServer = false;
    }

    namespace CompressString
    {
        internal static class StringCompressor
        {
            /// <summary>
            /// Compresses the string.
            /// </summary>
            /// <param name="text">The text.</param>
            /// <returns></returns>
            public static string CompressString(string text)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                var memoryStream = new MemoryStream();
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gZipStream.Write(buffer, 0, buffer.Length);
                }

                memoryStream.Position = 0;

                var compressedData = new byte[memoryStream.Length];
                memoryStream.Read(compressedData, 0, compressedData.Length);

                var gZipBuffer = new byte[compressedData.Length + 4];
                Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
                return Convert.ToBase64String(gZipBuffer);
            }

            /// <summary>
            /// Decompresses the string.
            /// </summary>
            /// <param name="compressedText">The compressed text.</param>
            /// <returns></returns>
            public static string DecompressString(string compressedText)
            {
                byte[] gZipBuffer = Convert.FromBase64String(compressedText);
                using (var memoryStream = new MemoryStream())
                {
                    int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                    memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                    var buffer = new byte[dataLength];

                    memoryStream.Position = 0;
                    using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        gZipStream.Read(buffer, 0, buffer.Length);
                    }

                    return Encoding.UTF8.GetString(buffer);
                }
            }
        }
    }

    public class RPCALL
    {
        public MethodInfo _function;
        public ParameterInfo[] _parameters;
        public object _obj = null;

        public object Execute(params object[] paramss)
        {
            if (_function == null) return null;

            if (_parameters == null)
            {
                _parameters = _function.GetParameters();
            }

            try
            {
                return (_parameters.Length == 1 && _parameters[0].ParameterType == typeof(object[])) ? _function.Invoke(_obj, new object[] { paramss }) : _function.Invoke(_obj, paramss);
            }
            catch (System.Exception ex)
            {
                if (ex.GetType() == typeof(System.NullReferenceException)) return null;
                Debug.LogException(ex.ToString());
                return null;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class RPC : Attribute
    {

    }

    public struct EntityChannel
    {
        public int ViewId;
        public int ChannelId;

        public EntityChannel(int viewid, int channelid)
        {
            ViewId = viewid;
            ChannelId = channelid;
        }
    }

    public static class NetBit
    {
        /// <summary>
        /// RPC Read Vector3d
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Vector3d ReadVector3(NetIncomingMessage msg)
        {
            Vector3d vec = new Vector3d();

            vec.X = msg.ReadFloat();
            vec.Y = msg.ReadFloat();
            vec.Z = msg.ReadFloat();

            return vec;
        }

        /// <summary>
        /// RPC Read Vector2d
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Vector2d ReadVector2(NetIncomingMessage msg)
        {
            Vector2d vec = new Vector2d();

            vec.X = msg.ReadFloat();
            vec.Y = msg.ReadFloat();

            return vec;
        }

        /// <summary>
        /// RPC Read Quaterniond
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Quaterniond ReadQuaternion(NetIncomingMessage msg)
        {
            Quaterniond vec = new Quaterniond();

            vec.X = msg.ReadFloat();
            vec.Y = msg.ReadFloat();
            vec.Z = msg.ReadFloat();
            vec.W = 1;

            return vec;
        }
    }

    /// <summary>
    /// Sender class to send rpc back to the sender
    /// </summary>
    public struct DNetConnection
    {
        /// <summary>
        /// UniqueId of the sender connection
        /// </summary>
        public long unique;
        /// <summary>
        /// Get your connection to send a menssage to this connection, if you are the server you can't use this
        /// </summary>
        public NetConnection NetConnection;

        public DNetConnection(NetConnection connection)
        {
            NetConnection = connection;
            unique = connection.RemoteUniqueIdentifier;
        }

        /// <summary>
        /// Check if you send this messagen for you, if you is the sender, i gone return true.
        /// </summary>
        public bool IsMine { get { if (unique == Network.MyPeer.UniqueIdentifier) { return true; } else { return false; } } private set { } }
    }

    public enum DataType : byte
    {
        RPC,
        RPC_All,
        RPC_AllOwner,
        RPC_Owner,

        Instantiate,
        DestroyEntity,
        OnConnectData,

        ExitDimension,
        ServerStop,

        RequestServerData,
        ServerDataReceived
    }

    public enum RPCMode
    {
        All, AllNoOwner, AllNoDimension, Owner, Server
    }

    [Serializable]
    public enum EntityType : byte
    {
        none, PlayerEntity
    }
}
