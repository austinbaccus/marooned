using Marooned.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marooned
{
    public static class SpriteFactory
    {
        public static ContentManager content;

        public static Bullet GenerateBullet(float lifeSpan, Vector2 direction, Vector2 linearVelocity, float damage, Vector2 origin)
        {
            var texture = content.Load<Texture2D>("Sprites/Banana");
            return new Bullet(texture, lifeSpan, direction, linearVelocity, damage, origin);
        }

        public static Bullet GenerateEnemyBullet(float lifeSpan, Vector2 direction, Vector2 linearVelocity, float damage, Vector2 origin)
        {
            var texture = content.Load<Texture2D>("Sprites/FireBall");
            return new Bullet(texture, lifeSpan, direction, linearVelocity, damage, origin);
        }
    }
}
