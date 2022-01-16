using Microsoft.Xna.Framework;
using Marooned.Sprites;

namespace Marooned
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        public void Follow(Sprite target)
        {
            var position = Matrix.CreateTranslation(
              -target.Position.X - (target.Rectangle.Width / 2),
              -target.Position.Y - (target.Rectangle.Height / 2),
              0);

            var offset = Matrix.CreateTranslation(
                Game1.ScreenWidth / 2,
                Game1.ScreenHeight / 2,
                0);

            Transform = position * offset;
        }
    }
}
