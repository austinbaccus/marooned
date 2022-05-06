using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;

namespace Marooned.Systems
{
    [With(typeof(TransformComponent), typeof(CollisionComponent), typeof(HitboxComponent))] // all bullets
    [WithEither(typeof(IsEnemyBulletComponent), typeof(IsPlayerBulletComponent))]
    public class BulletBoundarySystem : AEntitySetSystem<GameContext>
    {
        public BulletBoundarySystem(World world) : base(world, true)
        {
        }

        protected override void Update(GameContext gameContext, in Entity entity)
        {
            TransformComponent bulletPosition = entity.Get<TransformComponent>();

            if (bulletPosition.Position.X >= 500 || bulletPosition.Position.Y >= 500 || bulletPosition.Position.X <= 0 || bulletPosition.Position.Y <= 0)
            {
                entity.Set(new CollisionComponent
                {
                    HasCollided = true,
                });
            }
        }
    }
}
