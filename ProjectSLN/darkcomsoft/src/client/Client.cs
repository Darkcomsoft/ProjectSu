using OpenTK.Mathematics;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.engine;
using ProjectIND.darkcomsoft.src.entity;
using ProjectIND.darkcomsoft.src.entity.managers;
using ProjectIND.darkcomsoft.src.world;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;
using ProjectIND.darkcomsoft.src.gui.guisystem;
using OpenTK.Windowing.Common;
using ProjectIND.darkcomsoft.src.network;
using Lidgren.Network;

namespace ProjectIND.darkcomsoft.src.client
{
    /// <summary>
    /// is the client, this is the class start the game stuff, like graphics, and other things
    /// </summary>
    public class Client : BuildTypeBase
    {
        public static Client instance { get; private set; }

        private Game v_clientManager;

        public static Input v_input { get; private set; }
        public static Gizmo v_gizmos { get; private set; }
        public static GUI v_gui { get; private set; }

        private bool v_restartGame = false;

        public Client()
        {
            instance = this;

            v_gizmos = new Gizmo();//This is only for debug
            v_input = new Input();
            v_gui = new GUI();

            v_clientManager = new Game();
        }

        public override void Tick()
        {
            if (v_restartGame)
            {
                if (v_clientManager != null)
                {
                    v_clientManager.Dispose();
                    v_clientManager = null;

                    v_clientManager = new Game();
                }

                v_restartGame = false;
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

            v_gui?.Tick();
            base.Tick();
        }

        public override void TickDraw()
        {
            //DrawStuuff Like 3d

            //DrawGUI
            v_gui?.Draw();
            base.TickDraw();
        }

        protected override void OnDispose()
        {
            if (v_clientManager != null)
            {
                v_clientManager.Dispose();
                v_clientManager = null;
            }

            v_gui?.Dispose();
            v_gui = null;

            v_gizmos?.Dispose();
            v_gizmos = null;

            v_input?.Dispose();
            v_input = null;

            instance = null;
            base.OnDispose();
        }

        public override void OnResize()
        {
            v_gui?.OnResize();
            base.OnResize();
        }

        public override void OnMouseMove()
        {
            v_gui?.OnMouseMove();
            base.OnMouseMove();
        }

        public override void OnMouseDown(MouseButtonEventArgs e)
        {
            v_gui?.OnMousePress(e);
            base.OnMouseDown(e);
        }

        public override void OnMouseUp(MouseButtonEventArgs e)
        {
            v_gui?.OnMouseRelease(e);
            base.OnMouseUp(e);
        }

        public void RestartGame()
        {
            v_restartGame = true;
        }
    }
}
