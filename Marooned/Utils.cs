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
    }
}
