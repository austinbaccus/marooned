using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

using Marooned.Sprites;

namespace Marooned
{
    public class Camera
    {
        private float _zoom = 0.45f;

        public OrthographicCamera CameraOrtho;

        public Camera(OrthographicCamera orthographicCamera)
        {
            CameraOrtho = orthographicCamera;
            CameraOrtho.Zoom = 1f / _zoom;
        }

        public Matrix Transform { get; private set; }
        public float Zoom {
            get
            {
                return _zoom;
            }
            set
            {
                _zoom = value;
                CameraOrtho.Zoom = 1f / _zoom;
            }
        }

        public void Follow(Sprite target, Vector2 offset, GraphicsDevice graphics)
        {
            var scale = Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
            //Transform = position * offset * scale;

            // https://roguesharp.wordpress.com/2014/07/13/tutorial-5-creating-a-2d-camera-with-pan-and-zoom-in-monogame/
            var pos = Matrix.CreateTranslation(
                (target.Position.X + offset.X) - Zoom * graphics.Viewport.Width / 2f, 
                (target.Position.Y + offset.Y) - Zoom * graphics.Viewport.Height / 2f, 
                0);

            var transform = scale * pos;
            Transform = Matrix.Invert(transform);
        }
    }
}
