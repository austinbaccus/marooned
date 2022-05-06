using Microsoft.Xna.Framework;
using System;

namespace Marooned.Patterns
{
    public class CircleMovePattern : MovePattern
    {
        public CircleMovePattern(int degrees, Vector2 centerOffset)
        {
            Degrees = degrees;
            CenterOffset = centerOffset;
        }

        public Vector2 Origin { get; set; } = Vector2.Zero;
        public int Degrees { get; set; }
        public Vector2 CenterOffset { get; set; }
        public float Radius { get => Vector2.Distance(Origin, CenterOffset); }
        //public int StartingDegree { get => Degrees + Math.Acos() }

        public override Vector2 Transform(GameContext gameContext, TimeSpan totalTimeElapsed, TimeSpan duration, Vector2 currentPosition, Vector2 currentVelocity)
        {
            //double xParam = Utils.Lerp(totalTimeElapsed.TotalSeconds / currentVelocity.X, Degrees,;
            //float newX = Origin.X + (float)Math.Cos(totalTimeElapsed.TotalSeconds) * (CenterOffset.X - Origin.X);
            //float newY = Origin.Y + (float)Math.Sin(totalTimeElapsed.TotalSeconds / currentVelocity.Y) * (CenterOffset.X - Origin.Y);
            //return new Vector2(newX, newY);
            throw new NotImplementedException();
        }
    }
}
