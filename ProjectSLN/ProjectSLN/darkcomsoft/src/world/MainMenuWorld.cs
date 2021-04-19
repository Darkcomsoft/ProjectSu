using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.entity;
using Projectsln.darkcomsoft.src.entity.managers;
using Projectsln.darkcomsoft.src.gui.guisystem.guielements;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.world
{
    public class MainMenuWorld : World
    {
        public Buttom m_singlePlayerButtom;
        public Text m_textTeste;

        public override void Start()
        {
            Debug.Log("MainMenu Started!");

            m_singlePlayerButtom = new Buttom(new RectangleF(0,0,100,50), enums.GUIDock.Bottom, enums.GUIPivot.Bottom);
            m_textTeste = new Text(new RectangleF(0, 0, 100, 50), enums.GUIDock.Top, enums.GUIPivot.Top);

            base.Start();
        }

        protected override void OnDispose()
        {
            Debug.Log("MainMenu Disposed!");

            m_textTeste?.Dispose();
            m_textTeste = null;

            m_singlePlayerButtom?.Dispose();
            m_singlePlayerButtom = null;
            base.OnDispose();
        }
    }
}
