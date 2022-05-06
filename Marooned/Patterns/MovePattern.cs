using Microsoft.Xna.Framework;

namespace Marooned.Patterns
{
    public abstract class MovePattern
    {
        public abstract Vector2 Transform(GameContext gameContext, Vector2 currentPosition, Vector2 currentVelocity);
    }
}
