using ProjectSu.src.Engine.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Entitys
{
    /// <summary>
    /// A Livin Entity, using the class NEntity, Ex:player, Npc, etc. all living entitys
    /// </summary>
    public class Entity : NEntity
    {
        private float HP = 100;
        private float MaxHP = 100;

        private bool isAlive = true;

        protected override void OnStart()
        {
            AddTick();
            base.OnStart();
        }

        protected override void NetStart()
        {
            isAlive = true;
            HP = MaxHP;
            base.NetStart();
        }

        public void DoDamage(float damage)
        {
            HP -= damage;

            if (HP <= 0)
            {
                OnDead();
                isAlive = false;
            }
        }

        protected virtual void OnDead()
        {

        }

        public bool Alive { get { return isAlive; } }
    }
}
