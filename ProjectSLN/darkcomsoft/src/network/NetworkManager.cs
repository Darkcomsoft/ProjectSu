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

namespace ProjectIND.darkcomsoft.src.network
{
    [Obsolete("FUCK THE MULTIPLAYER FOR NOW")]
    public class NetworkManager : ClassBase
    {
        /// <summary>
        /// This i the default NetDeliveryMethod, used for the netcode systems, all entitys have ther one NetDeliveryMethod
        /// </summary>
        public const NetDeliveryMethod NetBaseDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
        private Dictionary<int, Entity> v_netViewEntityList = new Dictionary<int, Entity>();

        private static NetworkManager v_instance;
        private NetworkType v_netType;
        private NetworkBase v_network;

        private NetworkCallBacks v_networkCallBacks;

        public NetworkManager()
        {
            v_instance = this;

            v_networkCallBacks = new NetworkCallBacks();
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

            instance.v_network.Spawn(entity);
        }

        /// <summary>
        /// THIS IS ONLY FOR THE GAME-ENGINE TO CALL, Use This <see cref="GameObject.DestroyObject(GameObject, bool)"/> instead!
        /// </summary>
        /// <param name="gameobject"></param>
        public void DestroyEntity(Entity gameobject)
        {
            if (!IsRuning) { throw new Exception("You can't destroy a entity when you are disconnected or when server is not runing"); }

            instance.v_network.Destroy(gameobject);
        }

        public void Tick()
        {
            if (v_network != null)
            {
                v_network.Tick();
            }
        }

        private void doDisconnect()
        {
            v_netType = NetworkType.none;

            if (v_network != null)
            {
                v_network.Dispose();
                v_network = null;
            }
        }

        private void doCreateServer(long ip, int port, int maxplayers)
        {
            if (v_netType != NetworkType.none) { Debug.Log("Can't create a server, Is allready runing a network instance: " + v_netType, "NETWORK");  return; }

            v_netType = NetworkType.Server;
            v_network = new NetworkServer(ip, port, maxplayers);
        }

        private void doConnectClient(string ip, int port)
        {
            if (v_netType != NetworkType.none) { Debug.Log("Can't connect Is allready runing a network instance: " + v_netType, "NETWORK"); return; }

            v_netType = NetworkType.Client;
            v_network = new NetworkClient(ip, port);
        }

        protected override void OnDispose()
        {
            Disconnect();//Call disconnect any way if the network is finished.

            v_netViewEntityList.Clear();

            v_networkCallBacks.Dispose();
            v_networkCallBacks = null;

            v_netViewEntityList = null;
            v_instance = null;
            base.OnDispose();
        }

        #region NetUtilits
        public static void AddEntityNet(Entity entity)
        {
            instance.v_netViewEntityList.Add(entity.getViewId, entity);
        }

        public static void RemoveEntityNet(Entity entity)
        {
            instance.v_netViewEntityList.Remove(entity.getViewId);
        }

        /// <summary>
        /// Check if the Specified Id is my
        /// </summary>
        /// <param name="uniqId"></param>
        /// <returns></returns>
        public static bool IsMine(long uniqId)
        {
            if (uniqId.Equals(instance.v_network.getPeer.UniqueIdentifier))
            {
                return true;
            }
            return false;
        }

        public static NetConnection GetMyConnection()
        {
            foreach (var item in instance.v_network.getPeer.Connections)
            {
                if (item.RemoteUniqueIdentifier == instance.v_network.getPeer.UniqueIdentifier)
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
                if (instance.v_network.getPeer.Connections.Count > 0)
                {
                    try
                    {
                        retval = instance.v_network.getPeer.Connections[0];
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

        public static NetworkManager instance { get { return v_instance; } }
        public static NetworkType getStatus { get { return instance.v_netType; } }
        public static bool IsRuning { get { if (instance.v_netType != NetworkType.none) { return true; } return false; } }
        /// <summary>
        /// If is server return True, if is Client return False
        /// </summary>
        //public static bool IsServer { get { if (instance.v_netType == NetworkType.Server) { return true; } return false; } }

        public Dictionary<int, Entity> getNetViewEntityList { get { return v_netViewEntityList; } }
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
