using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.entity;
using ProjectIND.darkcomsoft.src.entity.managers;
using ProjectIND.darkcomsoft.src.gui.guisystem.guielements;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using ProjectIND.darkcomsoft.src.gui.guisystem.font;
using ProjectIND.darkcomsoft.src.client;
using ProjectIND.darkcomsoft.src.engine.window;
using ProjectIND.darkcomsoft.src.engine;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ProjectIND.darkcomsoft.src.world
{
    public class MainMenuWorld : World
    {
        public Buttom v_singlePlayerButtom;
        public Text v_textTeste;

        public override void Start()
        {
            Debug.Log("MainMenu Started!");

            v_singlePlayerButtom = new Buttom("SinglePlayer", new RectangleF(0, 0, 100, 50), enums.GUIDock.Center, enums.GUIPivot.Center);
            v_singlePlayerButtom.SetTextColor(Color4.Black);
            v_singlePlayerButtom.SetTextAling(TextAling.Center);

            v_singlePlayerButtom.OnClick += OnSingleClick;

            v_textTeste = new Text("Text Teste", new RectangleF(20, 20, 100, 20), enums.GUIDock.LeftTop, enums.GUIPivot.LeftTop, 25);

            base.Start();
        }

        protected override void OnDispose()
        {
            Debug.Log("MainMenu Disposed!");

            v_singlePlayerButtom.OnClick -= OnSingleClick;

            v_textTeste?.Dispose();
            v_textTeste = null;

            v_singlePlayerButtom?.Dispose();
            v_singlePlayerButtom = null;

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
