using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;
using System.Numerics;

namespace Marooned.Systems
{
    [With(typeof(TransformComponent), typeof(CollisionComponent), typeof(IsEnemyBulletComponent), typeof(HitboxComponent))] // enemy bullets
    public class PlayerCollisionSystem : AEntitySetSystem<GameContext>
    {
        public PlayerCollisionSystem(World world) : base(world)
        {
        }

        protected override void Update(GameContext gameContext, in Entity entity)
        {
            TransformComponent bulletPosition = entity.Get<TransformComponent>();
            float BulletX = bulletPosition.Position.X;
            float BulletY = bulletPosition.Position.Y;
            HitboxComponent bulletHitbox = entity.Get<HitboxComponent>();

            foreach (Entity player in World.GetEntities().With<IsPlayerComponent>().With<TransformComponent>().AsSet().GetEntities())
            {
                float PlayerX = player.Get<TransformComponent>().Position.X;
                float PlayerY = player.Get<TransformComponent>().Position.Y;

                if (Vector2.Distance(new Vector2(PlayerX, PlayerY), new Vector2(BulletX, BulletY)) <= player.Get<HitboxComponent>().HitboxRadius + bulletHitbox.HitboxRadius) 
                {
                    player.Set(new CollisionComponent
                    {
                        CollidedWith = entity
                    });

                }
            }
        }
    }
}
