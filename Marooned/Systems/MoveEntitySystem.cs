using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;
using Microsoft.Xna.Framework;

namespace Marooned.Systems
{
    [With(typeof(TransformComponent), typeof(VelocityComponent), typeof(MoveComponent))]
    public class MoveEntitySystem : AEntitySetSystem<GameContext>
    {
        public MoveEntitySystem(World world, bool useBuffer = true) : base(world, useBuffer)
        {
        }

        protected override void Update(GameContext gameContext, in Entity entity)
        {
            TransformComponent transform = entity.Get<TransformComponent>();
            VelocityComponent velocity = entity.Get<VelocityComponent>();
            MoveComponent move = entity.Get<MoveComponent>();

            Vector2 currentPosition = transform.Position;
            Vector2 currentVelocity = velocity.Value;
            Vector2 newPosition = move.Pattern.Transform(gameContext, move.Timer, move.Duration, currentPosition, currentVelocity);

            entity.Set(new TransformComponent
            {
                Position = newPosition
            });

            if (move.IsFinished)
            {
                entity.Remove<MoveComponent>();
            }
        }
    }
}
