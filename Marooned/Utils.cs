using DefaultEcs;
using Marooned.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Marooned
{
    public static class Utils
    {
        public static Vector2 GetCenterPos(int buttonWidth, int buttonHeight, Viewport viewport)
        {
            return new Vector2(
                viewport.Width / 2 - buttonWidth / 2,
                viewport.Height / 2 - buttonHeight / 2
            );
        }

        public static float Lerp(float start, float goal, float t)
        {
            return start + (goal - start) * t;
        }

        public static double Lerp(double start, double goal, double t)
        {
            return start + (goal - start) * t;
        }

        public static bool CheckCollision(Entity entity, ReadOnlySpan<Entity> others, out Entity? collidedWith)
        {
            TransformComponent t1 = entity.Get<TransformComponent>();
            HitboxComponent h1 = entity.Get<HitboxComponent>();
            foreach (Entity other in others)
            {
                TransformComponent t2 = other.Get<TransformComponent>();
                HitboxComponent h2 = other.Get<HitboxComponent>();

                if (Vector2.Distance(t1.Position, t2.Position) <= h1.HitboxRadius + h2.HitboxRadius)
                {
                    collidedWith = other;
                    return true;
                }
            }
            collidedWith = null;
            return false;
        }
    }
}
