using DefaultEcs;
using Marooned.Components;
using Marooned.Patterns;
using Microsoft.Xna.Framework;
using System;

namespace Marooned.Actions
{
    public class LinearMoveAction : MoveAction
    {
        public LinearMoveAction(MovePattern pattern, Entity entity, TimeSpan duration, Vector2 velocity) : base(pattern, entity, duration)
        {
            Velocity = velocity;
        }

        public Vector2 Velocity { get; set; }

        public override void Execute(GameContext gameContext)
        {
            Entity.Set(new VelocityComponent
            {
                Value = Velocity,
            });
            base.Execute(gameContext);
        }
    }
}
