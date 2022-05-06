using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;

namespace Marooned.Systems
{
    [With(typeof(TransformComponent), typeof(CollisionComponent), typeof(HitboxComponent))] // all bullets
    [WithEither(typeof(IsEnemyBulletComponent), typeof(IsPlayerBulletComponent))]
    public class BulletRemovalSystem : AEntitySetSystem<GameContext>
    {
        public BulletRemovalSystem(World world) : base(world, true)
        {
        }

        protected override void Update(GameContext gameContext, in Entity entity)
        {
           if (entity.Get<CollisionComponent>().HasCollided)
           {
                entity.Dispose();
           }
        }
    }
}
