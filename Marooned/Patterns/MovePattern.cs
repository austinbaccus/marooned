using Microsoft.Xna.Framework;
using System;

namespace Marooned.Patterns
{
    public abstract class MovePattern
    {
        public abstract Vector2 Transform(GameContext gameContext, TimeSpan totalTimeElapsed, TimeSpan duration, Vector2 currentPosition, Vector2 currentVelocity);
    }
}
