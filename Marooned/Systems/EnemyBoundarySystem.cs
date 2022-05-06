using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;

namespace Marooned.Systems
{
    [With(typeof(TransformComponent), typeof(CollisionComponent), typeof(IsEnemyComponent), typeof(HitboxComponent))] // all enemies
    public class EnemyBoundarySystem : AEntitySetSystem<GameContext>
    {
        public EnemyBoundarySystem(World world) : base(world, true)
        {
        }

        protected override void Update(GameContext gameContext, in Entity entity)
        {
            TransformComponent enemyPosition = entity.Get<TransformComponent>();

            if (enemyPosition.Position.X > 500 || enemyPosition.Position.Y > 500 || enemyPosition.Position.X < 0 || enemyPosition.Position.Y < 0)
            {
                entity.Get<HealthComponent>().Health = 0;
            }
        }
    }
}
