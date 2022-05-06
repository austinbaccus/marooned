using DefaultEcs;
using Marooned.Components;
using Marooned.Patterns;
using System;

namespace Marooned.Actions
{
    public class MoveAction : IAction
    {
        public MoveAction(MovePattern pattern, Entity entity, float duration)
        {
            Pattern = pattern;
            Entity = entity;
            Duration = duration;
        }

        public MovePattern Pattern { get; set; }
        public Entity Entity { get; set; }
        public float Duration { get; set; }

        public void Execute(GameContext gameContext)
        {
            Entity.Set(new MoveComponent
            {
                Pattern = Pattern,
                Duration = Duration,
                Timer = TimeSpan.Zero
            });
        }
    }
}
