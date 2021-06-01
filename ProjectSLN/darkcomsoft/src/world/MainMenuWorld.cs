using ProjectSLN.darkcomsoft.src.debug;
using ProjectSLN.darkcomsoft.src.entity;
using ProjectSLN.darkcomsoft.src.entity.managers;
using ProjectSLN.darkcomsoft.src.gui.guisystem.guielements;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using ProjectSLN.darkcomsoft.src.gui.guisystem.font;
using ProjectSLN.darkcomsoft.src.client;
using ProjectSLN.darkcomsoft.src.engine.window;
using ProjectSLN.darkcomsoft.src.engine;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ProjectSLN.darkcomsoft.src.world
{
    public class MainMenuWorld : World
    {
        public Buttom m_singlePlayerButtom;
        public Text m_textTeste;

        public override void Start()
        {
            Debug.Log("MainMenu Started!");

            m_singlePlayerButtom = new Buttom("SinglePlayer", new RectangleF(0, 0, 100, 50), enums.GUIDock.Center, enums.GUIPivot.Center);
            m_singlePlayerButtom.SetTextColor(Color4.Black);
            m_singlePlayerButtom.SetTextAling(TextAling.Center);

            m_singlePlayerButtom.OnClick += OnSingleClick;

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
