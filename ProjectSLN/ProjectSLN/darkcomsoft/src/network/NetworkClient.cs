using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lidgren.Network;
using OpenTK.Mathematics;
using ProjectSLN.darkcomsoft.src.debug;
using ProjectSLN.darkcomsoft.src.engine.gameobject;
using ProjectSLN.darkcomsoft.src.entity;
using ProjectSLN.darkcomsoft.src.entity.managers;
using ProjectSLN.darkcomsoft.src.misc;
using ProjectSLN.darkcomsoft.src.world;
using ProjectSLN.darkcomsoft.src.engine.gameobject;

namespace ProjectSLN.darkcomsoft.src.network
{
    public class NetworkClient : NetworkBase
    {
        public NetworkClient(string ip, int port)
        {
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

            NetworkCallBacks.OnClientStart?.Invoke();
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
                            NetworkCallBacks.OnPlayerDisconnect?.Invoke(inc.SenderConnection);
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
                            NetworkCallBacks.OnConnect?.Invoke();
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
                                NetworkCallBacks.OnPlayerDisconnect?.Invoke(inc.SenderConnection);
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
                    ReadSpawnData(inc);
                    break;
                case NetDataType.Destroy:
                    ReadDestroyData(inc);
                    break;
                case NetDataType.ConnectData:
                    ReadConnectData(inc);
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

            PeerClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);

            entity.SetupEntityNetcode(viewid, m_peer.UniqueIdentifier);
            base.Spawn(entity);
        }

        public override void Destroy(Entity entity)
        {
            var msg = m_peer.CreateMessage();

            msg.Write((byte)NetDataType.Destroy);
            msg.Write(entity.getViewId);

            PeerClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
            base.Destroy(entity);
        }

        private void Disconnect()
        {
            PeerClient.Disconnect("Client Requested to Disconnect!");
        }

        protected override void OnDispose()
        {
            Disconnect();
            NetworkCallBacks.OnDisconnect?.Invoke();
            base.OnDispose();
        }

        #region ReadReceviveDatas
        private void ReadSpawnData(NetIncomingMessage inc)
        {
            var typeName = inc.ReadString();//Entity Type
            var worldType = inc.ReadString();//Curret Entity World

            var viewId = inc.ReadInt32();
            var regionId = inc.ReadInt32();//Current region id
            var ownerId = inc.ReadVariableInt64();//Netcode ID

            Vector3d position = new Vector3d(inc.ReadDouble(), inc.ReadDouble(), inc.ReadDouble());
            Quaterniond rotation = new Quaterniond(inc.ReadDouble(), inc.ReadDouble(), inc.ReadDouble(), Quaterniond.Identity.W);

            if (ObjectManager.ContainsEntity(NetworkManager.instance.getNetViewEntityList[viewId])) { return; }//Check if the entity is allready in the list

            Entity entityBase = (Entity)GameObject.SpawnObject(Type.GetType(typeName), WorldManager.GetWorld(Type.GetType(worldType)));
            entityBase.SetupEntityNetcode(viewId, ownerId);
            entityBase.transform.Position = position;
            entityBase.transform.Rotation = rotation;
        }

        private void ReadDestroyData(NetIncomingMessage inc)
        {
            var viewId = inc.ReadInt32();

            if (!ObjectManager.ContainsEntity(NetworkManager.instance.getNetViewEntityList[viewId])) { return; }//Check if the entity is in the List

            GameObject.DestroyObject(NetworkManager.instance.getNetViewEntityList[viewId]);//Destroy the entity in engine
        }

        private void ReadConnectData(NetIncomingMessage inc)
        {
            int byteNumber = inc.ReadInt32();
            byte[] compressed = inc.ReadBytes(byteNumber);

            string json = DataCompressor.DecompressByteToString(compressed);

            NetViewSerializer[] entitylist = JsonHelper.FromJson<NetViewSerializer>(json);

            foreach (var kvp in entitylist)
            {
                Debug.Log("EntityReceived: " + kvp.ViewID);
                Entity entityBase = (Entity)GameObject.SpawnObject(Type.GetType(kvp.EntityType), WorldManager.GetWorld(Type.GetType(kvp.WorldType)));
                entityBase.SetupEntityNetcode(kvp.ViewID, kvp.Owner);
                entityBase.transform.Position = new Vector3d(kvp.p_x, kvp.p_y, kvp.p_z);
                entityBase.transform.Rotation = new Quaterniond(kvp.r_x, kvp.r_y, kvp.r_z, Quaterniond.Identity.W);
            }

            NetworkCallBacks.OnReceivedServerData?.Invoke();
        }
        #endregion
    }
}