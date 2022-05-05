using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Marooned.Sprites;

namespace Marooned.Factories
{
    public static class BulletFactory
    {
        // TODO: Come up with a better, non-hardcoded way to instantiate source rectangles.
        private static readonly Rectangle[] _animSources = {
            new Rectangle(18, 18, 16, 20),
            new Rectangle(33, 19, 20, 18),
            new Rectangle(54, 19, 16, 19),
            new Rectangle(70, 20, 19, 17),
        };

        // TODO: Make Bullets from an object pool for efficiency
        public static Bullet MakeBullet(GameContext gameContext, float lifeSpan, Vector2 linearVelocity, float damage, Vector2 origin)
        {
            var texture = gameContext.Content.Load<Texture2D>("Sprites/Banana");
            Bullet newBullet = new Bullet(texture, _animSources, lifeSpan, linearVelocity, damage, origin)
            {
#if DEBUG
                HitboxSprite = new Sprite(gameContext.Content.Load<Texture2D>("Sprites/PlayerHitbox")),
#endif
            };
            newBullet.Hitbox = new Hitbox(newBullet)
            {
                // For some reason, the bullet sprite is a little off-centered
                Offset = new Vector2(1f, 0f),
                Radius = 5,
            };
            return newBullet;
        }
    }
}
