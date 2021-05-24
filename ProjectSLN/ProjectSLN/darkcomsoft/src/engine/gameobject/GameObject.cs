using Lidgren.Network;
using ProjectSLN.darkcomsoft.src;
using ProjectSLN.darkcomsoft.src.debug;
using ProjectSLN.darkcomsoft.src.engine;
using ProjectSLN.darkcomsoft.src.engine.gameobject;
using ProjectSLN.darkcomsoft.src.engine.render;
using ProjectSLN.darkcomsoft.src.entity;
using ProjectSLN.darkcomsoft.src.enums;
using ProjectSLN.darkcomsoft.src.network;
using ProjectSLN.darkcomsoft.src.world;
using ProjectSLN.darkcomsoft.src.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSLN.darkcomsoft.src.engine.gameobject
{
    /// <summary>
    /// Class Base for all Game "Objects", is like a entity but used for all type of object, static, dynamic, etc.
    /// </summary>
    public abstract class GameObject : ClassBase
    {
        protected Transform m_transform { get; private set; }
        protected bool m_removed { get; private set; }
        protected bool m_visible { get; private set; }
        protected bool m_usefrustum { get; private set; }

        protected bool m_activated { get; private set; }

        protected World m_world { get; private set; }//world of this entity is in

        public void Create(World world)
        {
            Debug.Log(this.GetType() + " is Created!");

            m_removed = false;
            m_visible = false;
            m_usefrustum = false;

            m_transform = new Transform();
            this.m_world = world;

            OnAwake();

            Enable();
        }

        public void Tick() { if (m_activated && !m_removed) { doTick(); } }
        public void Draw(DrawStage drawStage) { if (m_activated && !m_removed) { doDraw(drawStage); } }

        public void DestroyThis()
        {
            m_removed = true;
            Disable();
        }

        private void doTick()
        {
            m_visible = false;

            if (m_usefrustum)
            {
                DoCheckIfVisible();
            }

            //<start> call tick function anyway if is visible or not, let this to the entity to decide to do what when is visible or not
            OnTick();
            //<end>
        }

        private void doDraw(DrawStage drawStage)
        {
            if (m_usefrustum)
            {
                if (m_visible)
                {
                    OnDraw(drawStage);
                }
            }
            else
            {
                OnDraw(drawStage);
            }
        }

        private void DoCheckIfVisible()
        {
            if (Camera.main == null) { return; }

            //I DONT KNOW BUT THIS FUNCTIONS FRUSTUM NEED SOME OPTIMIZATION, I DON'T KNOW DO A LOOK
            Frustum.CalculateFrustum(Camera.main.GetProjectionMatrix(), transform.GetTransformWorld);

            if (Frustum.VolumeVsFrustum(transform.Position, transform.VolumeSize))
            {
                m_visible = true;
            }
        }

        protected override void OnDispose()
        {
            Disable();

            transform.Dispose();

            m_transform = null;
            m_world = null;
            base.OnDispose();
        }

        /// <summary>
        /// If is true, is gone tick only if is visible by the camera, if is false, dont need to tick if is visible by the camera, true only recomended in static no render entity
        /// </summary>
        /// <param name="value"></param>
        public void UseFrustum(bool value)
        {
            m_usefrustum = value;
        }

        /// <summary>
        /// Enable this GameObject
        /// </summary>
        protected void Enable()
        {
            m_activated = true;
        }

        /// <summary>
        /// Disable GameObject, when is disable GameObject dont do Tick, Draw, all functionalitis etc.
        /// </summary>
        protected void Disable()
        {
            m_activated = false;
        }

        /// <summary>
        /// This is called everyFrame by Client and Server, is called anyway
        /// </summary>
        protected virtual void OnTick() { }

        /// <summary>
        /// A Render tick, used to render openGL stuff
        /// </summary>
        /// <param name="drawStage"></param>
        protected virtual void OnDraw(DrawStage drawStage) { }

        /// <summary>
        ///  Called when entity is ready to use, EX: Entity used this when network is setup and call this
        /// </summary>
        protected virtual void OnStart() { }
        /// <summary>
        /// Called when class instance is created, the <see cref="Entity"/> class call <see cref="OnStart"/> when net is setup, and ready
        /// </summary>
        protected virtual void OnAwake() { }
        /// <summary>
        /// When become visible to a camera, is visible in camera View port
        /// </summary>
        protected virtual void OnBecomeVisible() { }
        /// <summary>
        /// When become invisible to a camera, is not visible in camera View port anymore
        /// </summary>
        protected virtual void OnBecomeInvisible() { }

        protected virtual void OnWorldChanged() { }

        /// <summary>
        /// Move this GameObject to this <paramref name="world"/> World!
        /// </summary>
        /// <param name="world"></param>
        protected void ChangeWorld(World world) { m_world = world; OnWorldChanged(); }

        /// <summary>
        /// "Spawn" a GameObject, if is a Entity spawn on network
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="world"></param>
        public static GameObject SpawnObject<T>(World world)
        {
            GameObject obj = ObjectManager.CreateGameObject<T>(world);

            /*if (obj.GetType().Equals(typeof(Entity)))
            {
                NetworkManager.instance.CreatEntity((Entity)obj, world);
            }*/

            return obj;
        }

        /// <summary>
        /// Destroy a GameObject, if is a Entity Destroy on the network
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="insta"></param>
        public static void DestroyObject(GameObject gameObject, bool insta = false)
        {
            /*if (gameObject.GetType().Equals(typeof(Entity)))
            {
                NetworkManager.instance.DestroyEntity((Entity)gameObject);
            }*/

            ObjectManager.RemoveGameObject(gameObject, insta);
        }

        /// <summary>
        /// Dont use this to Spawn <see cref="SpawnObject{T}(World)"/>, thi is just to add and create a Entity instance to the list, but only the netcode call this
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="world"></param>
        public static GameObject SpawnObject(Type type, World world)
        {
            GameObject obj = ObjectManager.CreateGameObject(type, world);

            /*if (obj.GetType().Equals(typeof(Entity)))
            {
                NetworkManager.instance.CreatEntity((Entity)obj, world);
            }*/

            return obj;
        }

        public Transform transform { get { return m_transform; } }
        public World GetWorld { get { return m_world; } }
        public bool isRemoved { get { return m_removed; } }
        public bool isVisible { get { return m_visible; } }
    }
}