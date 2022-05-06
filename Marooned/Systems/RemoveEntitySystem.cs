using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;

namespace Marooned.Systems
{
    [With(typeof(HealthComponent))] // all enemies
    public class RemoveEntitySystem : AEntitySetSystem<GameContext>
    {
        public RemoveEntitySystem(World world) : base(world)
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
