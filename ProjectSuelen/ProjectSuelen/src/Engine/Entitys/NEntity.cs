using Lidgren.Network;
using OpenTK;
using ProjectSuelen.src.Engine.NetCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine.Entitys
{
    /// <summary>
    /// A dynamic entity, use network, so is syncronized with the others player. EX:Player, NPC, Projectil, Son, GameManager etc.
    /// </summary>
    public class NEntity : GameObject
    {
        public int _currentChannelID = 0;//This is the world id, like the normal world, all regions of chunks have ther owne channel id.

        private bool _IsEntityReady = false;
        public long _Owner;
        public int _ViewID = 0;
        public NetDeliveryMethod _DefaultNetDeliveryMethod = NetDeliveryMethod.Unreliable;

        [NonSerialized]
        internal Dictionary<string, RPCALL> _methodlist = new Dictionary<string, RPCALL>();

        protected override void OnCreate()
        {
            GetMethodList();
            _IsEntityReady = true;
            base.OnCreate();
        }

        protected virtual void NetStart()
        {
            
        }

        protected override void OnDestroy()
        {
            _methodlist.Clear();
            _methodlist = null;

            _IsEntityReady = false;
            Network._NetviewList.Remove(_ViewID);
            base.OnDestroy();
        }

        /// <summary>
        /// This is for seting up the entity in networking, only use in network class
        /// </summary>
        public void SetUpNet(int id, long owner, NetDeliveryMethod netDeliveryMethod)
        {
            _ViewID = id;
            _Owner = owner;
            _DefaultNetDeliveryMethod = netDeliveryMethod;

            _IsEntityReady = true;


            Network._NetviewList.Add(id, this);
            NetStart();
        }

        /// <summary>
        /// Send RPC with RPCmode and default NetDeliveryMethod
        /// </summary>
        /// <param name="funcname"></param>
        /// <param name="Mode"></param>
        /// <param name="param"></param>
        public void RPC(string funcname, RPCMode Mode, params object[] param)
        {
            if (Network.Ready)
            {
                SendRPC(funcname, Mode, _DefaultNetDeliveryMethod, param);
            }
        }

        /// <summary>
        /// Send RPC to Specifique User(NetPeer), with a default NetDeliveryMethod
        /// </summary>
        /// <param name="funcname"></param>
        /// <param name="player"></param>
        /// <param name="param"></param>
        public void RPC(string funcname, NetConnection player, params object[] param)
        {
            if (Network.Ready)
            {
                SendRPC(funcname, player, _DefaultNetDeliveryMethod, param);
            }
        }

        #region RPC
        private void SendRPC(string funcname, RPCMode Mode, NetDeliveryMethod DeliveryMethod, params object[] param)
        {
            var om = Network.MyPeer.CreateMessage();

            switch (Mode)
            {
                case RPCMode.All:
                    om.Write((byte)DataType.RPC_All);

                    om.Write(funcname);
                    om.Write(_ViewID);
                    //om.Write(Dimension);//Deisabled for now

                    DoData(om, param);

                    if (Network.IsClient)
                    {
                        Network.Client.SendMessage(om, DeliveryMethod);
                    }
                    else
                    {
                        Network.RPC_All(funcname, _ViewID, param);
                    }
                    break;
                case RPCMode.AllNoOwner:
                    om.Write((byte)DataType.RPC_AllOwner);

                    om.Write(funcname);
                    om.Write(_ViewID);
                    //om.Write(Dimension);

                    DoData(om, param);

                    if (Network.IsClient)
                    {
                        Network.Client.SendMessage(om, DeliveryMethod);
                    }
                    else
                    {
                        Network.RPC_AllOwner(funcname, _ViewID, param);
                    }
                    break;
                case RPCMode.Owner:
                    om.Write((byte)DataType.RPC_Owner);

                    om.Write(funcname);
                    om.Write(_ViewID);

                    DoData(om, param);

                    if (Network.IsClient)
                    {
                        Network.Client.SendMessage(om, DeliveryMethod);
                    }
                    else
                    {
                        Network.RPC_Owner(funcname, _ViewID, param);
                    }
                    break;
                case RPCMode.Server:
                    om.Write((byte)DataType.RPC);

                    om.Write(funcname);
                    om.Write(_ViewID);

                    DoData(om, param);

                    if (Network.IsClient == true)
                    {
                        Network.Client.SendMessage(om, DeliveryMethod);
                    }
                    else
                    {
                        Network.RPC_Server(funcname, _ViewID, param);
                    }
                    break;
            }
        }

        private void SendRPC(string funcname, NetConnection player, NetDeliveryMethod DeliveryMethod, params object[] param)
        {
            var om = Network.MyPeer.CreateMessage();

            om.Write((byte)DataType.RPC);

            om.Write(funcname);
            om.Write(_ViewID);

            DoData(om, param);

            Network.MyPeer.SendMessage(om, player, DeliveryMethod);
            return;
        }

        private void DoData(NetOutgoingMessage om, object[] param)
        {
            foreach (var item in param)
            {
                Type type = item.GetType();

                if (type == typeof(string))
                {
                    om.Write((string)item);
                }
                else if (type == typeof(int))
                {
                    om.Write((int)item);
                }
                else if (type == typeof(bool))
                {
                    om.Write((bool)item);
                }
                else if (type == typeof(double))
                {
                    om.Write((double)item);
                }
                else if (type == typeof(Vector3d))
                {
                    Vector3d vec = (Vector3d)item;

                    om.Write(vec.X);
                    om.Write(vec.Y);
                    om.Write(vec.Z);
                }
                else if (type == typeof(Vector2d))
                {
                    Vector2d vec = (Vector2d)item;

                    om.Write(vec.X);
                    om.Write(vec.Y);
                }
                else if (type == typeof(Quaterniond))
                {
                    Quaterniond vec = (Quaterniond)item;

                    om.Write(vec.X);
                    om.Write(vec.Y);
                    om.Write(vec.Z);
                }
            }
        }

        public object[] Execute(string funcName, NetIncomingMessage msg)
        {
            RPCALL ent;

            if (_methodlist.TryGetValue(funcName, out ent))
            {
                if (ent._parameters == null)
                    ent._parameters = ent._function.GetParameters();

                try
                {
                    List<object> objects = new List<object>();

                    for (int i = 0; i < ent._parameters.Length; i++)
                    {
                        objects.Add(ReadArgument(msg, ent._parameters[i].ParameterType));
                    }

                    ent._function.Invoke(ent._obj, objects.ToArray());
                    return objects.ToArray();
                }
                catch (System.Exception ex)
                {
                    if (ex.GetType() == typeof(System.NullReferenceException)) return null;
                    Debug.LogException(ex.ToString());
                    return null;
                }
            }
            return null;
        }

        public object[] Execute(string funcName, object[] param)
        {
            RPCALL ent;

            if (_methodlist.TryGetValue(funcName, out ent))
            {
                if (ent._parameters == null)
                    ent._parameters = ent._function.GetParameters();

                try
                {
                    List<object> objects = new List<object>();

                    foreach (var item in ent._parameters)
                    {
                        if (item.ParameterType == typeof(DNetConnection))
                        {
                            DNetConnection dnet = new DNetConnection();
                            dnet.unique = Network.MyPeer.UniqueIdentifier;
                            objects.Add(dnet);
                        }
                        else
                        {
                            objects.Add(item);
                        }
                    }

                    ent._function.Invoke(ent._obj, objects.ToArray());
                    return objects.ToArray();
                }
                catch (System.Exception ex)
                {
                    if (ex.GetType() == typeof(System.NullReferenceException)) return null;
                    Debug.LogException(ex.ToString());
                    return null;
                }
            }
            return null;
        }

        private void GetMethodList()
        {
            _methodlist.Clear();

            System.Type type = this.GetType();

            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            for (int i = 0; i < methods.Length; ++i)
            {
                MethodInfo method = methods[i];

                if (method.IsDefined(typeof(RPC), true))
                {
                    RPCALL ent = new RPCALL();
                    ent._function = method;
                    ent._obj = this;

                    RPC tnc = (RPC)ent._function.GetCustomAttributes(typeof(RPC), true)[0];

                    _methodlist[method.Name] = ent;
                }
            }
        }

        private static object ReadArgument(NetIncomingMessage msg, Type type)
        {
            if (type == typeof(int))
            {
                return msg.ReadInt32();
            }
            else if (type == typeof(byte))
            {
                return msg.ReadByte();
            }
            else if (type == typeof(bool))
            {
                return msg.ReadBoolean();
            }
            else if (type == typeof(double))
            {
                return msg.ReadFloat();
            }
            else if (type == typeof(Vector3d))
            {
                return NetBit.ReadVector3(msg);
            }
            else if (type == typeof(Vector2d))
            {
                return NetBit.ReadVector2(msg);
            }
            else if (type == typeof(Quaterniond))
            {
                return NetBit.ReadQuaternion(msg);
            }
            else if (type == typeof(DNetConnection))
            {
                return new DNetConnection(msg.SenderConnection);
            }
            else if (type == typeof(string))
            {
                return msg.ReadString();
            }
            else
            {
                throw new Exception("Unsupported argument type " + type);
            }
        }

        #endregion

        public bool isMine
        {
            get
            {
                if (Network.Runing)
                {
                    if (_Owner == Network.MyPeer.UniqueIdentifier)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public bool isReady { get { return _IsEntityReady; } }
    }
}