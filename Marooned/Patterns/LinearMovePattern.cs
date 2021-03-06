using Microsoft.Xna.Framework;
using System;

namespace Marooned.Patterns
{
    public class LinearMovePattern : MovePattern
    {
        public override Vector2 Transform(GameContext gameContext, TimeSpan totalTimeElapsed, TimeSpan duration, Vector2 currentPosition, Vector2 currentVelocity)
        {
            float dt = (float)gameContext.GameTime.ElapsedGameTime.TotalSeconds;
            return new Vector2(currentPosition.X + currentVelocity.X * dt, currentPosition.Y + currentVelocity.Y * dt);
        }
    }
}
