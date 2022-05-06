using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;
using System.Numerics;

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
            float BulletX = bulletPosition.Position.X;
            float BulletY = bulletPosition.Position.Y;
            HitboxComponent bulletHitbox = entity.Get<HitboxComponent>();

            foreach (Entity enemy in World.GetEntities().With<IsEnemyComponent>().With<TransformComponent>().AsSet().GetEntities())
            {
                float EnemyX = enemy.Get<TransformComponent>().Position.X;
                float EnemyY = enemy.Get<TransformComponent>().Position.Y;

                if (Vector2.Distance(new Vector2(EnemyX, EnemyY), new Vector2(BulletX, BulletY)) <= enemy.Get<HitboxComponent>().HitboxRadius + bulletHitbox.HitboxRadius)
                {
                    enemy.Set(new CollisionComponent
                    {
                        CollidedWith = entity
                    });
                }
            }
        }
    }
}
