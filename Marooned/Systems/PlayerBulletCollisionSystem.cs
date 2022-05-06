using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;
using System.Numerics;

namespace Marooned.Systems
{
    [With(typeof(TransformComponent), typeof(CollisionComponent), typeof(IsEnemyComponent), typeof(HitboxComponent))] // enemy
    public class PlayerBulletCollisionSystem : AEntitySetSystem<GameContext>
    {
        public PlayerBulletCollisionSystem(World world) : base(world)
        {
        }

        protected override void Update(GameContext gameContext, in Entity entity)
        {
            TransformComponent enemyPosition = entity.Get<TransformComponent>();
            float EnemyX = enemyPosition.Position.X;
            float EnemyY = enemyPosition.Position.Y;
            HitboxComponent enemyHitbox = entity.Get<HitboxComponent>();

            foreach (Entity bullet in World.GetEntities().With<IsPlayerBulletComponent>().With<TransformComponent>().AsSet().GetEntities())
            {
                float BulletX = bullet.Get<TransformComponent>().Position.X;
                float BulletY = bullet.Get<TransformComponent>().Position.Y;

                if (Vector2.Distance(new Vector2(BulletX, BulletY), new Vector2(EnemyX, EnemyY)) <= bullet.Get<HitboxComponent>().HitboxRadius + enemyHitbox.HitboxRadius)
                {
                    bullet.Set(new CollisionComponent
                    {
                        CollidedWith = entity
                    });
                }
            }
        }
    }
}
