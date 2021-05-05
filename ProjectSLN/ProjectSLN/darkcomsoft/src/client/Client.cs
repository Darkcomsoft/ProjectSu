using OpenTK.Mathematics;
using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.entity;
using Projectsln.darkcomsoft.src.entity.managers;
using Projectsln.darkcomsoft.src.world;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;
using Projectsln.darkcomsoft.src.gui.guisystem;
using OpenTK.Windowing.Common;
using Projectsln.darkcomsoft.src.network;
using Lidgren.Network;

namespace Projectsln.darkcomsoft.src.client
{
    /// <summary>
    /// is the client, this is the class start the game stuff, like graphics, and other things
    /// </summary>
    public class Client : BuildTypeBase
    {
        public static Client instance { get; private set; }

        private Game m_clientManager;

        public static Input m_input { get; private set; }
        public static Gizmo m_gizmos { get; private set; }
        public static GUI m_gui { get; private set; }

        private bool m_restartGame = false;

        public Client()
        {
            instance = this;

            m_gizmos = new Gizmo();//This is only for debug
            m_input = new Input();
            m_gui = new GUI();

            m_clientManager = new Game();
        }

        public override void Tick()
        {
            if (m_restartGame)
            {
                if (m_clientManager != null)
                {
                    m_clientManager.Dispose();
                    m_clientManager = null;

                    m_clientManager = new Game();
                }

                m_restartGame = false;
            }

            /*if (Input.GetKeyDown(Keys.P))
            {
                if (!CursorManager.isLocked)
                {
                    CursorManager.Lock();
                }
                else
                {
                    CursorManager.UnLock();
                }
            }*/

            if (Input.GetKeyDown(GameSettings.DEBUGSCREEN_KEY))
            {
                if (Debug.isDebugEnabled)
                {
                    Debug.DisableDebug();
                }
                else
                {
                    Debug.EnableDebug();
                }
            }

            m_gui?.Tick();
            base.Tick();
        }

        public override void TickDraw()
        {
            //DrawStuuff Like 3d

            //DrawGUI
            m_gui?.Draw();
            base.TickDraw();
        }

        protected override void OnDispose()
        {
            if (m_clientManager != null)
            {
                m_clientManager.Dispose();
                m_clientManager = null;
            }

            m_gui?.Dispose();
            m_gui = null;

            m_gizmos?.Dispose();
            m_gizmos = null;

            m_input?.Dispose();
            m_input = null;

            instance = null;
            base.OnDispose();
        }

        public override void OnResize()
        {
            m_gui?.OnResize();
            base.OnResize();
        }

        public override void OnMouseMove()
        {
            m_gui?.OnMouseMove();
            base.OnMouseMove();
        }

        public override void OnMouseDown(MouseButtonEventArgs e)
        {
            m_gui?.OnMousePress(e);
            base.OnMouseDown(e);
        }

        public override void OnMouseUp(MouseButtonEventArgs e)
        {
            m_gui?.OnMouseRelease(e);
            base.OnMouseUp(e);
        }

        public void RestartGame()
        {
            m_restartGame = true;
        }
    }
}
