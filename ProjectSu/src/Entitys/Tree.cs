using OpenTK.Mathematics;
using ProjectSu.src.database;
using ProjectSu.src.Engine;
using ProjectSu.src.Engine.AssetsPipeline;
using ProjectSu.src.Engine.Entitys;
using ProjectSu.src.Engine.PhysicsSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSu.src.Entitys
{
    public class Tree : EEntity
    {
        public int HP, MaxHP;

        private BoxCollider _boxCollider;
        private string treeType;

        public Tree(Vector3d position, TreeType treeType)
        {
            HP = 100;
            MaxHP = 100;

            System.Random rand = new System.Random();

            double a = (double)rand.NextDouble();
            double b = (double)rand.NextDouble();

            double ChunkSeed = position.X * a + position.Z * b;

            transform.Position = position;
            transform.Rotation = new Quaterniond(MathHelper.DegreesToRadians((double)DRandom.Range(-10, 10, (int)ChunkSeed)), MathHelper.DegreesToRadians(DRandom.Range(0, 90, (int)ChunkSeed)), MathHelper.DegreesToRadians((double)DRandom.Range(-10, 10, (int)ChunkSeed)));

            double size = DRandom.Range(1.5f, 2f, (int)ChunkSeed);

            transform.Size = new Vector3d(1, 1, 1);

            _boxCollider = new BoxCollider(transform, new Vector3d(0.7f, 5, 0.7f));

            switch (treeType)
            {
                case TreeType.Oak:
                    this.treeType = "oak";
                    break;
                case TreeType.Pine:
                    this.treeType = "Pine01";
                    break;
                case TreeType.PineSnow:
                    this.treeType = "Pine01";
                    break;
            }
        }

        protected override void OnStart()
        {
            //AddDrawTrans();
            base.OnStart();
        }

        protected override void OnTickDrawTrans()
        {
            //AssetManager.GetModel(treeType).DrawModel(this);
            base.OnTickDrawTrans();
        }

        protected override void OnDispose()
        {
            _boxCollider.Dispose();
            _boxCollider = null;
            base.OnDispose();
        }
    }
}
