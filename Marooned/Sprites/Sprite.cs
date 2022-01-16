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
    public class Sprite : Component
    {
        internal Texture2D _texture;
        public Vector2 Position;

        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }

        public Rectangle Rectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height); }
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }
    }
}
