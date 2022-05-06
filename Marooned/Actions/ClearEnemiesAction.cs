using DefaultEcs;
using Marooned.Components;

namespace Marooned.Actions
{
    public class ClearEnemiesAction : IAction
    {
        private readonly World _world;

        public ClearEnemiesAction(World world)
        {
            _world = world;
        }

        public void Execute(GameContext gameContext)
        {
            foreach (Entity entity in _world.GetEntities().With<IsEnemyComponent>().AsSet().GetEntities())
            {
                entity.Dispose();
            }
        }
    }
}
