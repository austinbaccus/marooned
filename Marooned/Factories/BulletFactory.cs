using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Marooned.Sprites;

namespace Marooned.Factories
{
    public static class BulletFactory
    {
        public static ContentManager content;
        public static Bullet MakeBullet(float lifeSpan, Vector2 direction, Vector2 linearVelocity, float damage, Vector2 origin)
        {
            var texture = content.Load<Texture2D>("Sprites/Banana");
            return new Bullet(texture, lifeSpan, direction, linearVelocity, damage, origin);
        }
    }
}
