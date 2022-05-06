using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;

namespace Marooned.Systems
{
    public class MoveSystem : AComponentSystem<GameContext, MoveComponent>
    {
        public MoveSystem(World world) : base(world)
        {
        }

        protected override void Update(GameContext gameContext, ref MoveComponent move)
        {
            move.Timer.Add(gameContext.GameTime.ElapsedGameTime);
        }
    }
}
