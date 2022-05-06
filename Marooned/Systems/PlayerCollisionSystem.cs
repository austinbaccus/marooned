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
            EntitySet others = World.GetEntities().With<IsPlayerComponent>().With<TransformComponent>().AsSet();

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
