using DefaultEcs;
using Marooned.Components;

namespace Marooned.Actions
{
    public class ClearEnemies
    {
        private readonly World _world;

        public ClearEnemies(World world)
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
