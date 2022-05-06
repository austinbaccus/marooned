using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;

namespace Marooned.Systems
{
    public class SpawnSystem : AComponentSystem<GameContext, SpawnComponent>
    {
        public SpawnSystem(World world) : base(world)
        {
        }

        protected override void Update(GameContext gameContext, ref SpawnComponent spawn)
        {
            if (spawn.ShouldSpawn)
            {
                spawn.RateTimer = 0;
            }
            else
            {
                spawn.RateTimer += gameContext.GameTime.ElapsedGameTime.TotalSeconds;
            }
            spawn.Timer = spawn.Timer.Add(gameContext.GameTime.ElapsedGameTime);
        }
    }
}
