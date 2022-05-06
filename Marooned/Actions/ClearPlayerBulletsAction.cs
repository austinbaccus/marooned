using DefaultEcs;
using Marooned.Components;

namespace Marooned.Actions
{
    internal class ClearPlayerBulletsAction : IAction
    {
        private readonly World _world;

        public ClearPlayerBulletsAction(World world)
        {
            _world = world;
        }

        public void Execute(GameContext gameContext)
        {
            foreach (Entity entity in _world.GetEntities().With<IsPlayerBulletComponent>().AsSet().GetEntities())
            {
                entity.Dispose();
            }
        }
    }
}
