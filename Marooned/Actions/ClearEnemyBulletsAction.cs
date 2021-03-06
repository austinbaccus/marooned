using DefaultEcs;
using Marooned.Components;

namespace Marooned.Actions
{
    public class ClearEnemyBulletsAction : IAction
    {
        private readonly World _world;

        public ClearEnemyBulletsAction(World world)
        {
            _world = world;
        }

        public void Execute(GameContext gameContext)
        {
            foreach (Entity entity in _world.GetEntities().With<IsEnemyBulletComponent>().AsSet().GetEntities())
            {
                entity.Dispose();
            }
        }
    }
}
