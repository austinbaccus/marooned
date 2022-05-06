using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;
using Microsoft.Xna.Framework;

namespace Marooned.Systems
{
    [With(typeof(TransformComponent), typeof(CollisionComponent), typeof(IsPlayerBulletComponent), typeof(HitboxComponent))] // player bullets
    public class EnemyCollisionSystem : AEntitySetSystem<GameContext>
    {
        public EnemyCollisionSystem(World world) : base(world)
        {
        }

        protected override void Update(GameContext gameContext, in Entity entity)
        {
            TransformComponent bulletPosition = entity.Get<TransformComponent>();
            HitboxComponent bulletHitbox = entity.Get<HitboxComponent>();

            foreach (Entity enemy in World.GetEntities().With<IsEnemyComponent>().With<TransformComponent>().AsSet().GetEntities())
            {
                if (Vector2.Distance(enemy.Get<TransformComponent>().Position, bulletPosition.Position) <= enemy.Get<HitboxComponent>().HitboxRadius + bulletHitbox.HitboxRadius)
                {
                    enemy.Set(new CollisionComponent
                    {
                        HasCollided = true,
                        CollidedWith = entity
                    });
                }
            }
        }
    }
}
