using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;

namespace Marooned.Systems
{
    [With(typeof(TransformComponent), typeof(CollisionComponent), typeof(HitboxComponent), typeof(IsEnemyComponent))] // all enemies
    public class RemoveEnemySystem : AEntitySetSystem<GameContext>
    {
        public RemoveEnemySystem(World world) : base(world)
        {
        }

        protected override void Update(GameContext gameContext, in Entity entity)
        {
            if (entity.Get<HealthComponent>().Health <= 0)
            {
                entity.Dispose();
            }
        }
    }
}
