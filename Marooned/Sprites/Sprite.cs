using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Marooned.Sprites
{
    public class Sprite : ComponentOld
    {
        internal Texture2D _texture;
        public Vector2 Position;

        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }

        public virtual Rectangle Rectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height); }
        }

        public virtual Rectangle? Destination { get; set; } = null;

        // The origin of all sprites should be the center, rather than top-left, so that position calculation is much more simple and does
        // not have to take into account the sprite width and height.
        public virtual Vector2 Origin
        {
            get { return new Vector2(_texture.Width / 2f, _texture.Height / 2f); }
        }

        public virtual float Scale { get; set; } = 1f;

        public virtual Color Color { get; set; } = Color.White;

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Destination is Rectangle destination)
            {
                spriteBatch.Draw(
                    texture: _texture,
                    destinationRectangle: destination,
                    sourceRectangle: null,
                    color: Color,
                    rotation: 0f,
                    origin: Origin,
                    effects: SpriteEffects.None,
                    layerDepth: 0f
               );
            }
            else
            {
                spriteBatch.Draw(
                    texture: _texture,
                    position: Position,
                    sourceRectangle: null,
                    color: Color,
                    rotation: 0f,
                    origin: Origin,
                    scale: Scale,
                    effects: SpriteEffects.None,
                    layerDepth: 0f
                );
            }
        }
    }
}
