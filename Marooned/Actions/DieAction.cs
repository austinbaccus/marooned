using DefaultEcs;

namespace Marooned.Actions
{
    public class DieAction : IAction
    {
        private readonly Entity _entity;

        public DieAction(Entity entity)
        {
            _entity = entity;
        }

        public void Execute(GameContext gameContext)
        {
            _entity.Dispose();
        }
    }
}
