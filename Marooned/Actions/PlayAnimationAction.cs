using DefaultEcs;
using Marooned.Components;

namespace Marooned.Actions
{
    public class PlayAnimationAction : IAction
    {
        public PlayAnimationAction(Entity entity, string name)
        {
            Entity = entity;
            AnimationName = name;
        }

        public Entity Entity { get; set; }
        public string AnimationName { get; set; }

        public void Execute(GameContext gameContext)
        {
            if (Entity.Has<AnimationComponent>())
            {
                Entity.Get<AnimationComponent>().Stop();
                Entity.Get<AnimationComponent>().Play(AnimationName);
            }
        }
    }
}
