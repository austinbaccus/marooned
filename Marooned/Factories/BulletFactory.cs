using Marooned.Components;
using DefaultEcs;
using Microsoft.Xna.Framework;

namespace Marooned.Factories
{
    public static class BulletFactory
    {
        public static Entity MakeBullet(GameContext gameContext, World world, string name, Vector2 at)
        {
            Entity entity = gameContext.EntitiesInterpreter.CreateEntityFrom(world, name);
            if (!entity.Has<TransformComponent>())
            {
                entity.Set(new TransformComponent()
                {
                    Position = at,
                });
            }
            entity.Get<AnimationComponent>().Play("spin");
            return entity;

//            var sprite = new MonoGame.Extended.Sprites.AnimatedSprite(spriteSheet);
//            sprite.Play("idle");

//            Bullet newBullet = new Bullet(sprite, lifeSpan, linearVelocity, damage, origin)
//            {
//#if DEBUG
//                HitboxSprite = new Sprite(gameContext.Content.Load<Texture2D>("Sprites/PlayerHitbox")),
//#endif
//            };
//            newBullet.Hitbox = new Hitbox(newBullet)
//            {
//                // For some reason, the bullet sprite is a little off-centered
//                Offset = new Vector2(1f, 0f),
//                Radius = 5,
//            };
//            return newBullet;
        }
    }
}
