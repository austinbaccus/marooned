using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;

namespace Marooned.Systems
{
    [With(typeof(TransformComponent), typeof(CollisionComponent), typeof(HitboxComponent))] // player and enemies
    [WithEither(typeof(IsEnemyComponent), typeof(IsPlayerComponent))]
    public class DamageSystem : AEntitySetSystem<GameContext>
    {
        public DamageSystem(World world) : base(world)
        {
        }

        protected override void Update(GameContext gameContext, in Entity entity)
        {
            if (entity.Get<CollisionComponent>().CollidedWith != null)
            {
                entity.Set(new CollisionComponent
                {
                    CollidedWith = null
                });
                entity.Get<HealthComponent>().Health -= 1;
            }
        }
    }
}
