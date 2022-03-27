using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Marooned.States
{
    // TODO: Use MonoGame.Extended's Screen Management instead (?)
    public abstract class State
    {
        #region Fields

        public ContentManager content;

        public GraphicsDevice graphicsDevice;

        public Game1 game;

        #endregion

        #region Methods

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void PostUpdate(GameTime gameTime);

        public State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            this.game = game;

            this.graphicsDevice = graphicsDevice;

            this.content = content;
        }

        public abstract void Update(GameTime gameTime);

        #endregion
    }
}
