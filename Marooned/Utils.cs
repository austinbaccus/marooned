using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
    }
}
