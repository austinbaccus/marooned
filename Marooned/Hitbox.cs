using Microsoft.Xna.Framework;

using Marooned.Sprites;

namespace Marooned
{
    // Note: The drawing of the hitbox sprite is NOT handled by the hitbox class itself.
    //       That is delegated to the instance using the hitbox instead
    //       (e.g., Player draws the hitbox inside its Draw() method).
    public class Hitbox
    {
        // This is the sprite using hitbox, not the actual sprite of hitbox itself.
        private Sprite _parentSprite;

        public Vector2 Position
        {
            get
            {
                return _parentSprite.Position + Offset;
            }
        }
        public Vector2 Offset { get; set; } = Vector2.Zero;
        
        public int Radius { get; set; }

        public Hitbox(Sprite sprite)
        {
            _parentSprite = sprite;
        }

        // TODO: Come up with a better way of detecting collision. Specifically:
        //       1) Better code structure that does not rely on a reference to Sprite for its position. Maybe a separate class handles collision entirely?
        //       2) Efficiency, if need be. Possible solutions: Space Partitioning (Quadtrees), Sweep and Prune
        //       3) (this - other) <= radiiSum^2 instead of sqrt(this - other) <= radiiSum (sqrting is a slow operation in comparison to squaring)
        public bool IsTouching(Hitbox other)
        {
            var radiiSum = Radius + other.Radius;
            var distance = Vector2.Distance(Position, other.Position);
            return distance <= radiiSum;
        }
    }
}
