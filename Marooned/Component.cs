using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Marooned
{
    public abstract class Component
    {
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch); // Draws a texture on screen

        public abstract void Update(GameTime gameTime); // Updates any component information
    }
}
