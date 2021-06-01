using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Network;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.misc;
using ProjectIND.darkcomsoft.src.entity;
using System.Reflection;
using ProjectIND.darkcomsoft.src.world;
using ProjectIND.darkcomsoft.src.entity.managers;
using ProjectIND.darkcomsoft.src.engine.gameobject;
using ProjectIND.darkcomsoft.src.engine.gameobject;

namespace ProjectIND.darkcomsoft.src.network
{
    [Obsolete("FUCK THE MULTIPLAYER FOR NOW")]
    public class NetworkManager : ClassBase
    {
        /// <summary>
        /// This i the default NetDeliveryMethod, used for the netcode systems, all entitys have ther one NetDeliveryMethod
        /// </summary>
        public const NetDeliveryMethod NetBaseDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
        private Dictionary<int, Entity> m_netViewEntityList = new Dictionary<int, Entity>();

        private static NetworkManager m_instance;
        private NetworkType m_netType;
        private NetworkBase m_network;

        private NetworkCallBacks m_networkCallBacks;

        public NetworkManager()
        {
            m_instance = this;

            m_networkCallBacks = new NetworkCallBacks();
        }

        public static void CreateServer(long ip, int port, int maxplayers)
        {
            instance.doCreateServer(ip, port, maxplayers);
        }

        /// <summary>
        /// Connect Using Numeric ip
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="password"></param>
        public static void Connect(int ip, int port)
        {
            instance.doConnectClient(ip.ToString(), port);
        }

        /// <summary>
        /// Connect Using Web URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="port"></param>
        /// <param name="password"></param>
        public static void Connect(string url, int port)
        {
            instance.doConnectClient(Utilits.GetipfromURL(url), port);
        }

        public static void Disconnect()
        {
            instance.doDisconnect();
        }

        /// <summary>
        /// THIS IS ONLY FOR THE GAME-ENGINE TO CALL, Use This <see cref="GameObject.SpawnObject{T}(World)"/> instead!
        /// </summary>
        /// <typeparam name="T">Type of the entity you want spawn</typeparam>
        /// <param name="world">world you want to spawn a entity</param>
        /// <returns></returns>
        public void CreatEntity(Entity entity,World world)
        {
            if (!IsRuning) { throw new Exception("You can't spawn a entity when you are disconnected or when server is not runing"); }

            instance.m_network.Spawn(entity);
        }

        /// <summary>
        /// THIS IS ONLY FOR THE GAME-ENGINE TO CALL, Use This <see cref="GameObject.DestroyObject(GameObject, bool)"/> instead!
        /// </summary>
        /// <param name="gameobject"></param>
        public void DestroyEntity(Entity gameobject)
        {
            if (!IsRuning) { throw new Exception("You can't destroy a entity when you are disconnected or when server is not runing"); }

            instance.m_network.Destroy(gameobject);
        }

        public void Tick()
        {
            if (m_network != null)
            {
                m_network.Tick();
            }
        }

        private void doDisconnect()
        {
            m_netType = NetworkType.none;

            if (m_network != null)
            {
                m_network.Dispose();
                m_network = null;
            }
        }

        private void doCreateServer(long ip, int port, int maxplayers)
        {
            if (m_netType != NetworkType.none) { Debug.Log("Can't create a server, Is allready runing a network instance: " + m_netType, "NETWORK");  return; }

            m_netType = NetworkType.Server;
            m_network = new NetworkServer(ip, port, maxplayers);
        }

        private void doConnectClient(string ip, int port)
        {
            if (m_netType != NetworkType.none) { Debug.Log("Can't connect Is allready runing a network instance: " + m_netType, "NETWORK"); return; }

            m_netType = NetworkType.Client;
            m_network = new NetworkClient(ip, port);
        }

        protected override void OnDispose()
        {
            Disconnect();//Call disconnect any way if the network is finished.

            m_netViewEntityList.Clear();

            m_networkCallBacks.Dispose();
            m_networkCallBacks = null;

            m_netViewEntityList = null;
            m_instance = null;
            base.OnDispose();
        }

        #region NetUtilits
        public static void AddEntityNet(Entity entity)
        {
            instance.m_netViewEntityList.Add(entity.getViewId, entity);
        }

        public static void RemoveEntityNet(Entity entity)
        {
            instance.m_netViewEntityList.Remove(entity.getViewId);
        }

        /// <summary>
        /// Check if the Specified Id is my
        /// </summary>
        /// <param name="uniqId"></param>
        /// <returns></returns>
        public static bool IsMine(long uniqId)
        {
            if (uniqId.Equals(instance.m_network.getPeer.UniqueIdentifier))
            {
                return true;
            }
            return false;
        }

        public static NetConnection GetMyConnection()
        {
            foreach (var item in instance.m_network.getPeer.Connections)
            {
                if (item.RemoteUniqueIdentifier == instance.m_network.getPeer.UniqueIdentifier)
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
                if (instance.m_network.getPeer.Connections.Count > 0)
                {
                    try
                    {
                        retval = instance.m_network.getPeer.Connections[0];
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
        #endregion

        public static NetworkManager instance { get { return m_instance; } }
        public static NetworkType getStatus { get { return instance.m_netType; } }
        public static bool IsRuning { get { if (instance.m_netType != NetworkType.none) { return true; } return false; } }
        /// <summary>
        /// If is server return True, if is Client return False
        /// </summary>
        //public static bool IsServer { get { if (instance.m_netType == NetworkType.Server) { return true; } return false; } }

        public Dictionary<int, Entity> getNetViewEntityList { get { return m_netViewEntityList; } }
    }

    public enum NetworkType : byte
    {
        none, Server, Client
    }

    public enum NetDataType : byte
    {
        RPC, //Remote Procedural Call
        RPC_All, //RPC from client asking to send some data to everyone
        RPC_AllOwner, //RPC from client asking to send some data to everyone but no the owner
        RPC_Owner, //RPC from client asking to send some data to the owner
        Spawn, //Spawn a entity
        Destroy, //Destroy a entity
        ConnectData, //Send all data when connect to server, EX:spawn all entitys
        RequestSpawn, //Client send a ask to server to Spawn a entity
        RequestDestroy, //Client send a ask to server to Destroy a entity

    }

    public struct RPCALL
    {
        public MethodInfo _function;
        public ParameterInfo[] _parameters;
        public object _obj;

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

    [Serializable]
    public class NetViewSerializer
    {
        public string WorldType;
        public string EntityType;

        public int RegionID = 0;
        public long Owner;
        public int ViewID = 0;

        public double p_x;
        public double p_y;
        public double p_z;

        public double r_x;
        public double r_y;
        public double r_z;
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class RPC : Attribute {  }

    public static class NetConfig
    {
        /// <summary>
        /// Is a string used for check if the client can connect with the server, if the server key or client is not equal dont connect, Ex:GameName + GameVersion + Plataform
        /// </summary>
        public static string SecretKey = "your app identifier";
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
}
