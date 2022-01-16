using Microsoft.Xna.Framework;
using Marooned.Sprites;

namespace Marooned
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        public void Follow(Sprite target)
        {
            float zoom = 2.0f;

            var position = Matrix.CreateTranslation(
              -target.Position.X - (target.Rectangle.Width / (2 / zoom)),
              -target.Position.Y - (target.Rectangle.Height / (2 / zoom)),
              0);

            var offset = Matrix.CreateTranslation(
                Game1.ScreenWidth / 2,
                Game1.ScreenHeight / 2,
                0);

            // https://roguesharp.wordpress.com/2014/07/13/tutorial-5-creating-a-2d-camera-with-pan-and-zoom-in-monogame/
            var scale = Matrix.CreateScale(new Vector3(zoom, zoom, 1));

            Transform = position * offset * scale;
        }
    }
}
