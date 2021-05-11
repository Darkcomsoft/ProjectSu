using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.entity;
using Projectsln.darkcomsoft.src.entity.managers;
using Projectsln.darkcomsoft.src.gui.guisystem.guielements;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using Projectsln.darkcomsoft.src.gui.guisystem.font;
using Projectsln.darkcomsoft.src.client;
using Projectsln.darkcomsoft.src.engine.window;
using Projectsln.darkcomsoft.src.engine;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Projectsln.darkcomsoft.src.world
{
    public class MainMenuWorld : World
    {
        public Buttom m_singlePlayerButtom;
        public Buttom m_multiPlayerButtom;
        public Text m_textTeste;

        public override void Start()
        {
            Debug.Log("MainMenu Started!");

            m_singlePlayerButtom = new Buttom("SinglePlayer", new RectangleF(0, 0, 100, 50), enums.GUIDock.Center, enums.GUIPivot.Center);
            m_singlePlayerButtom.SetTextColor(Color4.Black);
            m_singlePlayerButtom.SetTextAling(TextAling.Center);

            m_singlePlayerButtom.OnClick += OnSingleClick;

            m_multiPlayerButtom = new Buttom("MultiPlayer", new RectangleF(0, -150, 100, 50), enums.GUIDock.Center, enums.GUIPivot.Center);
            m_multiPlayerButtom.SetTextColor(Color4.Black);
            m_multiPlayerButtom.SetTextAling(TextAling.Center);

            m_textTeste = new Text("Text Teste", new RectangleF(20, 20, 100, 20), enums.GUIDock.LeftTop, enums.GUIPivot.LeftTop, 25);

            base.Start();
        }

        protected override void OnDispose()
        {
            Debug.Log("MainMenu Disposed!");

            m_singlePlayerButtom.OnClick -= OnSingleClick;

            m_textTeste?.Dispose();
            m_textTeste = null;

            m_singlePlayerButtom?.Dispose();
            m_singlePlayerButtom = null;

            m_multiPlayerButtom?.Dispose();
            m_multiPlayerButtom = null;
            base.OnDispose();
        }

        private void OnSingleClick()
        {
            Game.instance.StartSinglePlayer();
        }

        protected override void OnTick()
        {
            if (Input.GetKeyDown(Keys.Escape))
            {
                Application.CloseApp();
            }
            base.OnTick();
        }
    }
}
