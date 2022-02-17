﻿using Microsoft.Xna.Framework;
using Marooned.Sprites;
using Microsoft.Xna.Framework.Graphics;

namespace Marooned
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        public void Follow(Sprite target)
        {
            float zoom = 0.45f; // 0.45f is good

            var position = Matrix.CreateTranslation(
              -target.Position.X - (target.Rectangle.Width / 2),
              -target.Position.Y - (target.Rectangle.Height / 2),
              0);

            var offset = Matrix.CreateTranslation(
                Game1.ScreenWidth / 2,
                Game1.ScreenHeight / 2,
                0);

            var scale = Matrix.CreateScale(new Vector3(zoom, zoom, 1));
            //Transform = position * offset * scale;

            // https://roguesharp.wordpress.com/2014/07/13/tutorial-5-creating-a-2d-camera-with-pan-and-zoom-in-monogame/
            var pos = Matrix.CreateTranslation(
                target.Position.X - 150, // - (target.Rectangle.Width / 2), 
                target.Position.Y - 100, // - (target.Rectangle.Height / 2), 
                0);

            var transform = scale * pos;
            Transform = Matrix.Invert(transform);

        }
    }
}
