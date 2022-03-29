using Marooned.Factories;
using Marooned.Sprites;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marooned.Actions
{
    public class ShootAction : IAction
    {
        private List<Bullet> _bulletList;

        public ShootAction(List<Bullet> bulletList)
        {
            _bulletList = bulletList;
        }

        public void Execute(GameTime gameTime)
        {
            _bulletList.Add(BulletFactory.MakeBullet(LifeSpan, Velocity, Damage, Origin));
        }

        public float LifeSpan { get; set; }
        public Vector2 Velocity { get; set; }
        public float Damage { get; set; }
        public Vector2 Origin { get; set; }
    }
}
