using DefaultEcs;
using Marooned.Components;
using Marooned.Patterns;
using System;

namespace Marooned.Actions
{
    public class MoveAction : IAction
    {
        public MoveAction(MovePattern pattern, Entity entity, TimeSpan duration)
        {
            Pattern = pattern;
            Entity = entity;
            Duration = duration;
        }

        public MovePattern Pattern { get; set; }
        public Entity Entity { get; set; }
        public TimeSpan Duration { get; set; }

        public virtual void Execute(GameContext gameContext)
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
