using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;
using System.Numerics;

namespace Marooned.Systems
{
    [With(typeof(TransformComponent), typeof(CollisionComponent), typeof(IsPlayerComponent), typeof(HitboxComponent))] // player
    public class EnemyBulletCollisionSystem : AEntitySetSystem<GameContext>
    {
        public EnemyBulletCollisionSystem(World world) : base(world)
        {
        }

        protected override void Update(GameContext gameContext, in Entity entity)
        {
            //TransformComponent playerPosition = entity.Get<TransformComponent>();
            //float PlayerX = playerPosition.Position.X;
            //float PlayerY = playerPosition.Position.Y;
            //HitboxComponent playerHitbox = entity.Get<HitboxComponent>();

            //foreach (Entity bullet in World.GetEntities().With<IsEnemyBulletComponent>().With<TransformComponent>().AsSet().GetEntities())
            //{
            //    float BulletX = bullet.Get<TransformComponent>().Position.X;
            //    float BulletY = bullet.Get<TransformComponent>().Position.Y;

            //    if (Vector2.Distance(new Vector2(BulletX, BulletY), new Vector2(PlayerX, PlayerY)) <= bullet.Get<HitboxComponent>().HitboxRadius + playerHitbox.HitboxRadius)
            //    {
            //        bullet.Set(new CollisionComponent
            //        {
            //            HasCollided = true,
            //            CollidedWith = entity
            //        });
            //    }
            //}
            EntitySet others = World.GetEntities().With<IsEnemyBulletComponent>().With<TransformComponent>().AsSet();

            Entity? collidedWith;
            if (Utils.CheckCollision(entity, others.GetEntities(), out collidedWith))
            {
                collidedWith.Value.Set(new CollisionComponent
                {
                    HasCollided = true,
                    CollidedWith = entity,
                });
            }
        }
    }
}
